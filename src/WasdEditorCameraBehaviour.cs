using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
//using System.Reflection;
using System.Runtime.InteropServices;

namespace WasdEditorCamera
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class WasdEditorCameraBehaviour : MonoBehaviour
    {
        private readonly Bounds ZERO_BOUNDS = new Bounds(Vector3.zero, Vector3.zero);

        private readonly ScreenMessage MESSAGE_TEMPLATE = new ScreenMessage(String.Empty, 1, ScreenMessageStyle.UPPER_CENTER);

        private delegate void CleanupFn();

        private CleanupFn OnCleanup;

        //private KerbalFSM editorFSM;
        private CursorLocker cursorLocker;
        private static Bounds movementBounds;
        private Vector3 partOffset;
        private bool mouseWasDown;
        private bool movePart;

        private float yaw;
        private float pitch;
        private Vector3 pos;
        private Vector3 vel;

        private bool cameraEnabled, selectedPart;

        public MainMenuGui gui = null;

        public static Config config = new Config();

        static bool hangerExtenderInstalled = false;

        static public bool hasMod(string modIdent)
        {
            foreach (AssemblyLoader.LoadedAssembly a in AssemblyLoader.loadedAssemblies)
            {
                if (modIdent == a.name)
                    return true;

            }
            return false;
        }

        static bool ReadConfigFile()
        {
            string fname = MainMenuGui.WASD_CFG_FILE;
            if (System.IO.File.Exists(fname))
            {

                ConfigNode file = ConfigNode.Load(fname);
                var root = file.GetNode(MainMenuGui.WASD_NODENAME);

                config.parseConfigNode(root);
                return true;
            }
            return false;
        }

        void UpdateForEditor()
        {
            checkMovementBounds();
            if (config.defaultCamera)
            {
                SwitchMode(false);
                ResetCamera();
            }
        }
        public static void setConfig(ConfigNode config)
        {
            //GameDatabase.Instance.GetConfigs (MainMenuGui.WASD_NODENAME).First ().config = config;
            checkMovementBounds();
        }


        static void readConfig()
        {

            Log.Debug("Loading config...");
            if (!ReadConfigFile())
            {
                config.initConfig();
            }
            //var root = GameDatabase.Instance.GetConfigs (MainMenuGui.WASD_NODENAME).First ().config;
            //Log.Info ("Before parseConfigNode");
            //config.parseConfigNode (root);
            //Log.Info ("end of readConfig");
        }
        static EditorFacility lastEditorMode;
        public static void checkMovementBounds()
        {
            lastEditorMode = EditorDriver.editorFacility;
            hangerExtenderInstalled = hasMod("FShangarExtender");
            movementBounds = new Bounds();
            if (EditorDriver.editorFacility == EditorFacility.VAB)
            {
                movementBounds = config.vab.bounds;
            }
            else if (EditorDriver.editorFacility == EditorFacility.SPH)
            {
                movementBounds = config.sph.bounds;
            }
            if (!config.enforceBounds || hangerExtenderInstalled)
            {
                movementBounds = new Bounds(Vector3.zero, Vector3.one * float.MaxValue);
            }
        }

        public void Start()
        {
            Log.SetTitle("this.GetType ().Name");

            Log.Debug("Start");
#if (DEBUG)
            Log.SetLevel(Log.LEVEL.INFO);
            Log.Debug("Start 2");
            gui = null;
#endif

            if (gui == null)
            {
                gui = this.gameObject.AddComponent<MainMenuGui>();
                gui.UpdateToolbarStock();
                gui.SetVisible(false);

            }

            readConfig();

            // KspIssue3838Fix.ApplyFix (config.enableExperimentalEditorExtensionsCompatibility);

            //editorFSM = (KerbalFSM)Refl.GetValue (EditorLogic.fetch, "\u0001");
            //Log.Info ("editorFSM: " + editorFSM.ToString ());

            cursorLocker = Application.platform == RuntimePlatform.WindowsPlayer ? new WinCursorLocker() : (CursorLocker)new UnityLocker();




            var restartListener = new EventVoid.OnEvent(this.OnEditorRestart);
            GameEvents.onEditorRestart.Add(restartListener);
            OnCleanup += () => GameEvents.onEditorRestart.Remove(restartListener);

            var partEventListener = new EventData<ConstructionEventType, Part>.OnEvent(this.OnPartEvent);
            GameEvents.onEditorPartEvent.Add(partEventListener);
            OnCleanup += () => GameEvents.onEditorPartEvent.Remove(partEventListener);

            UpdateForEditor();
        }

        public void OnDestroy()
        {
            Log.Debug("OnDestroy, WasdEditorCameraBehaviour.cs");
            if (OnCleanup != null)
                OnCleanup();
            Log.Debug("Cleanup complete.");
        }

        private void OnEditorRestart()
        {
            if (!cameraEnabled && config.defaultCamera)
            {
                SwitchMode(false);
            }
            ResetCamera();
        }

        public void Update()
        {
            if (lastEditorMode != EditorDriver.editorFacility)
            {
                checkMovementBounds();
                if (config.defaultCamera)
                {
                    SwitchMode(false, false, true);
                    ResetCamera();
                }
                //UpdateForEditor();
            }

            if (gui == null)
            {
                gui = this.gameObject.AddComponent<MainMenuGui>();
                gui.UpdateToolbarStock();
                gui.SetVisible(false);

            }

            gui.set_WASD_Button_active(cameraEnabled && EditorLogic.SelectedPart == null);
        }

        private void OnPartEvent(ConstructionEventType type, Part part)
        {
            if (type == ConstructionEventType.PartCreated && part == EditorLogic.RootPart)
            {
                ResetCamera();
            }
        }

        private void SwitchMode(bool showMessage = true, bool partSelectedTmp = false, bool leaveCameraEnabled = false)
        {
            if (EditorLogic.SelectedPart != null)
            {
                if (showMessage)
                    ScreenMessages.PostScreenMessage("Can't change modes while part is selected", MESSAGE_TEMPLATE);
                return;
            }
            if (!leaveCameraEnabled)
                cameraEnabled = !cameraEnabled;

            selectedPart = partSelectedTmp;

            if (gui) gui.set_WASD_Button_active(cameraEnabled);
            if (!cameraEnabled || EditorLogic.SelectedPart != null)
            {
                cursorLocker.UnlockCursor();
                //				Screen.showCursor = true;
                mouseWasDown = false;
                movePart = false;
                EditorBounds.Instance.constructionBounds = movementBounds;
                EditorBounds.Instance.cameraOffsetBounds = movementBounds;
                EditorBounds.Instance.cameraMaxDistance = movementBounds.extents.magnitude;

                Vector3 point;
                float distance;
                var rayHit = GetNewFocalPoint(out point, out distance);
                Log.Info("point: " + point.ToString() + "   distance: " + distance.ToString());

                if (EditorDriver.editorFacility == EditorFacility.VAB)
                {
                    // Try to keep camera in place but looking at central pillar. VABCamera doesn't have xz offset.
#if true
                    var cam = (VABCamera)EditorLogic.fetch.editorCamera.gameObject.GetComponent(typeof(VABCamera));
                    if (!rayHit)
                    {
                        var xzOffset = EditorLogic.fetch.editorCamera.gameObject.transform.position;
                        xzOffset.y = 0;
                        var xzOffsetDistance = xzOffset.magnitude;
                        var yOffset = -1 * Mathf.Tan(pitch / 180 * Mathf.PI) * xzOffsetDistance;
                        yOffset = Mathf.Clamp(yOffset, -20, 20);

                        point = new Vector3(0, pos.y + yOffset, 0);
                        point = GetClosestPointOnBounds(movementBounds, point);
                        distance = (EditorLogic.fetch.editorCamera.gameObject.transform.position - point).magnitude;
                    }
                    else
                    {
                        point = new Vector3(0, point.y, 0);
                    }

                    cam.PlaceCamera(point, distance);

                    var lookDir = (point - pos).normalized;
                    var rot = Quaternion.LookRotation(lookDir);
                    cam.camPitch = pitch * Mathf.PI / 180;
                    cam.camHdg = rot.eulerAngles.y * Mathf.PI / 180;

                    //					StartCoroutine (TurnSmoothingOffForOneFrame (cam));
                    cam.enabled = true;
#else

					var cam = (SPHCamera)EditorLogic.fetch.editorCamera.gameObject.GetComponent (typeof(SPHCamera));
					Log.Info("cam.maxDisplaceX: " + cam.maxDisplaceX.ToString() + "     movementBounds.extents.x: " + movementBounds.extents.x.ToString());
					Log.Info("cam.maxDisplaceZ: " + cam.maxDisplaceZ.ToString() + "     movementBounds.extents.z: " + movementBounds.extents.z.ToString());
					cam.maxDisplaceX = movementBounds.extents.x;
					cam.maxDisplaceZ = movementBounds.extents.z;

					cam.PlaceCamera (point, distance);
					cam.camPitch = pitch * Mathf.PI / 180;
					cam.camHdg = yaw * Mathf.PI / 180;
					StartCoroutine (TurnSmoothingOffForOneFrame (cam));
					cam.enabled = true;
				
#endif
                }
                else if (EditorDriver.editorFacility == EditorFacility.SPH)
                {
                    var cam = (SPHCamera)EditorLogic.fetch.editorCamera.gameObject.GetComponent(typeof(SPHCamera));
                    cam.maxDisplaceX = movementBounds.extents.x;
                    cam.maxDisplaceZ = movementBounds.extents.z;

                    cam.PlaceCamera(point, distance);
                    cam.camPitch = pitch * Mathf.PI / 180;
                    cam.camHdg = yaw * Mathf.PI / 180;
                    //					StartCoroutine (TurnSmoothingOffForOneFrame (cam));
                    cam.enabled = true;
                }

                if (EditorLogic.SelectedPart != null && EditorLogic.fetch.EditorConstructionMode == ConstructionMode.Place)
                {
                    //					UpdateDragPlane ();
                }
                if (showMessage)
                    ScreenMessages.PostScreenMessage("Switched to stock camera", MESSAGE_TEMPLATE);
            }
            else
            {
                ((MonoBehaviour)EditorLogic.fetch.editorCamera.gameObject.GetComponent(typeof(VABCamera))).enabled = false;
                ((MonoBehaviour)EditorLogic.fetch.editorCamera.gameObject.GetComponent(typeof(SPHCamera))).enabled = false;
                pos = EditorLogic.fetch.editorCamera.transform.position;
                pitch = EditorLogic.fetch.editorCamera.transform.rotation.eulerAngles.x;
                if (pitch > 180)
                {
                    pitch -= 360;
                }
                yaw = EditorLogic.fetch.editorCamera.transform.rotation.eulerAngles.y;
                if (showMessage)
                    ScreenMessages.PostScreenMessage("Switched to wasd camera", MESSAGE_TEMPLATE);
            }
        }

        private bool GetNewFocalPoint(out Vector3 point, out float distance)
        {
            Log.Info("GetNewFocalPoint");
            Log.Info("EditorLogic.fetch.editorCamera.transform.position: " + EditorLogic.fetch.editorCamera.transform.position.ToString());
            Log.Info("EditorLogic.fetch.editorCamera.transform.TransformDirection (Vector3.forward): " + EditorLogic.fetch.editorCamera.transform.TransformDirection(Vector3.forward).ToString());

            var MAX_DISTANCE = 10f;
            Ray ray = new Ray(EditorLogic.fetch.editorCamera.transform.position,
                          EditorLogic.fetch.editorCamera.transform.TransformDirection(Vector3.forward));
            RaycastHit hit;
            distance = MAX_DISTANCE;
            point = Vector3.zero;
            if (Physics.Raycast(ray, out hit, MAX_DISTANCE))
            {
                //log.Debug("DISTANCE " + hit.distance);
                var part = (Part)hit.transform.gameObject.GetComponent(typeof(Part));
                if (part)
                {
                    distance = (hit.point - EditorLogic.fetch.editorCamera.transform.position).magnitude;
                    point = hit.point;
                    return true;
                }
                else
                {
                    distance = hit.distance;
                }
            }
            point = pos + EditorLogic.fetch.editorCamera.transform.TransformVector(Vector3.forward * distance);
            return false;
        }

        private void ResetCamera()
        {
            if (EditorDriver.editorFacility == EditorFacility.VAB)
            {
                pitch = config.vab.initialPitch;
                yaw = config.vab.initialYaw;
                pos = config.vab.initialPosition;
            }
            else if (EditorDriver.editorFacility == EditorFacility.SPH)
            {
                pitch = config.sph.initialPitch;
                yaw = config.sph.initialYaw;
                pos = config.sph.initialPosition;
            }
            vel = Vector3.zero;
        }

        public void LateUpdate()
        {
            if (HighLogic.LoadedScene != GameScenes.EDITOR || EditorLogic.fetch == null || !MainMenuGui.active)
                return;

            GameObject obj = EventSystem.current.currentSelectedGameObject;
            bool inputFieldIsFocused = (obj != null && obj.GetComponent<TMPro.TMP_InputField>() != null && obj.GetComponent<TMPro.TMP_InputField>().isFocused);
            if (inputFieldIsFocused)
                return;

            if (Input.GetKeyDown(config.keySwitchMode))
            {
                SwitchMode();
            }

            //			if (selectedPart && EditorLogic.SelectedPart == null)
            //				SwitchMode (false, false);
            if (!cameraEnabled)
                return;
            //			if (EditorLogic.SelectedPart != null) {
            //				SwitchMode (false, true);
            //				return;
            //			}

            cursorLocker.LockUpdate();

            bool isDown = Input.GetKey(KeyCode.Mouse1);
            int p = (int)Environment.OSVersion.Platform;
            if ((p == 4) || (p == 6) || (p == 128))
            {
                isDown = Input.GetKey(KeyCode.Mouse2);
                //} else {
                //	isDown = Input.GetKey (KeyCode.Mouse1);
            }

            bool goneDown = isDown && !mouseWasDown;
            bool goneUp = !isDown && mouseWasDown;
            mouseWasDown = isDown;

            bool shiftIsDown = Input.GetKey(KeyCode.LeftShift);

            var isTweaking = (Input.GetMouseButton(0)
                && (EditorLogic.fetch.currentStateName == "st_offset_tweak" || EditorLogic.fetch.currentStateName == "st_rotate_tweak"));
            if (isTweaking)
                return;

            float dx = 0, dy = 0;
            if (isDown)
            {
                dx = Input.GetAxis("Mouse X");
                dy = Input.GetAxis("Mouse Y");
                yaw += 0.22f * dx * config.sensitivity;
                yaw %= 360;
                pitch += -0.22f * dy * config.sensitivity;
                pitch = Mathf.Clamp(pitch, -90, 90);
            }

            if (goneDown)
            {
                // Locks part rotation with WASD keys. Doesn't actually seem to affect gizmos.
                InputLockManager.SetControlLock(ControlTypes.EDITOR_GIZMO_TOOLS, this.GetType().Name);

                if (EditorLogic.SelectedPart != null)
                {
                    partOffset = EditorLogic.SelectedPart.transform.position - EditorLogic.fetch.editorCamera.transform.position;
                    partOffset += EditorLogic.fetch.selPartGrabOffset;
                    partOffset = EditorLogic.fetch.editorCamera.transform.InverseTransformVector(partOffset);
                }

                //				Screen.showCursor = false;
                cursorLocker.PrepareLock();
                cursorLocker.LockCursor();
            }

            if (goneUp)
            {
                InputLockManager.RemoveControlLock(this.GetType().Name);

                //				Screen.showCursor = true;
                cursorLocker.UnlockCursor();

                if (movePart)
                {
                    EditorBounds.Instance.constructionBounds = movementBounds;
                    //					UpdateDragPlane ();
                }
                movePart = false;
            }

            if (EditorLogic.SelectedPart == null)
            {
                movePart = false;
            }

            // WASD in place mode is part rotation.
            var simpleMove = EditorLogic.SelectedPart == null || EditorLogic.fetch.EditorConstructionMode != ConstructionMode.Place;

            if (isDown || simpleMove)
            {
                var rot = Quaternion.AngleAxis(yaw, Vector3.up) * Quaternion.AngleAxis(pitch, Vector3.right);
                var fwd = rot * Vector3.forward;
                var side = rot * Vector3.right;

                Vector3 acc = Vector3.zero;
                if (Input.GetKey(config.keyForward))
                {
                    acc += fwd;
                }
                if (config.mouseWheelActive && Input.GetAxis("Mouse ScrollWheel") > 0 && shiftIsDown)
                {
                    acc += fwd * config.mouseWheelAcceleration;
                }
                if (Input.GetKey(config.keyBack))
                {
                    acc -= fwd;
                }
                if (config.mouseWheelActive && Input.GetAxis("Mouse ScrollWheel") < 0 && shiftIsDown)
                {
                    acc -= fwd * config.mouseWheelAcceleration;
                }
                if (Input.GetKey(config.keyRight))
                {
                    acc += side;
                }
                if (Input.GetKey(config.keyLeft))
                {
                    acc -= side;
                }
                if (Input.GetKey(config.keyDown))
                {
                    acc -= Vector3.up;
                }
                if (config.mouseWheelActive && Input.GetAxis("Mouse ScrollWheel") < 0 && !shiftIsDown)
                {
                    acc -= Vector3.up * config.mouseWheelAcceleration;
                }
                if (Input.GetKey(config.keyUp))
                {
                    acc += Vector3.up;
                }
                if (config.mouseWheelActive && Input.GetAxis("Mouse ScrollWheel") > 0 && !shiftIsDown)
                {
                    acc += Vector3.up * config.mouseWheelAcceleration;
                }
                if (Input.GetKey(config.keyRun))
                {
                    acc *= config.runMultiplier;
                }
                if (Input.GetKey(config.keySneak))
                {
                    acc *= config.sneakMultiplier;
                }
                acc *= config.acceleration;
                vel += (acc - config.friction * vel) * Time.deltaTime;
                var delta = vel * Time.deltaTime;
                EditorLogic.fetch.editorCamera.transform.rotation = rot;
                if (!movePart && EditorLogic.SelectedPart != null && EditorLogic.fetch.EditorConstructionMode == ConstructionMode.Place
                    && (delta != Vector3.zero || dx != 0 || dy != 0))
                {
                    movePart = true;
                    // A convenient trick to prevent EditorLogic::dragOverPlane from updating the part position.
                    EditorBounds.Instance.constructionBounds = ZERO_BOUNDS;
                }

                if (movePart || simpleMove)
                {
                    pos += delta;

                    float distance = 0;
                    movementBounds.IntersectRay(new Ray(pos, -delta), out distance);
                    //distance = -1;
                    //log.Debug("POS {0} {1} {2}", pos, yaw, pitch);
                    if (distance < 0)
                    {
                        EditorLogic.fetch.editorCamera.transform.position = pos;
                    }
                    else
                    {
                        EditorLogic.fetch.editorCamera.transform.position = GetClosestPointOnBounds(movementBounds, pos);
                        pos = EditorLogic.fetch.editorCamera.transform.position;
                    }

                    if (movePart)
                    {
                        //						Refl.Invoke (EditorLogic.fetch, "displayAttachNodeIcons", false, false, false);

                        var offset = EditorLogic.fetch.editorCamera.transform.TransformPoint(partOffset);
                        EditorLogic.SelectedPart.transform.position = offset - EditorLogic.fetch.selPartGrabOffset;
                    }
                }
            }
        }

        private Vector3 GetClosestPointOnBounds(Bounds bounds, Vector3 outsidePoint)
        {
            var closest = outsidePoint;
            for (var i = 0; i < 3; ++i)
            {
                if (closest[i] < bounds.min[i])
                {
                    closest[i] = bounds.min[i];
                }
                else if (closest[i] > bounds.max[i])
                {
                    closest[i] = bounds.max[i];
                }
            }
            return closest;
        }
    }

    public interface CursorLocker
    {
        void LockUpdate();

        void PrepareLock();

        void LockCursor();

        void UnlockCursor();
    }


    /**
	 * Screen.lockCursor resets cursor to center causing selected part to jump :(
	 */
    public class UnityLocker : CursorLocker
    {
        public void LockUpdate()
        {
        }

        public void PrepareLock()
        {
        }

        public void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            //			Screen.lockCursor = true;
        }

        public void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            //			Screen.lockCursor = false;
        }
    }

    /**
	 * ClipCursor isn't working. Maybe Unity is resetting it each frame?
	 * SetCursorPos is lame but kinda works.
	 */
    public class WinCursorLocker : CursorLocker
    {
        private static POINT pos;
        private static bool locked;

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);

        public void LockUpdate()
        {
            if (locked)
            {
                SetCursorPos(pos.X, pos.Y);
            }
        }

        public void PrepareLock()
        {
            GetCursorPos(out pos);
        }

        public void LockCursor()
        {
            locked = true;
        }

        public void UnlockCursor()
        {
            locked = false;
        }
    }

