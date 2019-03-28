using System;
using UnityEngine;
using System.IO;

namespace WasdEditorCamera
{
	public class Config
	{
		public KeyCode keyForward;
		public KeyCode keyBack;
		public KeyCode keyRight;
		public KeyCode keyLeft;
		public KeyCode keyUp;
		public KeyCode keyDown;
		public KeyCode keyRun;
		public KeyCode keySneak;
		public KeyCode keySwitchMode;
        
		public bool enableExperimentalEditorExtensionsCompatibility;
		public bool defaultCamera;
		public bool enforceBounds;
		public bool mouseWheelActive;

		public float sensitivity;
		public float acceleration;
		public float friction;
		public float runMultiplier;
		public float sneakMultiplier;
		public float mouseWheelAcceleration;

		public EditorConfig vab;
		public EditorConfig sph;

		public class EditorConfig
		{
			public Bounds bounds;
			public Vector3 initialPosition;
			public float initialPitch;
			public float initialYaw;
		}

        public void initConfig()
        {
            keyForward = KeyCode.W;
            keyBack = KeyCode.S;
            keyRight = KeyCode.D;
            keyLeft = KeyCode.A;
            keyUp = KeyCode.E;
            keyDown = KeyCode.Q;
            keyRun = KeyCode.Space;
            keySneak = KeyCode.LeftControl;
            keySwitchMode = KeyCode.Alpha5;

            enableExperimentalEditorExtensionsCompatibility = true;
            defaultCamera = true;
            enforceBounds = true;
            mouseWheelActive = true;

            sensitivity = 14;
            acceleration = 150;
            friction = 10;
            runMultiplier = 2;
            sneakMultiplier = 0.3f;
            mouseWheelAcceleration = 1.75f;

            //vab = cfg.vab;
            vab = new EditorConfig();
            sph = new EditorConfig();

            vab.initialPosition = new Vector3(-6.1f,17.7f, -1.9f);
            vab.initialPitch = 16.386f;
            vab.initialYaw = 72.536f;
            Vector3 min = new Vector3(-28.8f, 0.8f, -22.5f);
            Vector3 max = new Vector3(29.7f, 70f, 22.7f);
            var result = new Bounds();
            result.SetMinMax(min, max);
            vab.bounds = result;

            // sph = cfg.sph;
            sph.initialPosition = new Vector3(-8.6f, 14.4f,  2.7f);
            sph.initialPitch = 14.080f;
            sph.initialYaw = 120.824f;

            min = new Vector3(-43.2f, 1.4f, -56f);
            max = new Vector3(43.0f, 30.3f, 56.0f);
            result = new Bounds();
            result.SetMinMax(min, max);
            sph.bounds = result;
        }
        
        public void parseConfigNode (ConfigNode root)
		{
			//Config config = new Config ();
			Log.Info("parseConfigNode");
			try {
				enableExperimentalEditorExtensionsCompatibility = Boolean.Parse (root.GetValue ("enableExperimentalEditorExtensionsCompatibility"));
			} catch {
			}

            try {
				defaultCamera = Boolean.Parse (root.GetValue ("defaultCamera"));
			} catch {
			}
			try {
				mouseWheelActive = Boolean.Parse (root.GetValue ("mouseWheelActive"));
			} catch {
			}
			try {
				enforceBounds = Boolean.Parse (root.GetValue ("enforceBounds"));
				Log.Info("enforceBounds: " + enforceBounds.ToString());
			} catch {
			}

			try {
				sensitivity = Single.Parse (root.GetValue ("sensitivity"));
			} catch {
			}
			try {
				acceleration = Single.Parse (root.GetValue ("acceleration"));
			} catch {
			}
			try {
				mouseWheelAcceleration = Single.Parse (root.GetValue ("mouseWheelAcceleration"));
			} catch {
			}
			try {
				friction = Single.Parse (root.GetValue ("friction"));
			} catch {
			}
			try {
				runMultiplier = Single.Parse (root.GetValue ("runMultiplier"));
			} catch {
			}
			try {
				sneakMultiplier = Single.Parse (root.GetValue ("sneakMultiplier"));
			} catch {
			}

			var keys = root.GetNode ("KEYS");
			try {
				keyForward = (KeyCode)ConfigNode.ParseEnum (typeof(KeyCode), keys.GetValue ("forward"));
			} catch {
			}
			try {
				keyBack = (KeyCode)ConfigNode.ParseEnum (typeof(KeyCode), keys.GetValue ("back"));
			} catch {
			}
			try {
				keyRight = (KeyCode)ConfigNode.ParseEnum (typeof(KeyCode), keys.GetValue ("right"));
			} catch {
			}
			try {
				keyLeft = (KeyCode)ConfigNode.ParseEnum (typeof(KeyCode), keys.GetValue ("left"));
			} catch {
			}
			try {
				keyUp = (KeyCode)ConfigNode.ParseEnum (typeof(KeyCode), keys.GetValue ("up"));
			} catch {
			}
			try {
				keyDown = (KeyCode)ConfigNode.ParseEnum (typeof(KeyCode), keys.GetValue ("down"));
			} catch {
			}
			try {
				keyRun = (KeyCode)ConfigNode.ParseEnum (typeof(KeyCode), keys.GetValue ("run"));
			} catch {
			}
			try {
				keySneak = (KeyCode)ConfigNode.ParseEnum (typeof(KeyCode), keys.GetValue ("sneak"));
			} catch {
			}
			try {
				keySwitchMode = (KeyCode)ConfigNode.ParseEnum (typeof(KeyCode), keys.GetValue ("switchMode"));
			} catch {
			}

			var editors = root.GetNode ("EDITORS");
			try {
				vab = ParseEditorConfig (editors.GetNode ("VAB"));
			} catch {
			}
			try {
				sph = ParseEditorConfig (editors.GetNode ("SPH"));
			} catch {
			}

			//return config;
		}

		private Bounds ParseBounds (ConfigNode node)
		{
			var min = ConfigNode.ParseVector3 (node.GetValue ("min"));
			var max = ConfigNode.ParseVector3 (node.GetValue ("max"));
			var result = new Bounds ();
			result.SetMinMax (min, max);
			return result;
		}

		private Config.EditorConfig ParseEditorConfig (ConfigNode node)
		{
			Config.EditorConfig result = new Config.EditorConfig ();

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

	}
}

