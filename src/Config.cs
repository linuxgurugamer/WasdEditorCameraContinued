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

		#if false
		public void setConfig(Config cfg)
		{
			keyForward = cfg.keyForward;
			keyBack = cfg.keyBack;
			keyRight = cfg.keyRight;
			keyLeft = cfg.keyLeft;
			keyUp = cfg.keyUp;
			keyDown = cfg.keyDown;
			keyRun = cfg.keyRun;
			keySneak = cfg.keySneak;
			keySwitchMode = cfg.keySwitchMode;

			enableExperimentalEditorExtensionsCompatibility = cfg.enableExperimentalEditorExtensionsCompatibility;
			defaultCamera = cfg.defaultCamera;
			enforceBounds = cfg.enforceBounds;
			mouseWheelActive = cfg.mouseWheelActive;

			sensitivity = cfg.sensitivity;
			acceleration = cfg.acceleration;
			friction = cfg.friction;
			runMultiplier = cfg.runMultiplier;
			sneakMultiplier = cfg.sneakMultiplier;
			mouseWheelAcceleration = cfg.mouseWheelAcceleration;

			vab = cfg.vab;
			sph = cfg.sph;

		}
		#endif

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