#if false
	public static class Refl
	{
		public static FieldInfo GetField (object obj, string name)
		{
			Log.Info ("obj: " + obj.ToString () + "    name: " + name);
			var f = obj.GetType ().GetField (name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
			if (f == null)
				throw new Exception ("No such field: " + obj.GetType () + "#" + name);
			return f;
		}

		public static object GetValue (object obj, string name)
		{
			Log.Info ("GetValue");
			return GetField (obj, name).GetValue (obj);
		}

		public static void SetValue (object obj, string name, object value)
		{
			GetField (obj, name).SetValue (obj, value);
		}

		public static MethodInfo GetMethod (object obj, string name)
		{
			var m = obj.GetType ().GetMethod (name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			if (m == null)
				throw new Exception ("No such method: " + obj.GetType () + "#" + name);
			return m;
		}

		public static object Invoke (object obj, string name, params object[] args)
		{
			return GetMethod (obj, name).Invoke (obj, args);
		}
	}
#endif

#if false
	/**
	 * What? How does that fix anything you ask? Magic! Well, here the best explaination I came up with:
	 * 
	 * UIManager::DidAnyPointerHitUI calls UIManager::Update if it hasn't been called this frame yet.
	 * UIManager::Update will update the UI and cause the new part to be spawned on click on the icon.
	 * VABCamera::Update and EditorLogic::Update run before UIManager::Update, so the first call from them of
	 * UIManager::DidAnyPointerHitUI will cause UIManager::Update to run.
	 * 
	 * Normally this happens in VABCamera::Update and everything is fine. When the VABCamera is disabled,
	 * this call shifts to EditorLogic::Update. Here an unfortunate sequence of events happens.
	 * 
	 * Click on part icon runs EditorLogic::pickPart, which in turn runs UIManager::DidAnyPointerHitUI
	 * and UIManager::Update. This causes the new part to be spawned, but pickPart will not find it yet
	 * and return null. It will reset EditorLogic::SelectedPart and the new part is lost.
	 * 
	 * So the fix is to run EditorLogic after UIManager so it can find the newly spawned part. This is 
	 * where the magic and undocumented behaviour of Unity comes in. Apparently MonoBehaviours are run
	 * in the order they were enabled. By toggling the enabled flag of EditorLogic it will be moved
	 * back in the queue and will run after UIManager.
	 * 
	 * That's what I think anyway. ¯\_(ツ)_/¯
	 * 
	 * http://bugs.kerbalspaceprogram.com/issues/3838
	 */
	public static class KspIssue3838Fix
	{
		public static void ApplyFix (bool editorExtensionsCompatibility)
		{
			// Have you tried turning off and on again?
			EditorLogic.fetch.enabled = false;
			EditorLogic.fetch.enabled = true;

			if (!editorExtensionsCompatibility)
				return;
			// EditorExtensions needs to run after EditorLogic or stuff breaks:
			// http://forum.kerbalspaceprogram.com/threads/38768?p=1946608#post1946608
			var obj = GameObject.Find ("EditorExtensions");
			if (obj) {
				var ee = (MonoBehaviour)obj.GetComponent ("EditorExtensions");
				if (ee) {
					ee.enabled = false;
					ee.enabled = true;
				}
			}
		}
	}
#endif
}
