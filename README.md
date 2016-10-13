The original release of this was done by FW Industries in May, 2015.

I've enhanced it with the following new features:

	Added configuration window using Dropdownlists for the keys
	Added ability to enable/disable mouse wheel in Editors
	Changed "sneak" key from left shift to left control to avoid conflict with mouse wheel left shift
	Added toolbar icon which turns green when WASD editor is active
	Disable WASD editor when part is selected
	Refactored code to make it more organized
	Replaced logging code with a class



Usage: Move around with WASD keys, hold right mouse to look around. Q/E for up/down. 
Space/leftCtrl to move faster/slower. When you have a part selected it will follow you 
around if right mouse is held. Otherwise it will rotate the part as usual.
Press 5 to switch between stock camera and wasd controls.

The mouse scroll wheel is active when configured.  This makes it easier to use the new editor 
controls while still being able to use the mouse scroll to move up/down and shift-mousescroll to move in and out

Keys, movement speed, etc can be configured in the configuration screen, which is available 
by clicking the WASD icon when in either the VAB or SPF

Known issues:
- The workaround for issue 3838 is a bit hacky and might break things.
  Compatibility mode for EditorExtensions is enabled by default, though.
- The stock VAB camera doesn't have a horizontal offset. So when switching 
  back to stock camera the pivot point snaps back to center. I tried to keep 
  the camera in it's current position and only rotate the view.
- When moving parts around the camera is clamped to editor bounds but the part 
  isn't. If you clip the part through the wall you can't select it anymore. 
  But undo will move it back.
- Unity's Screen.lockCursor resets the cursor to screen center. Used native 
  functions on Windows as workaround. Other OSs are currently stuck with snapping cursors.

Changelog:
- 2015 May 20 Initial release by FW Industries

- 2015 Sept. 16 by Linuxgurugamer
