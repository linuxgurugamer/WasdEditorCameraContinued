﻿using System;
using UnityEngine;
using System.IO;
using KSP.UI.Screens;
using ClickThroughFix;
using ToolbarControl_NS;
using System.Linq;

namespace WasdEditorCamera
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(MainMenuGui.MODID, MainMenuGui.MODNAME);
        }
    }

    public class MainMenuGui : MonoBehaviour
    {

        public const String TITLE = "WASD Editor Camera";
        public static String ROOT_PATH;
        private static String CONFIG_BASE_FOLDER;

        private static String WASD_BASE_FOLDER ;
        private static string TEXTURE_DIR;

        public static String WASD_NODENAME = "WASDEDITORCAMERA";
        public static String WASD_CFG_FILE;

        void Awake()
        {
            
            ROOT_PATH = KSPUtil.ApplicationRootPath;
            CONFIG_BASE_FOLDER = ROOT_PATH + "GameData/";

            WASD_BASE_FOLDER = CONFIG_BASE_FOLDER + "WasdEditorCamera/";
            TEXTURE_DIR = "WasdEditorCamera/" + "Textures/";
            
            WASD_CFG_FILE = WASD_BASE_FOLDER + "PluginData/WASD_Settings.cfg";

        }

        public /*static*/ bool infoDisplayActive = false;

        private const int WIDTH = 725;
        private const int HEIGHT = 550;
        private Rect bounds = new Rect(Screen.width / 2 - WIDTH / 2, Screen.height / 2 - HEIGHT / 2, WIDTH, HEIGHT);
        private /* volatile*/ bool visible = false;
        // Stock APP Toolbar - Stavell
        //public /*static*/ ApplicationLauncherButton WASD_Button = null;
        ToolbarControl toolbarControl = null;
        //public  bool stockToolBarcreated = false;


        private bool cfgWinData = false;
        private bool defaultsLoaded = false;
        //private static bool appLaucherHidden = true;

        private Config newconfig;

        string strkeyForward, strkeyBack, strkeyRight, strkeyLeft, strkeyUp, strkeyDown, strkeyRun, strkeySneak, strkeySwitchMode;


        private ComboBox cbkeyForward = new ComboBox();
        private ComboBox cbkeyBack = new ComboBox();
        private ComboBox cbkeyRight = new ComboBox();
        private ComboBox cbkeyLeft = new ComboBox();
        private ComboBox cbkeyUp = new ComboBox();
        private ComboBox cbkeyDown = new ComboBox();
        private ComboBox cbkeyRun = new ComboBox();
        private ComboBox cbkeySneak = new ComboBox();
        private ComboBox cbkeySwitchMode = new ComboBox();

        string strsensitivity;
        string stracceleration;
        string strmouseWheelAcceleration;
        string strfriction;
        string strrunMultiplier;
        string strsneakMultiplier;

        string strVabInitPosX, strVabInitPosY, strVabInitPosZ;
        string strVabInitPitch, strVabInitYaw;
        string strVabBoundsMinX, strVabBoundsMinY, strVabBoundsMinZ;
        string strVabBoundsMaxX, strVabBoundsMaxY, strVabBoundsMaxZ;

        string strSphInitPosX, strSphInitPosY, strSphInitPosZ;
        string strSphInitPitch, strSphInitYaw;
        string strSphBoundsMinX, strSphBoundsMinY, strSphBoundsMinZ;
        string strSphBoundsMaxX, strSphBoundsMaxY, strSphBoundsMaxZ;



        GUIContent[] comboBoxList;
        //private ComboBox comboBoxControl = new ComboBox ();
        private GUIStyle listStyle = new GUIStyle();

        private void Start()
        {
            //			DontDestroyOnLoad (this);

            var dontBindTheseKeys = Config
                .ExcludeKeysFromBinding
                .ToDictionary(k => k.ToString());

            // set key dropdown values to be all keycodes (except ExcludeKeysFromBinding).
            string[] keys = Enum
                .GetNames(typeof(KeyCode))
                .Where(x => !dontBindTheseKeys.ContainsKey(x))
                .ToArray();

            comboBoxList = new GUIContent[keys.Length];

            for (int i = 0; i < keys.Length; i++)
                comboBoxList[i] = new GUIContent(keys[i]);

            listStyle.normal.textColor = Color.white;


            //listStyle.onHover.background =
            //  listStyle.hover.background = new Texture2D (2, 2);
            listStyle.padding.left =
              listStyle.padding.right =
              listStyle.padding.top =
              listStyle.padding.bottom = 4;

            //			var texWhite = new Texture2D (2, 2);
            //			var colors = new Color[4];

            var texBlack = new Texture2D(2, 2);
            var colorsBlack = new Color[4];

            var texGrey = new Texture2D(2, 2);
            var colorsGrey = new Color[4];

            //			var texGreen = new Texture2D (2, 2);
            //			var colorsGreen = new Color[4];

            //			var texYellow = new Texture2D (2, 2);
            //			var colorsYellow = new Color[4];

            var texRed = new Texture2D(2, 2);
            var colorsRed = new Color[4];

            var texBlue = new Texture2D(2, 2);
            var colorsBlue = new Color[4];

            //			var texMagenta = new Texture2D (2, 2);
            //			var colorsMagenta = new Color[4];


            for (int i = 0; i < 4; i++)
            {
                //				colors [i] = Color.white;
                colorsBlack[i] = Color.black;
                colorsGrey[i] = Color.grey;
                //				colorsGreen [i] = Color.green;
                //				colorsYellow [i] = Color.yellow;
                colorsRed[i] = Color.red;
                colorsBlue[i] = Color.blue;
                //				colorsMagenta [i] = Color.magenta;

            }
            //			texWhite.SetPixels (colors);
            //			texWhite.Apply ();
            texBlack.SetPixels(colorsBlack);
            texBlack.Apply();

            texGrey.SetPixels(colorsGrey);
            texGrey.Apply();
            //			texGreen.SetPixels (colorsGreen);
            //			texGreen.Apply ();
            //			texYellow.SetPixels (colorsYellow);
            //			texYellow.Apply ();
            texRed.SetPixels(colorsRed);
            texRed.Apply();
            texBlue.SetPixels(colorsBlue);
            texBlue.Apply();
            //			texMagenta.SetPixels (colorsMagenta);
            //			texMagenta.Apply ();

            listStyle.normal.background = texBlack;
            //			listStyle.onHover.background = texWhite;
            listStyle.hover.background = texGrey;
            listStyle.active.background = texBlue;
            //			listStyle.focused.background = texMagenta;
            //			listStyle.onActive.background = texGreen;
            //			listStyle.onNormal.background = texGrey;
            //			listStyle.onFocused.background = texYellow;			

        }


        internal const string MODID = "WASD_NS";
        internal const string MODNAME = "WASD Editor Camera";

        public void UpdateToolbarStock()
        {
            if (toolbarControl == null)
            {
                toolbarControl = gameObject.AddComponent<ToolbarControl>();
                toolbarControl.AddToAllToolbars(null, null,
                    ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.SPH,
                    MODID,
                    "wasdButton",
                    TEXTURE_DIR + "WASD-on-38",
                    TEXTURE_DIR + "WASD-38",
                    TEXTURE_DIR + "WASD-on-24",
                    TEXTURE_DIR + "WASD-24",
                    MODNAME
                );
                toolbarControl.AddLeftRightClickCallbacks(GUIToggle, ToggleActive);

            }
        }

        static bool on = false;
        public void set_WASD_Button_active(bool i)
        {
            if (!active)
                toolbarControl.SetTexture(TEXTURE_DIR + "WASD-disabled-38",
                        TEXTURE_DIR + "WASD-disabled-24");
            else
            {
                on = i;
                if (!i)
                    toolbarControl.SetTexture(TEXTURE_DIR + "WASD-38", TEXTURE_DIR + "WASD-24");
                else
                    toolbarControl.SetTexture(TEXTURE_DIR + "WASD-on-38", TEXTURE_DIR + "WASD-on-24");
            }
        }

        public void GUIToggle()
        {
            infoDisplayActive = !infoDisplayActive;

            if (infoDisplayActive)
            {
                SetVisible(true);
            }
            else
            {
                SetVisible(false);
                cfgWinData = false;
                defaultsLoaded = false;

                UpdateToolbarStock();
            }
        }
        public static bool active = true;
        public void ToggleActive()
        {
            active = !active;
            set_WASD_Button_active(on);
        }

        private void HideToolbarStock()
        {
            //ApplicationLauncher.Instance.RemoveModApplication (MainMenuGui.WASD_Button);
            //Destroy (WASD_Button); // Is this necessary?
            //WASD_Button = null;
            //appLaucherHidden = false;
        }

        public void OnDestroy()
        {
            toolbarControl.OnDestroy();
            Destroy(toolbarControl);
        }

        public bool Visible()
        {
            return visible;
        }

        public void SetVisible(bool newVisible)
        {
            visible = newVisible;
        }

        public void OnGUI()
        {
            try
            {
                if (this.Visible())
                {
                    this.bounds = ClickThruBlocker.GUILayoutWindow(this.GetInstanceID(), this.bounds, this.Window, TITLE, HighLogic.Skin.window);
                }
            }
            catch (Exception e)
            {
                Log.Error("exception: " + e.Message);
            }
        }


        bool SetDefaults()
        {
            newconfig.initConfig();
            cfgWinData = false;
            defaultsLoaded = true;
            return true;
        }

        private void Window(int id)
        {
            if (cfgWinData == false)
            {
                cfgWinData = true;

                if (!defaultsLoaded)
                    newconfig = WasdEditorCameraBehaviour.config;
                defaultsLoaded = false;

                strkeyForward = newconfig.keyForward.ToString();
                strkeyBack = newconfig.keyBack.ToString();
                strkeyRight = newconfig.keyRight.ToString();
                strkeyLeft = newconfig.keyLeft.ToString();
                strkeyUp = newconfig.keyUp.ToString();
                strkeyDown = newconfig.keyDown.ToString();
                strkeyRun = newconfig.keyRun.ToString();
                strkeySneak = newconfig.keySneak.ToString();
                strkeySwitchMode = newconfig.keySwitchMode.ToString();

                cbkeyForward.SetSelectedItemIndex(strkeyForward, comboBoxList);
                cbkeyBack.SetSelectedItemIndex(strkeyBack, comboBoxList);
                cbkeyRight.SetSelectedItemIndex(strkeyRight, comboBoxList);
                cbkeyLeft.SetSelectedItemIndex(strkeyLeft, comboBoxList);
                cbkeyUp.SetSelectedItemIndex(strkeyUp, comboBoxList);
                cbkeyDown.SetSelectedItemIndex(strkeyDown, comboBoxList);
                cbkeyRun.SetSelectedItemIndex(strkeyRun, comboBoxList);
                cbkeySneak.SetSelectedItemIndex(strkeySneak, comboBoxList);
                cbkeySwitchMode.SetSelectedItemIndex(strkeySwitchMode, comboBoxList);

                strsensitivity = newconfig.sensitivity.ToString();
                stracceleration = newconfig.acceleration.ToString();
                strmouseWheelAcceleration = newconfig.mouseWheelAcceleration.ToString();
                strfriction = newconfig.friction.ToString();
                strrunMultiplier = newconfig.runMultiplier.ToString();
                strsneakMultiplier = newconfig.sneakMultiplier.ToString();



                strVabInitPosX = newconfig.vab.initialPosition.x.ToString();
                strVabInitPosY = newconfig.vab.initialPosition.y.ToString();
                strVabInitPosZ = newconfig.vab.initialPosition.z.ToString();
                strVabInitPitch = newconfig.vab.initialPitch.ToString();
                strVabInitYaw = newconfig.vab.initialYaw.ToString();
                strVabBoundsMinX = newconfig.vab.bounds.min.x.ToString();
                strVabBoundsMinY = newconfig.vab.bounds.min.y.ToString();
                strVabBoundsMinZ = newconfig.vab.bounds.min.z.ToString();
                strVabBoundsMaxX = newconfig.vab.bounds.max.x.ToString();
                strVabBoundsMaxY = newconfig.vab.bounds.max.y.ToString();
                strVabBoundsMaxZ = newconfig.vab.bounds.max.z.ToString();


                strSphInitPosX = newconfig.sph.initialPosition.x.ToString();
                strSphInitPosY = newconfig.sph.initialPosition.y.ToString();
                strSphInitPosZ = newconfig.sph.initialPosition.z.ToString();
                strSphInitPitch = newconfig.sph.initialPitch.ToString();
                strSphInitYaw = newconfig.sph.initialYaw.ToString();
                strSphBoundsMinX = newconfig.sph.bounds.min.x.ToString();
                strSphBoundsMinY = newconfig.sph.bounds.min.y.ToString();
                strSphBoundsMinZ = newconfig.sph.bounds.min.z.ToString();
                strSphBoundsMaxX = newconfig.sph.bounds.max.x.ToString();
                strSphBoundsMaxY = newconfig.sph.bounds.max.y.ToString();
                strSphBoundsMaxZ = newconfig.sph.bounds.max.z.ToString();

            }
            SetVisible(true);
            GUI.enabled = true;

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.EndHorizontal();

            GUILayout.BeginArea(new Rect(10, 50, 375, 500));

            GUILayout.BeginVertical();
            DrawTitle("Keys");

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Key forward: ");
#if true
            //			GUILayout.BeginArea (new Rect (10, 20, 300, 500));
            //			strkeyForward = comboBoxList[cbkeyForward.List (new Rect (125, 5, 150, 20), strkeyForward, comboBoxList, listStyle)].text;
            //			GUILayout.EndArea ();
#endif
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Key back: ");
#if true
            //			GUILayout.BeginArea (new Rect (10, 45, 300, 500));
            //			strkeyBack = comboBoxList[cbkeyBack.List (new Rect (125, 5, 150, 20), strkeyBack, comboBoxList, listStyle)].text;
            //			GUILayout.EndArea ();
#endif
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Key right: ");
#if true
            //			GUILayout.BeginArea (new Rect (10, 70, 300, 500));
            //			strkeyRight = comboBoxList[cbkeyRight.List (new Rect (125, 5, 150, 20), strkeyRight, comboBoxList, listStyle)].text;
            //			GUILayout.EndArea ();
#endif
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Key left: ");
#if true
            //			GUILayout.BeginArea (new Rect (10, 95, 300, 500));
            //			strkeyLeft = comboBoxList[cbkeyLeft.List (new Rect (125, 5, 150, 20), strkeyLeft, comboBoxList, listStyle)].text;
            //			GUILayout.EndArea ();
#endif
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Key up: ");
#if true
            //			GUILayout.BeginArea (new Rect (10, 120, 300, 500));
            //			strkeyUp = comboBoxList[cbkeyUp.List (new Rect (125, 5, 150, 20), strkeyUp, comboBoxList, listStyle)].text;
            //			GUILayout.EndArea ();
#endif
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Key down: ");
#if true
            //			GUILayout.BeginArea (new Rect (10, 145, 300, 500));
            //			strkeyDown = comboBoxList[cbkeyDown.List (new Rect (125, 5, 150, 20), strkeyDown, comboBoxList, listStyle)].text;
            //			GUILayout.EndArea ();
#endif
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Key run: ");
#if true
            //			GUILayout.BeginArea (new Rect (10, 170, 300, 500));
            //			strkeyRun = comboBoxList[cbkeyRun.List (new Rect (125, 5, 150, 20), strkeyRun, comboBoxList, listStyle)].text;
            //			GUILayout.EndArea ();
#endif
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Key sneak: ");
#if true
            //			GUILayout.BeginArea (new Rect (10, 195, 300, 500));
            //			strkeySneak = comboBoxList[cbkeySneak.List (new Rect (125, 5, 150, 20), strkeySneak, comboBoxList, listStyle)].text;
            //			GUILayout.EndArea ();
#endif
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Key switch mode: ");
#if true
            //			GUILayout.BeginArea (new Rect (10, 220, 300, 500));
            //			strkeySwitchMode = comboBoxList[cbkeySwitchMode.List (new Rect (125, 5, 150, 20), strkeySwitchMode, comboBoxList, listStyle)].text;
            //			GUILayout.EndArea ();
#endif
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("");
            GUILayout.EndHorizontal();


            DrawTitle("VAB", true);

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Initial Position: ");
            GUILayout.FlexibleSpace();
            strVabInitPosX = GUILayout.TextField(strVabInitPosX, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            strVabInitPosY = GUILayout.TextField(strVabInitPosY, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            strVabInitPosZ = GUILayout.TextField(strVabInitPosZ, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Initial Pitch: ");
            GUILayout.FlexibleSpace();
            strVabInitPitch = GUILayout.TextField(strVabInitPitch, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Initial Yaw: ");
            GUILayout.FlexibleSpace();
            strVabInitYaw = GUILayout.TextField(strVabInitYaw, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Bounds min: ");
            GUILayout.FlexibleSpace();
            // x, y, z
            strVabBoundsMinX = GUILayout.TextField(strVabBoundsMinX, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            strVabBoundsMinY = GUILayout.TextField(strVabBoundsMinY, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            strVabBoundsMinZ = GUILayout.TextField(strVabBoundsMinZ, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Bounds max: ");
            GUILayout.FlexibleSpace();
            // x, y, z
            strVabBoundsMaxX = GUILayout.TextField(strVabBoundsMaxX, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            strVabBoundsMaxY = GUILayout.TextField(strVabBoundsMaxY, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            strVabBoundsMaxZ = GUILayout.TextField(strVabBoundsMaxZ, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(40));
            GUILayout.Label("");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            if (GUILayout.Button("Defaults", GUILayout.Width(125.0f)))
            {
                bool b = SetDefaults();
                return;
            }
            GUILayout.EndHorizontal();



            ///////////////////////////////////////////////
            /// These have to be here so that when they are clicked on, they will overwrite the other controls in the column
            /// 

#if true
            strkeyForward = comboBoxList[cbkeyForward.List(new Rect(135, 25, 150, 20), strkeyForward, comboBoxList, listStyle)].text;
            strkeyBack = comboBoxList[cbkeyBack.List(new Rect(135, 50, 150, 20), strkeyBack, comboBoxList, listStyle)].text;
            strkeyRight = comboBoxList[cbkeyRight.List(new Rect(135, 75, 150, 20), strkeyRight, comboBoxList, listStyle)].text;
            strkeyLeft = comboBoxList[cbkeyLeft.List(new Rect(135, 100, 150, 20), strkeyLeft, comboBoxList, listStyle)].text;
            strkeyUp = comboBoxList[cbkeyUp.List(new Rect(135, 125, 150, 20), strkeyUp, comboBoxList, listStyle)].text;
            strkeyDown = comboBoxList[cbkeyDown.List(new Rect(135, 150, 150, 20), strkeyDown, comboBoxList, listStyle)].text;
            strkeyRun = comboBoxList[cbkeyRun.List(new Rect(135, 175, 150, 20), strkeyRun, comboBoxList, listStyle)].text;
            strkeySneak = comboBoxList[cbkeySneak.List(new Rect(135, 200, 150, 20), strkeySneak, comboBoxList, listStyle)].text;
            strkeySwitchMode = comboBoxList[cbkeySwitchMode.List(new Rect(135, 225, 150, 20), strkeySwitchMode, comboBoxList, listStyle)].text;
#endif
            GUILayout.EndVertical();
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(400, 50, 300, 500));
            GUILayout.BeginVertical();
            DrawTitle("Misc");

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Exp Editor Ext. Compatibility: ");
            GUILayout.FlexibleSpace();
            newconfig.enableExperimentalEditorExtensionsCompatibility =
                GUILayout.Toggle(newconfig.enableExperimentalEditorExtensionsCompatibility, "");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Default Camera: ");
            GUILayout.FlexibleSpace();
            newconfig.defaultCamera =
                GUILayout.Toggle(newconfig.defaultCamera, "");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Mouse wheel active: ");
            GUILayout.FlexibleSpace();
            newconfig.mouseWheelActive =
                GUILayout.Toggle(newconfig.mouseWheelActive, "");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Enforce bounds (ignored if HangerExtender installed): ");
            GUILayout.FlexibleSpace();
            newconfig.enforceBounds =
                GUILayout.Toggle(newconfig.enforceBounds, "");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Sensitivity: ");
            GUILayout.FlexibleSpace();
            strsensitivity = GUILayout.TextField(strsensitivity, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Acceleration: ");
            GUILayout.FlexibleSpace();
            stracceleration = GUILayout.TextField(stracceleration, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Mouse wheel acceleration: ");
            GUILayout.FlexibleSpace();
            strmouseWheelAcceleration = GUILayout.TextField(strmouseWheelAcceleration, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Friction: ");
            GUILayout.FlexibleSpace();
            strfriction = GUILayout.TextField(strfriction, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Run multiplier: ");
            GUILayout.FlexibleSpace();
            strrunMultiplier = GUILayout.TextField(strrunMultiplier, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Sneak multiplier: ");
            GUILayout.FlexibleSpace();
            strsneakMultiplier = GUILayout.TextField(strsneakMultiplier, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            GUILayout.EndHorizontal();

            //GUILayout.BeginHorizontal (GUILayout.Height(20));
            //GUILayout.Label ("");
            //GUILayout.EndHorizontal ();

            DrawTitle("SPH", true);

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Initial Position: ");
            GUILayout.FlexibleSpace();
            strSphInitPosX = GUILayout.TextField(strSphInitPosX, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            strSphInitPosY = GUILayout.TextField(strSphInitPosY, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            strSphInitPosZ = GUILayout.TextField(strSphInitPosZ, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Initial Pitch: ");
            GUILayout.FlexibleSpace();
            strSphInitPitch = GUILayout.TextField(strSphInitPitch, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Initial Yaw: ");
            GUILayout.FlexibleSpace();
            strSphInitYaw = GUILayout.TextField(strSphInitYaw, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Bounds min: ");
            GUILayout.FlexibleSpace();
            // x, y, z
            strSphBoundsMinX = GUILayout.TextField(strSphBoundsMinX, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            strSphBoundsMinY = GUILayout.TextField(strSphBoundsMinY, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            strSphBoundsMinZ = GUILayout.TextField(strSphBoundsMinZ, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("Bounds max: ");
            GUILayout.FlexibleSpace();
            // x, y, z
            strSphBoundsMaxX = GUILayout.TextField(strSphBoundsMaxX, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            strSphBoundsMaxY = GUILayout.TextField(strSphBoundsMaxY, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            strSphBoundsMaxZ = GUILayout.TextField(strSphBoundsMaxZ, GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label("");
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal(GUILayout.Height(20));

            if (GUILayout.Button("Save", GUILayout.Width(125.0f)))
            {
                writeConfig(newconfig);
                //WasdEditorCameraBehaviour.config.setConfig (newconfig);
                GUIToggle();
            }

            if (GUILayout.Button("Cancel", GUILayout.Width(125.0f)))
            {
                GUIToggle();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndArea();

            try
            {
                newconfig.keyForward = (KeyCode)Enum.Parse(typeof(KeyCode), strkeyForward);
            }
            catch (Exception)
            {
            }
            try
            {
                newconfig.keyBack = (KeyCode)Enum.Parse(typeof(KeyCode), strkeyBack);
            }
            catch (Exception)
            {
            }
            try
            {
                newconfig.keyRight = (KeyCode)Enum.Parse(typeof(KeyCode), strkeyRight);
            }
            catch (Exception)
            {
            }
            try
            {
                newconfig.keyLeft = (KeyCode)Enum.Parse(typeof(KeyCode), strkeyLeft);
            }
            catch (Exception)
            {
            }
            try
            {
                newconfig.keyUp = (KeyCode)Enum.Parse(typeof(KeyCode), strkeyUp);
            }
            catch (Exception)
            {
            }
            try
            {
                newconfig.keyDown = (KeyCode)Enum.Parse(typeof(KeyCode), strkeyDown);
            }
            catch (Exception)
            {
            }
            try
            {
                newconfig.keyRun = (KeyCode)Enum.Parse(typeof(KeyCode), strkeyRun);
            }
            catch (Exception)
            {
            }
            try
            {
                newconfig.keySneak = (KeyCode)Enum.Parse(typeof(KeyCode), strkeySneak);
            }
            catch (Exception)
            {
            }
            try
            {
                newconfig.keySwitchMode = (KeyCode)Enum.Parse(typeof(KeyCode), strkeySwitchMode);
            }
            catch (Exception)
            {
            }

            try
            {
                newconfig.sensitivity = Convert.ToSingle(strsensitivity);
            }
            catch (Exception)
            {
            }

            try
            {
                newconfig.acceleration = Convert.ToSingle(stracceleration);
            }
            catch (Exception)
            {
            }
            try
            {
                newconfig.mouseWheelAcceleration = Convert.ToSingle(strmouseWheelAcceleration);
            }
            catch (Exception)
            {
            }


            try
            {
                newconfig.friction = Convert.ToSingle(strfriction);
            }
            catch (Exception)
            {
            }

            try
            {
                newconfig.runMultiplier = Convert.ToSingle(strrunMultiplier);
            }
            catch (Exception)
            {
            }

            try
            {
                newconfig.sneakMultiplier = Convert.ToSingle(strsneakMultiplier);
            }
            catch (Exception)
            {
            }


            try
            {
                newconfig.vab.initialPosition.x = Convert.ToSingle(strVabInitPosX);
            }
            catch (Exception)
            {
            }
            try
            {
                newconfig.vab.initialPosition.y = Convert.ToSingle(strVabInitPosY);
            }
            catch (Exception)
            {
            }
            try
            {
                newconfig.vab.initialPosition.z = Convert.ToSingle(strVabInitPosZ);
            }
            catch (Exception)
            {
            }

            try
            {
                newconfig.vab.initialPitch = Convert.ToSingle(strVabInitPitch);
            }
            catch (Exception)
            {
            }
            try
            {
                newconfig.vab.initialYaw = Convert.ToSingle(strVabInitYaw);
            }
            catch (Exception)
            {
            }

            float x, y, z;
            try
            {
                x = Convert.ToSingle(strVabBoundsMinX);
                y = Convert.ToSingle(strVabBoundsMinY);
                z = Convert.ToSingle(strVabBoundsMinZ);
                newconfig.vab.bounds.min = new Vector3(x, y, z);
            }
            catch (Exception)
            {
            }
            try
            {
                x = Convert.ToSingle(strVabBoundsMaxX);
                y = Convert.ToSingle(strVabBoundsMaxY);
                z = Convert.ToSingle(strVabBoundsMaxZ);
                newconfig.vab.bounds.max = new Vector3(x, y, z);
            }
            catch (Exception)
            {
            }

            try
            {
                newconfig.sph.initialPosition.x = Convert.ToSingle(strSphInitPosX);
            }
            catch (Exception)
            {
            }
            try
            {
                newconfig.sph.initialPosition.y = Convert.ToSingle(strSphInitPosY);
            }
            catch (Exception)
            {
            }
            try
            {
                newconfig.sph.initialPosition.z = Convert.ToSingle(strSphInitPosZ);
            }
            catch (Exception)
            {
            }

            try
            {
                newconfig.sph.initialPitch = Convert.ToSingle(strSphInitPitch);
            }
            catch (Exception)
            {
            }
            try
            {
                newconfig.sph.initialYaw = Convert.ToSingle(strSphInitYaw);
            }
            catch (Exception)
            {
            }

            try
            {
                x = Convert.ToSingle(strSphBoundsMinX);
                y = Convert.ToSingle(strSphBoundsMinY);
                z = Convert.ToSingle(strSphBoundsMinZ);
                newconfig.sph.bounds.min = new Vector3(x, y, z);
            }
            catch (Exception)
            {
            }
            try
            {
                x = Convert.ToSingle(strSphBoundsMaxX);
                y = Convert.ToSingle(strSphBoundsMaxY);
                z = Convert.ToSingle(strSphBoundsMaxZ);
                newconfig.sph.bounds.max = new Vector3(x, y, z);
            }
            catch (Exception)
            {
            }

            GUI.DragWindow();
        }

        private void DrawTitle(String text, bool xyz = false)
        {
            GUILayout.BeginHorizontal(GUILayout.Height(20));
            GUILayout.Label(text, HighLogic.Skin.label);
            GUILayout.FlexibleSpace();
            if (xyz)
            {
                GUILayout.Label("   X", GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
                GUILayout.Label("   Y", GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
                GUILayout.Label("   Z", GUILayout.MinWidth(60.0F), GUILayout.MaxWidth(60.0F));
            }
            GUILayout.EndHorizontal();
        }


        private void writeConfig(Config config)
        {
            ConfigNode root = new ConfigNode();
            ConfigNode top = new ConfigNode(WASD_NODENAME);
            root.SetNode(WASD_NODENAME, top, true);
            top.SetValue("defaultCamera", config.defaultCamera, true);
            top.SetValue("mouseWheelActive", config.mouseWheelActive, true);
            top.SetValue("enableExperimentalEditorExtensionsCompatibility", config.enableExperimentalEditorExtensionsCompatibility, true);
            top.SetValue("enforceBounds", config.enforceBounds, true);
            top.SetValue("sensitivity", config.sensitivity.ToString(), true);
            top.SetValue("acceleration", config.acceleration.ToString(), true);
            top.SetValue("mouseWheelAcceleration", config.mouseWheelAcceleration.ToString(), true);
            top.SetValue("friction", config.friction.ToString(), true);
            top.SetValue("runMultiplier", config.runMultiplier.ToString(), true);
            top.SetValue("sneakMultiplier", config.sneakMultiplier.ToString(), true);

            ConfigNode keysNode = new ConfigNode("KEYS");
            top.SetNode("KEYS", keysNode, true);

            keysNode.SetValue("forward", config.keyForward.ToString(), true);
            keysNode.SetValue("back", config.keyBack.ToString(), true);
            keysNode.SetValue("right", config.keyRight.ToString(), true);
            keysNode.SetValue("left", config.keyLeft.ToString(), true);
            keysNode.SetValue("up", config.keyUp.ToString(), true);
            keysNode.SetValue("down", config.keyDown.ToString(), true);
            keysNode.SetValue("switchMode", config.keySwitchMode.ToString(), true);
            keysNode.SetValue("run", config.keyRun.ToString(), true);
            keysNode.SetValue("sneak", config.keySneak.ToString(), true);


            ConfigNode editorsNode = new ConfigNode("EDITORS");
            top.SetNode("EDITORS", editorsNode, true);
            ConfigNode vabNode = new ConfigNode("VAB");
            editorsNode.SetNode("VAB", vabNode, true);
            string s;

            s = config.vab.initialPosition.ToString();
            s = s.TrimStart('(');
            s = s.TrimEnd(')');
            vabNode.SetValue("initialPosition", s, true);
            vabNode.SetValue("initialPitch", config.vab.initialPitch.ToString(), true);
            vabNode.SetValue("initialYaw", config.vab.initialYaw.ToString(), true);
            ConfigNode vabBoundsNode = new ConfigNode("BOUNDS");
            vabNode.SetNode("BOUNDS", vabBoundsNode, true);
            s = config.vab.bounds.min.ToString();
            s = s.TrimStart('(');
            s = s.TrimEnd(')');
            vabBoundsNode.SetValue("min", s, true);
            s = config.vab.bounds.max.ToString();
            s = s.TrimStart('(');
            s = s.TrimEnd(')');
            vabBoundsNode.SetValue("max", s, true);

            // need to do bounds


            ConfigNode sphNode = new ConfigNode("SPH");
            editorsNode.SetNode("SPH", sphNode, true);
            s = config.sph.initialPosition.ToString();
            s = s.TrimStart('(');
            s = s.TrimEnd(')');
            sphNode.SetValue("initialPosition", s, true);
            sphNode.SetValue("initialPitch", config.sph.initialPitch.ToString(), true);
            sphNode.SetValue("initialYaw", config.sph.initialYaw.ToString(), true);
            // need to do bounds
            ConfigNode sphBoundsNode = new ConfigNode("BOUNDS");
            sphNode.SetNode("BOUNDS", sphBoundsNode, true);
            s = config.sph.bounds.min.ToString();
            s = s.TrimStart('(');
            s = s.TrimEnd(')');

            sphBoundsNode.SetValue("min", s, true);
            s = config.sph.bounds.max.ToString();
            s = s.TrimStart('(');
            s = s.TrimEnd(')');
            sphBoundsNode.SetValue("max", s, true);


            root.Save(WASD_CFG_FILE);

            WasdEditorCameraBehaviour.setConfig(root);

        }


    }


}