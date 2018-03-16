using System;
using UnityEngine;
using System.IO;




namespace WasdEditorCamera
{
	

	public class MainMenuGui : MonoBehaviour
	{

		public const String TITLE = "WASD Editor Camera";
		public const string TEXTURE_DIR = "WASDEditorCamera/Textures/";
		public static readonly String ROOT_PATH = KSPUtil.ApplicationRootPath;
		private static readonly String CONFIG_BASE_FOLDER = ROOT_PATH + "GameData/";
		private static String WASD_BASE_FOLDER = CONFIG_BASE_FOLDER + "WasdEditorCamera/";
		private static String WASD_NODENAME = "WASDEDITORCAMERA";
		private static String WASD_CFG_FILE = WASD_BASE_FOLDER + "WASD_Settings.cfg";



		public static bool infoDisplayActive = false;

		private const int WIDTH = 725;
		private const int HEIGHT = 550;
		private Rect bounds = new Rect (Screen.width / 2 - WIDTH / 2, Screen.height / 2 - HEIGHT / 2, WIDTH, HEIGHT);
		private /* volatile*/ bool visible = false;
		// Stock APP Toolbar - Stavell
		public static ApplicationLauncherButton WASD_Button = null;
		public  bool stockToolBarcreated = false;

		private static Texture2D WASD_button_off = new Texture2D (38, 38, TextureFormat.ARGB32, false);
		private static Texture2D WASD_button_on = new Texture2D (38, 38, TextureFormat.ARGB32, false);
		private static bool WASD_Texture_Load = false;

		private bool cfgWinData = false;
		private bool defaultsLoaded = false;
		private static bool appLaucherHidden = true;

		private WasdEditorCameraBehaviour.Config newconfig;

		string strkeyForward, strkeyBack, strkeyRight, strkeyLeft, strkeyUp, strkeyDown, strkeyRun, strkeySneak, strkeySwitchMode;

		#if true
		private ComboBox cbkeyForward = new ComboBox ();
		private ComboBox cbkeyBack = new ComboBox ();
		private ComboBox cbkeyRight = new ComboBox ();
		private ComboBox cbkeyLeft = new ComboBox ();
		private ComboBox cbkeyUp = new ComboBox ();
		private ComboBox cbkeyDown = new ComboBox ();
		private ComboBox cbkeyRun = new ComboBox ();
		private ComboBox cbkeySneak = new ComboBox ();
		private ComboBox cbkeySwitchMode = new ComboBox ();
		#endif
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
		private GUIStyle listStyle = new GUIStyle ();

		private void Start ()
		{
			comboBoxList = new GUIContent[87];

			comboBoxList [0] = new GUIContent ("Space");
			comboBoxList [1] = new GUIContent ("Keypad0");
			comboBoxList [2] = new GUIContent ("Keypad1");
			comboBoxList [3] = new GUIContent ("Keypad2");
			comboBoxList [4] = new GUIContent ("Keypad3");
			comboBoxList [5] = new GUIContent ("Keypad4");
			comboBoxList [6] = new GUIContent ("Keypad5");
			comboBoxList [7] = new GUIContent ("Keypad6");
			comboBoxList [8] = new GUIContent ("Keypad7");
			comboBoxList [9] = new GUIContent ("Keypad8");
			comboBoxList [10] = new GUIContent ("Keypad9");
			comboBoxList [11] = new GUIContent ("KeypadPeriod");
			comboBoxList [12] = new GUIContent ("KeypadDivide");
			comboBoxList [13] = new GUIContent ("KeypadMultiply");
			comboBoxList [14] = new GUIContent ("KeypadMinus");
			comboBoxList [15] = new GUIContent ("KeypadPlus");
			comboBoxList [16] = new GUIContent ("KeypadEnter");
			comboBoxList [17] = new GUIContent ("KeypadEquals");
			comboBoxList [18] = new GUIContent ("UpArrow");
			comboBoxList [19] = new GUIContent ("DownArrow");
			comboBoxList [20] = new GUIContent ("RightArrow");
			comboBoxList [21] = new GUIContent ("LeftArrow");
			comboBoxList [22] = new GUIContent ("Insert");
			comboBoxList [23] = new GUIContent ("Home");
			comboBoxList [24] = new GUIContent ("End");
			comboBoxList [25] = new GUIContent ("PageUp");
			comboBoxList [26] = new GUIContent ("PageDown");
			comboBoxList [27] = new GUIContent ("F1");
			comboBoxList [28] = new GUIContent ("F2");
			comboBoxList [29] = new GUIContent ("F3");
			comboBoxList [30] = new GUIContent ("F4");
			comboBoxList [31] = new GUIContent ("F5");
			comboBoxList [32] = new GUIContent ("F6");
			comboBoxList [33] = new GUIContent ("F7");
			comboBoxList [34] = new GUIContent ("F8");
			comboBoxList [35] = new GUIContent ("F9");
			comboBoxList [36] = new GUIContent ("F10");
			comboBoxList [37] = new GUIContent ("F11");
			comboBoxList [38] = new GUIContent ("F12");
			comboBoxList [39] = new GUIContent ("F13");
			comboBoxList [40] = new GUIContent ("F14");
			comboBoxList [41] = new GUIContent ("F15");
			comboBoxList [42] = new GUIContent ("Alpha0");
			comboBoxList [43] = new GUIContent ("Alpha1");
			comboBoxList [44] = new GUIContent ("Alpha2");
			comboBoxList [45] = new GUIContent ("Alpha3");
			comboBoxList [46] = new GUIContent ("Alpha4");
			comboBoxList [47] = new GUIContent ("Alpha5");
			comboBoxList [48] = new GUIContent ("Alpha6");
			comboBoxList [49] = new GUIContent ("Alpha7");
			comboBoxList [50] = new GUIContent ("Alpha8");
			comboBoxList [51] = new GUIContent ("Alpha9");
			comboBoxList [52] = new GUIContent ("A");
			comboBoxList [53] = new GUIContent ("B");
			comboBoxList [54] = new GUIContent ("C");
			comboBoxList [55] = new GUIContent ("D");
			comboBoxList [56] = new GUIContent ("E");
			comboBoxList [57] = new GUIContent ("F");
			comboBoxList [58] = new GUIContent ("G");
			comboBoxList [59] = new GUIContent ("H");
			comboBoxList [60] = new GUIContent ("I");
			comboBoxList [61] = new GUIContent ("J");
			comboBoxList [62] = new GUIContent ("K");
			comboBoxList [63] = new GUIContent ("L");
			comboBoxList [64] = new GUIContent ("M");
			comboBoxList [65] = new GUIContent ("N");
			comboBoxList [66] = new GUIContent ("O");
			comboBoxList [67] = new GUIContent ("P");
			comboBoxList [68] = new GUIContent ("Q");
			comboBoxList [69] = new GUIContent ("R");
			comboBoxList [70] = new GUIContent ("S");
			comboBoxList [71] = new GUIContent ("T");
			comboBoxList [72] = new GUIContent ("U");
			comboBoxList [73] = new GUIContent ("V");
			comboBoxList [74] = new GUIContent ("W");
			comboBoxList [75] = new GUIContent ("X");
			comboBoxList [76] = new GUIContent ("Y");
			comboBoxList [77] = new GUIContent ("Z");
			comboBoxList [78] = new GUIContent ("Numlock");
			comboBoxList [79] = new GUIContent ("CapsLock");
			comboBoxList [80] = new GUIContent ("ScrollLock");
			comboBoxList [81] = new GUIContent ("RightShift");
			comboBoxList [82] = new GUIContent ("LeftShift");
			comboBoxList [83] = new GUIContent ("RightControl");
			comboBoxList [84] = new GUIContent ("LeftControl");
			comboBoxList [85] = new GUIContent ("RightAlt");
			comboBoxList [86] = new GUIContent ("LeftAlt");

			listStyle.normal.textColor = Color.white;


			//listStyle.onHover.background =
			//  listStyle.hover.background = new Texture2D (2, 2);
			listStyle.padding.left =
		      listStyle.padding.right =
		      listStyle.padding.top =
		      listStyle.padding.bottom = 4;
			
			var texWhite = new Texture2D (2, 2);
			var texBlack = new Texture2D (2, 2);
			var colors = new Color[4];
			var colorsBlack = new Color[4];
			for (int i = 0; i < 4; i++) {
				colors [i] = Color.white;
				colorsBlack [i] = Color.black;
			}
			texWhite.SetPixels (colors);
			texWhite.Apply ();
			texBlack.SetPixels (colorsBlack);
			texBlack.Apply ();
			#if true
			listStyle.normal.background = texBlack;
			listStyle.onHover.background = texWhite;
			listStyle.hover.background = texWhite;
			listStyle.active.background = texBlack;
			listStyle.focused.background = texBlack;
			listStyle.onActive.background = texBlack;
			listStyle.onNormal.background = texBlack;
			listStyle.onFocused.background = texBlack;			
			#endif
		}

		public void setAppLauncherHidden ()
		{
			appLaucherHidden = true;
		}

		public void OnGUIShowApplicationLauncher ()
		{
			if (appLaucherHidden) {
				appLaucherHidden = false;
				if (WASD_Button != null)
					UpdateToolbarStock ();
			}

		}

		public void OnGUIApplicationLauncherReady ()
		{
			UpdateToolbarStock ();
		}


		public void UpdateToolbarStock ()
		{

			// Create the button in the KSP AppLauncher
			if (!WASD_Texture_Load) {
				if (GameDatabase.Instance.ExistsTexture (TEXTURE_DIR + "WASD-38"))
					WASD_button_off = GameDatabase.Instance.GetTexture (TEXTURE_DIR + "WASD-38", false);
				if (GameDatabase.Instance.ExistsTexture (TEXTURE_DIR + "WASD-on-38"))
					WASD_button_on = GameDatabase.Instance.GetTexture (TEXTURE_DIR + "WASD-on-38", false);
				

				WASD_Texture_Load = true;
			}
			if (WASD_Button == null) {

				WASD_Button = ApplicationLauncher.Instance.AddModApplication (GUIToggle, GUIToggle,
					null, null,
					null, null,
					ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.SPH,
					WASD_button_off);
				stockToolBarcreated = true;
			}
		}

		public void set_WASD_Button_active (bool i)
		{
			if (!i)
				WASD_Button.SetTexture (WASD_button_off);
			else
				WASD_Button.SetTexture (WASD_button_on);
		}

		public void GUIToggle ()
		{
			stockToolBarcreated = true;

			infoDisplayActive = !infoDisplayActive;
			if (infoDisplayActive) {
				SetVisible (true);
				//WASD_Button.SetTexture (WASD_button_on); 
			} else {
				SetVisible (false);
				//set_WASD_Button_active (false);
				cfgWinData = false;
				defaultsLoaded = false;

//				GUI_SaveData ();

//				WASD.configuration.Save ();

				//WASD_Button.SetTexture (MainMenuGui.WASD_button_config);
				UpdateToolbarStock ();
				//set_WASD_Button_active (false);
			}
		}

		private void HideToolbarStock ()
		{
			ApplicationLauncher.Instance.RemoveModApplication (MainMenuGui.WASD_Button);
			Destroy (WASD_Button); // Is this necessary?
			WASD_Button = null;
			appLaucherHidden = false;
		}

		public bool Visible ()
		{ 
			return this.visible;
		}

		public void SetVisible (bool visible)
		{
			this.visible = visible;
		}

		public void OnGUI ()
		{
			try {
				if (this.Visible ()) {
					this.bounds = GUILayout.Window (this.GetInstanceID (), this.bounds, this.Window, TITLE, HighLogic.Skin.window);
				}
			} catch (Exception) {
				//Log.Error ("exception: " + e.Message);
			}
		}


		private Bounds ParseBounds (ConfigNode node)
		{
			var min = ConfigNode.ParseVector3 (node.GetValue ("min"));
			var max = ConfigNode.ParseVector3 (node.GetValue ("max"));
			var result = new Bounds ();
			result.SetMinMax (min, max);
			return result;
		}

		private WasdEditorCameraBehaviour.Config.EditorConfig ParseEditorConfig (ConfigNode node)
		{
			WasdEditorCameraBehaviour.Config.EditorConfig result = new WasdEditorCameraBehaviour.Config.EditorConfig ();

			try {
				result.initialPosition = ConfigNode.ParseVector3 (node.GetValue ("initialPosition"));
			} catch {
			}
			try {
				result.initialPitch = Single.Parse (node.GetValue ("initialPitch"));
			} catch {
			}
			try {
				result.initialYaw = Single.Parse (node.GetValue ("initialYaw"));
			} catch {
			}
			try {
				result.bounds = ParseBounds (node.GetNode ("BOUNDS"));
			} catch {
			}


			return result;
		}

		public   WasdEditorCameraBehaviour.Config parseConfigNode (ConfigNode root)
		{
			WasdEditorCameraBehaviour.Config config = new WasdEditorCameraBehaviour.Config ();

			try {
				config.enableExperimentalEditorExtensionsCompatibility = Boolean.Parse (root.GetValue ("enableExperimentalEditorExtensionsCompatibility"));
			} catch {
			}
			try {
				config.defaultCamera = Boolean.Parse (root.GetValue ("defaultCamera"));
			} catch {
			}
			try {
				config.mouseWheelActive = Boolean.Parse (root.GetValue ("mouseWheelActive"));
			} catch {
			}
			try {
				config.enforceBounds = Boolean.Parse (root.GetValue ("enforceBounds"));
			} catch {
			}

			try {
				config.sensitivity = Single.Parse (root.GetValue ("sensitivity"));
			} catch {
			}
			try {
				config.acceleration = Single.Parse (root.GetValue ("acceleration"));
			} catch {
			}
			try {
				config.mouseWheelAcceleration = Single.Parse (root.GetValue ("mouseWheelAcceleration"));
			} catch {
			}
			try {
				config.friction = Single.Parse (root.GetValue ("friction"));
			} catch {
			}
			try {
				config.runMultiplier = Single.Parse (root.GetValue ("runMultiplier"));
			} catch {
			}
			try {
				config.sneakMultiplier = Single.Parse (root.GetValue ("sneakMultiplier"));
			} catch {
			}

			var keys = root.GetNode ("KEYS");
			try {
				config.keyForward = (KeyCode)ConfigNode.ParseEnum (typeof(KeyCode), keys.GetValue ("forward"));
			} catch {
			}
			try {
				config.keyBack = (KeyCode)ConfigNode.ParseEnum (typeof(KeyCode), keys.GetValue ("back"));
			} catch {
			}
			try {
				config.keyRight = (KeyCode)ConfigNode.ParseEnum (typeof(KeyCode), keys.GetValue ("right"));
			} catch {
			}
			try {
				config.keyLeft = (KeyCode)ConfigNode.ParseEnum (typeof(KeyCode), keys.GetValue ("left"));
			} catch {
			}
			try {
				config.keyUp = (KeyCode)ConfigNode.ParseEnum (typeof(KeyCode), keys.GetValue ("up"));
			} catch {
			}
			try {
				config.keyDown = (KeyCode)ConfigNode.ParseEnum (typeof(KeyCode), keys.GetValue ("down"));
			} catch {
			}
			try {
				config.keyRun = (KeyCode)ConfigNode.ParseEnum (typeof(KeyCode), keys.GetValue ("run"));
			} catch {
			}
			try {
				config.keySneak = (KeyCode)ConfigNode.ParseEnum (typeof(KeyCode), keys.GetValue ("sneak"));
			} catch {
			}
			try {
				config.keySwitchMode = (KeyCode)ConfigNode.ParseEnum (typeof(KeyCode), keys.GetValue ("switchMode"));
			} catch {
			}

			var editors = root.GetNode ("EDITORS");
			try {
				config.vab = ParseEditorConfig (editors.GetNode ("VAB"));
			} catch {
			}
			try {
				config.sph = ParseEditorConfig (editors.GetNode ("SPH"));
			} catch {
			}

			//log.Debug ("Config loaded.");
			return config;
		}

		void SetDefaults ()
		{
			string fname = WASD_CFG_FILE + ".default";
			if (System.IO.File.Exists (fname)) {
				ConfigNode file = ConfigNode.Load (fname);
				var root = file.GetNode (WASD_NODENAME);
				newconfig = parseConfigNode (root);
				cfgWinData = false;
				defaultsLoaded = true;
			}
			//newconfig = configFile;
		}

		private void Window (int id)
		{
			if (cfgWinData == false) {
				cfgWinData = true;
	
				if (!defaultsLoaded)
					newconfig = WasdEditorCameraBehaviour.config;
				defaultsLoaded = false;

				strkeyForward = newconfig.keyForward.ToString ();
				strkeyBack = newconfig.keyBack.ToString ();
				strkeyRight = newconfig.keyRight.ToString ();
				strkeyLeft = newconfig.keyLeft.ToString ();
				strkeyUp = newconfig.keyUp.ToString ();
				strkeyDown = newconfig.keyDown.ToString ();
				strkeyRun = newconfig.keyRun.ToString ();
				strkeySneak = newconfig.keySneak.ToString ();
				strkeySwitchMode = newconfig.keySwitchMode.ToString ();
				#if true
				cbkeyForward.SetSelectedItemIndex (strkeyForward, comboBoxList);
				cbkeyBack.SetSelectedItemIndex (strkeyBack, comboBoxList);
				cbkeyRight.SetSelectedItemIndex (strkeyRight, comboBoxList);
				cbkeyLeft.SetSelectedItemIndex (strkeyLeft, comboBoxList);
				cbkeyUp.SetSelectedItemIndex (strkeyUp, comboBoxList);
				cbkeyDown.SetSelectedItemIndex (strkeyDown, comboBoxList);
				cbkeyRun.SetSelectedItemIndex (strkeyRun, comboBoxList);
				cbkeySneak.SetSelectedItemIndex (strkeySneak, comboBoxList);
				cbkeySwitchMode.SetSelectedItemIndex (strkeySwitchMode, comboBoxList);
				#endif
				strsensitivity = newconfig.sensitivity.ToString ();
				stracceleration = newconfig.acceleration.ToString ();
				strmouseWheelAcceleration = newconfig.mouseWheelAcceleration.ToString ();
				strfriction = newconfig.friction.ToString ();
				strrunMultiplier = newconfig.runMultiplier.ToString ();
				strsneakMultiplier = newconfig.sneakMultiplier.ToString ();



				strVabInitPosX = newconfig.vab.initialPosition.x.ToString ();
				strVabInitPosY = newconfig.vab.initialPosition.y.ToString ();
				strVabInitPosZ = newconfig.vab.initialPosition.z.ToString ();
				strVabInitPitch = newconfig.vab.initialPitch.ToString ();
				strVabInitYaw = newconfig.vab.initialYaw.ToString ();
				strVabBoundsMinX = newconfig.vab.bounds.min.x.ToString ();
				strVabBoundsMinY = newconfig.vab.bounds.min.y.ToString ();
				strVabBoundsMinZ = newconfig.vab.bounds.min.z.ToString ();
				strVabBoundsMaxX = newconfig.vab.bounds.max.x.ToString ();
				strVabBoundsMaxY = newconfig.vab.bounds.max.y.ToString ();
				strVabBoundsMaxZ = newconfig.vab.bounds.max.z.ToString ();


				strSphInitPosX = newconfig.sph.initialPosition.x.ToString ();
				strSphInitPosY = newconfig.sph.initialPosition.y.ToString ();
				strSphInitPosZ = newconfig.sph.initialPosition.z.ToString ();
				strSphInitPitch = newconfig.sph.initialPitch.ToString ();
				strSphInitYaw = newconfig.sph.initialYaw.ToString ();
				strSphBoundsMinX = newconfig.sph.bounds.min.x.ToString ();
				strSphBoundsMinY = newconfig.sph.bounds.min.y.ToString ();
				strSphBoundsMinZ = newconfig.sph.bounds.min.z.ToString ();
				strSphBoundsMaxX = newconfig.sph.bounds.max.x.ToString ();
				strSphBoundsMaxY = newconfig.sph.bounds.max.y.ToString ();
				strSphBoundsMaxZ = newconfig.sph.bounds.max.z.ToString ();

			}
			SetVisible (true);
			GUI.enabled = true;

			GUILayout.BeginHorizontal ();
			GUILayout.EndHorizontal ();
		
			GUILayout.BeginArea (new Rect (10, 50, 375, 500));

			GUILayout.BeginVertical ();
			DrawTitle ("Keys");

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Key forward: ");
			#if true
//			GUILayout.BeginArea (new Rect (10, 20, 300, 500));
//			strkeyForward = comboBoxList[cbkeyForward.List (new Rect (125, 5, 150, 20), strkeyForward, comboBoxList, listStyle)].text;
//			GUILayout.EndArea ();
			#else
			GUILayout.FlexibleSpace (); 

			strkeyForward = GUILayout.TextField (strkeyForward, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			#endif
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Key back: ");
			#if true
//			GUILayout.BeginArea (new Rect (10, 45, 300, 500));
//			strkeyBack = comboBoxList[cbkeyBack.List (new Rect (125, 5, 150, 20), strkeyBack, comboBoxList, listStyle)].text;
//			GUILayout.EndArea ();
			#else
			GUILayout.FlexibleSpace ();

			strkeyBack = GUILayout.TextField (strkeyBack, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			#endif
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Key right: ");
			#if true
//			GUILayout.BeginArea (new Rect (10, 70, 300, 500));
//			strkeyRight = comboBoxList[cbkeyRight.List (new Rect (125, 5, 150, 20), strkeyRight, comboBoxList, listStyle)].text;
//			GUILayout.EndArea ();
			#else
			GUILayout.FlexibleSpace ();

			strkeyRight = GUILayout.TextField (strkeyRight, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			#endif
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Key left: ");
			#if true
//			GUILayout.BeginArea (new Rect (10, 95, 300, 500));
//			strkeyLeft = comboBoxList[cbkeyLeft.List (new Rect (125, 5, 150, 20), strkeyLeft, comboBoxList, listStyle)].text;
//			GUILayout.EndArea ();
			#else
			GUILayout.FlexibleSpace ();

			strkeyLeft = GUILayout.TextField (strkeyLeft, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			#endif
			GUILayout.EndHorizontal ();


			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Key up: ");
			#if true
//			GUILayout.BeginArea (new Rect (10, 120, 300, 500));
//			strkeyUp = comboBoxList[cbkeyUp.List (new Rect (125, 5, 150, 20), strkeyUp, comboBoxList, listStyle)].text;
//			GUILayout.EndArea ();
			#else
			GUILayout.FlexibleSpace ();

			strkeyUp = GUILayout.TextField (strkeyUp, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			#endif
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Key down: ");
			#if true
//			GUILayout.BeginArea (new Rect (10, 145, 300, 500));
//			strkeyDown = comboBoxList[cbkeyDown.List (new Rect (125, 5, 150, 20), strkeyDown, comboBoxList, listStyle)].text;
//			GUILayout.EndArea ();
			#else
			GUILayout.FlexibleSpace ();

			strkeyDown = GUILayout.TextField (strkeyDown, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			#endif
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Key run: ");
			#if true
//			GUILayout.BeginArea (new Rect (10, 170, 300, 500));
//			strkeyRun = comboBoxList[cbkeyRun.List (new Rect (125, 5, 150, 20), strkeyRun, comboBoxList, listStyle)].text;
//			GUILayout.EndArea ();
			#else
			GUILayout.FlexibleSpace ();

			strkeyRun = GUILayout.TextField (strkeyRun, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			#endif
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Key sneak: ");
			#if true
//			GUILayout.BeginArea (new Rect (10, 195, 300, 500));
//			strkeySneak = comboBoxList[cbkeySneak.List (new Rect (125, 5, 150, 20), strkeySneak, comboBoxList, listStyle)].text;
//			GUILayout.EndArea ();
			#else
			GUILayout.FlexibleSpace ();

			strkeySneak = GUILayout.TextField (strkeySneak, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			#endif
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Key switch mode: ");
			#if true
//			GUILayout.BeginArea (new Rect (10, 220, 300, 500));
//			strkeySwitchMode = comboBoxList[cbkeySwitchMode.List (new Rect (125, 5, 150, 20), strkeySwitchMode, comboBoxList, listStyle)].text;
//			GUILayout.EndArea ();
			#else
			GUILayout.FlexibleSpace ();

			strkeySwitchMode = GUILayout.TextField (strkeySwitchMode, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			#endif
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("");
			GUILayout.EndHorizontal ();


			DrawTitle ("VAB", true);

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Initial Position: ");
			GUILayout.FlexibleSpace ();
			strVabInitPosX = GUILayout.TextField (strVabInitPosX, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			strVabInitPosY = GUILayout.TextField (strVabInitPosY, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			strVabInitPosZ = GUILayout.TextField (strVabInitPosZ, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Initial Pitch: ");
			GUILayout.FlexibleSpace ();
			strVabInitPitch = GUILayout.TextField (strVabInitPitch, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Initial Yaw: ");
			GUILayout.FlexibleSpace ();
			strVabInitYaw = GUILayout.TextField (strVabInitYaw, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Bounds min: ");
			GUILayout.FlexibleSpace ();
			// x, y, z
			strVabBoundsMinX = GUILayout.TextField (strVabBoundsMinX, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			strVabBoundsMinY = GUILayout.TextField (strVabBoundsMinY, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			strVabBoundsMinZ = GUILayout.TextField (strVabBoundsMinZ, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Bounds max: ");
			GUILayout.FlexibleSpace ();
			// x, y, z
			strVabBoundsMaxX = GUILayout.TextField (strVabBoundsMaxX, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			strVabBoundsMaxY = GUILayout.TextField (strVabBoundsMaxY, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			strVabBoundsMaxZ = GUILayout.TextField (strVabBoundsMaxZ, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("");
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Defaults", GUILayout.Width (125.0f))) {
				SetDefaults ();
			}
			GUILayout.EndHorizontal ();



			///////////////////////////////////////////////
			/// These have to be here so that when they are clicked on, they will overwrite the other controls in the column
			/// 


			GUILayout.BeginArea (new Rect (10, 20, 300, 500));
			strkeyForward = comboBoxList [cbkeyForward.List (new Rect (125, 5, 150, 20), strkeyForward, comboBoxList, listStyle)].text;
			GUILayout.EndArea ();

			GUILayout.BeginArea (new Rect (10, 45, 300, 500));
			strkeyBack = comboBoxList [cbkeyBack.List (new Rect (125, 5, 150, 20), strkeyBack, comboBoxList, listStyle)].text;
			GUILayout.EndArea ();

			GUILayout.BeginArea (new Rect (10, 70, 300, 500));
			strkeyRight = comboBoxList [cbkeyRight.List (new Rect (125, 5, 150, 20), strkeyRight, comboBoxList, listStyle)].text;
			GUILayout.EndArea ();

			GUILayout.BeginArea (new Rect (10, 95, 300, 500));
			strkeyLeft = comboBoxList [cbkeyLeft.List (new Rect (125, 5, 150, 20), strkeyLeft, comboBoxList, listStyle)].text;
			GUILayout.EndArea ();

			GUILayout.BeginArea (new Rect (10, 120, 300, 500));
			strkeyUp = comboBoxList [cbkeyUp.List (new Rect (125, 5, 150, 20), strkeyUp, comboBoxList, listStyle)].text;
			GUILayout.EndArea ();

			GUILayout.BeginArea (new Rect (10, 145, 300, 500));
			strkeyDown = comboBoxList [cbkeyDown.List (new Rect (125, 5, 150, 20), strkeyDown, comboBoxList, listStyle)].text;
			GUILayout.EndArea ();

			GUILayout.BeginArea (new Rect (10, 170, 300, 500));
			strkeyRun = comboBoxList [cbkeyRun.List (new Rect (125, 5, 150, 20), strkeyRun, comboBoxList, listStyle)].text;
			GUILayout.EndArea ();

			GUILayout.BeginArea (new Rect (10, 195, 300, 500));
			strkeySneak = comboBoxList [cbkeySneak.List (new Rect (125, 5, 150, 20), strkeySneak, comboBoxList, listStyle)].text;
			GUILayout.EndArea ();

			GUILayout.BeginArea (new Rect (10, 220, 300, 500));
			strkeySwitchMode = comboBoxList [cbkeySwitchMode.List (new Rect (125, 5, 150, 20), strkeySwitchMode, comboBoxList, listStyle)].text;
			GUILayout.EndArea ();

			GUILayout.EndVertical ();
			GUILayout.EndArea ();
		

			GUILayout.BeginArea (new Rect (400, 50, 300, 500));
			GUILayout.BeginVertical ();
			DrawTitle ("Misc");

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Exp Editor Ext. Compatibility: ");
			GUILayout.FlexibleSpace ();
			newconfig.enableExperimentalEditorExtensionsCompatibility = 
				GUILayout.Toggle (newconfig.enableExperimentalEditorExtensionsCompatibility, "");
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Default Camera: ");
			GUILayout.FlexibleSpace ();
			newconfig.defaultCamera = 
				GUILayout.Toggle (newconfig.defaultCamera, "");
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Mouse wheel active: ");
			GUILayout.FlexibleSpace ();
			newconfig.mouseWheelActive = 
				GUILayout.Toggle (newconfig.mouseWheelActive, "");
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Enforce bounds: ");
			GUILayout.FlexibleSpace ();
			newconfig.enforceBounds = 
				GUILayout.Toggle (newconfig.enforceBounds, "");
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Sensitivity: ");
			GUILayout.FlexibleSpace ();
			strsensitivity = GUILayout.TextField (strsensitivity, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Acceleration: ");
			GUILayout.FlexibleSpace ();
			stracceleration = GUILayout.TextField (stracceleration, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Mouse wheel acceleration: ");
			GUILayout.FlexibleSpace ();
			strmouseWheelAcceleration = GUILayout.TextField (strmouseWheelAcceleration, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			GUILayout.EndHorizontal ();


			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Friction: ");
			GUILayout.FlexibleSpace ();
			strfriction = GUILayout.TextField (strfriction, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Run multiplier: ");
			GUILayout.FlexibleSpace ();
			strrunMultiplier = GUILayout.TextField (strrunMultiplier, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Sneak multiplier: ");
			GUILayout.FlexibleSpace ();
			strsneakMultiplier = GUILayout.TextField (strsneakMultiplier, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			GUILayout.EndHorizontal ();

			//GUILayout.BeginHorizontal ();
			//GUILayout.Label ("");
			//GUILayout.EndHorizontal ();

			DrawTitle ("SPH", true);

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Initial Position: ");
			GUILayout.FlexibleSpace ();
			strSphInitPosX = GUILayout.TextField (strSphInitPosX, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			strSphInitPosY = GUILayout.TextField (strSphInitPosY, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			strSphInitPosZ = GUILayout.TextField (strSphInitPosZ, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Initial Pitch: ");
			GUILayout.FlexibleSpace ();
			strSphInitPitch = GUILayout.TextField (strSphInitPitch, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Initial Yaw: ");
			GUILayout.FlexibleSpace ();
			strSphInitYaw = GUILayout.TextField (strSphInitYaw, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Bounds min: ");
			GUILayout.FlexibleSpace ();
			// x, y, z
			strSphBoundsMinX = GUILayout.TextField (strSphBoundsMinX, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			strSphBoundsMinY = GUILayout.TextField (strSphBoundsMinY, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			strSphBoundsMinZ = GUILayout.TextField (strSphBoundsMinZ, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Bounds max: ");
			GUILayout.FlexibleSpace ();
			// x, y, z
			strSphBoundsMaxX = GUILayout.TextField (strSphBoundsMaxX, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			strSphBoundsMaxY = GUILayout.TextField (strSphBoundsMaxY, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			strSphBoundsMaxZ = GUILayout.TextField (strSphBoundsMaxZ, GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("");
			GUILayout.EndHorizontal ();


			GUILayout.BeginHorizontal ();

			if (GUILayout.Button ("Save", GUILayout.Width (125.0f))) {
				writeConfig (newconfig);
				GUIToggle ();
			}

			if (GUILayout.Button ("Cancel", GUILayout.Width (125.0f))) {
				GUIToggle ();
			}
			GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();
			GUILayout.EndArea ();

			try {
				newconfig.keyForward = (KeyCode)Enum.Parse (typeof(KeyCode), strkeyForward);
			} catch (Exception) {
			}
			try {
				newconfig.keyBack = (KeyCode)Enum.Parse (typeof(KeyCode), strkeyBack);
			} catch (Exception) {
			}
			try {
				newconfig.keyRight = (KeyCode)Enum.Parse (typeof(KeyCode), strkeyRight);
			} catch (Exception) {
			}
			try {
				newconfig.keyLeft = (KeyCode)Enum.Parse (typeof(KeyCode), strkeyLeft);
			} catch (Exception) {
			}
			try {
				newconfig.keyUp = (KeyCode)Enum.Parse (typeof(KeyCode), strkeyUp);
			} catch (Exception) {
			}
			try {
				newconfig.keyDown = (KeyCode)Enum.Parse (typeof(KeyCode), strkeyDown);
			} catch (Exception) {
			}
			try {
				newconfig.keyRun = (KeyCode)Enum.Parse (typeof(KeyCode), strkeyRun);
			} catch (Exception) {
			}
			try {
				newconfig.keySneak = (KeyCode)Enum.Parse (typeof(KeyCode), strkeySneak);
			} catch (Exception) {
			}
			try {
				newconfig.keySwitchMode = (KeyCode)Enum.Parse (typeof(KeyCode), strkeySwitchMode);
			} catch (Exception) {
			}

			try {
				newconfig.sensitivity = Convert.ToSingle (strsensitivity);
			} catch (Exception) {
			}

			try {
				newconfig.acceleration = Convert.ToSingle (stracceleration);
			} catch (Exception) {
			}
			try {
				newconfig.mouseWheelAcceleration = Convert.ToSingle (strmouseWheelAcceleration);
			} catch (Exception) {
			}


			try {
				newconfig.friction = Convert.ToSingle (strfriction);
			} catch (Exception) {
			} 

			try {
				newconfig.runMultiplier = Convert.ToSingle (strrunMultiplier);
			} catch (Exception) {
			}

			try {
				newconfig.sneakMultiplier = Convert.ToSingle (strsneakMultiplier);
			} catch (Exception) {
			}

			 
			try {
				newconfig.vab.initialPosition.x = Convert.ToSingle (strVabInitPosX);
			} catch (Exception) {
			}
			try {
				newconfig.vab.initialPosition.y = Convert.ToSingle (strVabInitPosY);
			} catch (Exception) {
			}
			try {
				newconfig.vab.initialPosition.z = Convert.ToSingle (strVabInitPosZ);
			} catch (Exception) {
			}

			try {
				newconfig.vab.initialPitch = Convert.ToSingle (strVabInitPitch);
			} catch (Exception) {
			}
			try {
				newconfig.vab.initialYaw = Convert.ToSingle (strVabInitYaw);
			} catch (Exception) {
			}

			float x, y, z;
			try {
				x = Convert.ToSingle (strVabBoundsMinX);
				y = Convert.ToSingle (strVabBoundsMinY);
				z = Convert.ToSingle (strVabBoundsMinZ);
				newconfig.vab.bounds.min = new Vector3 (x, y, z);
			} catch (Exception) {
			}
			try {
				x = Convert.ToSingle (strVabBoundsMaxX);
				y = Convert.ToSingle (strVabBoundsMaxY);
				z = Convert.ToSingle (strVabBoundsMaxZ);
				newconfig.vab.bounds.max = new Vector3 (x, y, z);
			} catch (Exception) {
			}
				
			try {
				newconfig.sph.initialPosition.x = Convert.ToSingle (strSphInitPosX);
			} catch (Exception) {
			}
			try {
				newconfig.sph.initialPosition.y = Convert.ToSingle (strSphInitPosY);
			} catch (Exception) {
			}
			try {
				newconfig.sph.initialPosition.z = Convert.ToSingle (strSphInitPosZ);
			} catch (Exception) {
			}

			try {
				newconfig.sph.initialPitch = Convert.ToSingle (strSphInitPitch);
			} catch (Exception) {
			}
			try {
				newconfig.sph.initialYaw = Convert.ToSingle (strSphInitYaw);
			} catch (Exception) {
			}

			try {
				x = Convert.ToSingle (strSphBoundsMinX);
				y = Convert.ToSingle (strSphBoundsMinY);
				z = Convert.ToSingle (strSphBoundsMinZ);
				newconfig.sph.bounds.min = new Vector3 (x, y, z);
			} catch (Exception) {
			}
			try {
				x = Convert.ToSingle (strSphBoundsMaxX);
				y = Convert.ToSingle (strSphBoundsMaxY);
				z = Convert.ToSingle (strSphBoundsMaxZ);
				newconfig.sph.bounds.max = new Vector3 (x, y, z);
			} catch (Exception) {
			}

			GUI.DragWindow ();
		}

		private void DrawTitle (String text, bool xyz = false)
		{
			GUILayout.BeginHorizontal ();
			GUILayout.Label (text, HighLogic.Skin.label);
			GUILayout.FlexibleSpace ();
			if (xyz) {
				GUILayout.Label ("   X", GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
				GUILayout.Label ("   Y", GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
				GUILayout.Label ("   Z", GUILayout.MinWidth (60.0F), GUILayout.MaxWidth (60.0F));
			}
			GUILayout.EndHorizontal ();
		}


		private void writeConfig (WasdEditorCameraBehaviour.Config config)
		{
			ConfigNode root = new ConfigNode ();
			ConfigNode top = new ConfigNode (WASD_NODENAME);
			root.SetNode (WASD_NODENAME, top, true);
			top.SetValue ("defaultCamera", config.defaultCamera.ToString (), true);
			top.SetValue ("mouseWheelActive", config.mouseWheelActive.ToString (), true);
			top.SetValue ("enableExperimentalEditorExtensionsCompatibility", config.enableExperimentalEditorExtensionsCompatibility.ToString (), true);
			top.SetValue ("enforceBounds", config.enforceBounds.ToString (), true);
			top.SetValue ("sensitivity", config.sensitivity.ToString (), true);
			top.SetValue ("acceleration", config.acceleration.ToString (), true);
			top.SetValue ("mouseWheelAcceleration", config.mouseWheelAcceleration.ToString (), true);
			top.SetValue ("friction", config.friction.ToString (), true);
			top.SetValue ("runMultiplier", config.runMultiplier.ToString (), true);
			top.SetValue ("sneakMultiplier", config.sneakMultiplier.ToString (), true);

			ConfigNode keysNode = new ConfigNode ("KEYS");
			top.SetNode ("KEYS", keysNode, true);

			keysNode.SetValue ("forward", config.keyForward.ToString (), true);
			keysNode.SetValue ("back", config.keyBack.ToString (), true);
			keysNode.SetValue ("right", config.keyRight.ToString (), true);
			keysNode.SetValue ("left", config.keyLeft.ToString (), true);
			keysNode.SetValue ("up", config.keyUp.ToString (), true);
			keysNode.SetValue ("down", config.keyDown.ToString (), true);
			keysNode.SetValue ("switchMode", config.keySwitchMode.ToString (), true);
			keysNode.SetValue ("run", config.keyRun.ToString (), true);
			keysNode.SetValue ("sneak", config.keySneak.ToString (), true);


			ConfigNode editorsNode = new ConfigNode ("EDITORS");
			top.SetNode ("EDITORS", editorsNode, true);
			ConfigNode vabNode = new ConfigNode ("VAB");
			editorsNode.SetNode ("VAB", vabNode, true);
			string s;

			s = config.vab.initialPosition.ToString ();
			s = s.TrimStart ('(');
			s = s.TrimEnd (')');
			vabNode.SetValue ("initialPosition", s, true);
			vabNode.SetValue ("initialPitch", config.vab.initialPitch.ToString (), true);
			vabNode.SetValue ("initialYaw", config.vab.initialYaw.ToString (), true);
			ConfigNode vabBoundsNode = new ConfigNode ("BOUNDS");
			vabNode.SetNode ("BOUNDS", vabBoundsNode, true);
			s = config.vab.bounds.min.ToString ();
			s = s.TrimStart ('(');
			s = s.TrimEnd (')');
			vabBoundsNode.SetValue ("min", s, true);
			s = config.vab.bounds.max.ToString ();
			s = s.TrimStart ('(');
			s = s.TrimEnd (')');
			vabBoundsNode.SetValue ("max", s, true);

			// need to do bounds


			ConfigNode sphNode = new ConfigNode ("SPH");
			editorsNode.SetNode ("SPH", sphNode, true);
			s = config.sph.initialPosition.ToString ();
			s = s.TrimStart ('(');
			s = s.TrimEnd (')');
			sphNode.SetValue ("initialPosition", s, true);
			sphNode.SetValue ("initialPitch", config.sph.initialPitch.ToString (), true);
			sphNode.SetValue ("initialYaw", config.sph.initialYaw.ToString (), true);
			// need to do bounds
			ConfigNode sphBoundsNode = new ConfigNode ("BOUNDS");
			sphNode.SetNode ("BOUNDS", sphBoundsNode, true);
			s = config.sph.bounds.min.ToString ();
			s = s.TrimStart ('(');
			s = s.TrimEnd (')');

			sphBoundsNode.SetValue ("min", s, true);
			s = config.sph.bounds.max.ToString ();
			s = s.TrimStart ('(');
			s = s.TrimEnd (')');
			sphBoundsNode.SetValue ("max", s, true);


			root.Save (WASD_CFG_FILE);

			WasdEditorCameraBehaviour.setConfig (root);
		}


	}


}