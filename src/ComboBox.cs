using System;
using UnityEngine;
#if true
namespace WasdEditorCamera
{
	

	// Popup list created by Eric Haines
	// ComboBox Extended by Hyungseok Seo.(Jerry) sdragoon@nate.com
	// Scrollview added by Luis Anton http://playmedusa.com  Though it may need some tweaks!
	//
	// -----------------------------------------------
	// This code working like ComboBox Control.
	// I just changed some part of code,
	// because I want to seperate ComboBox button and List.
	// ( You can see the result of this code from Description's last picture )
	// -----------------------------------------------
	//
	// === usage ======================================
	//
	// public class SomeClass : MonoBehaviour
	// {
	//  GUIContent[] comboBoxList;
	//  private ComboBox comboBoxControl = new ComboBox();
	//  private GUIStyle listStyle = new GUIStyle();
	//
	//  private void Start()
	//  {
	//      comboBoxList = new GUIContent[5];
	//      comboBoxList[0] = new GUIContent("Thing 1");
	//      comboBoxList[1] = new GUIContent("Thing 2");
	//      comboBoxList[2] = new GUIContent("Thing 3");
	//      comboBoxList[3] = new GUIContent("Thing 4");
	//      comboBoxList[4] = new GUIContent("Thing 5");
	//
	//      listStyle.normal.textColor = Color.white;
	//      listStyle.onHover.background =
	//      listStyle.hover.background = new Texture2D(2, 2);
	//      listStyle.padding.left =
	//      listStyle.padding.right =
	//      listStyle.padding.top =
	//      listStyle.padding.bottom = 4;
	//  }
	//
	//  private void OnGUI ()
	//  {
	//      int selectedItemIndex = comboBoxControl.GetSelectedItemIndex();
	//      selectedItemIndex = comboBoxControl.List(
	//      new Rect(50, 100, 100, 20), comboBoxList[selectedItemIndex].text, comboBoxList, listStyle );
	//          GUI.Label( new Rect(50, 70, 400, 21),
	//      "You picked " + comboBoxList[selectedItemIndex].text + "!" );
	//  }
	// }
	//
	// =================================================

	using UnityEngine;

	public class ComboBox
	{
		private static bool forceToUnShow = false;
		private static int useControlID = -1;
		private bool isClickedComboButton = false;  
		private Vector2 scrollViewVector =  Vector2.zero;

		private int selectedItemIndex = 0;  

		private static ComboBox current = null;

		public int List( Rect rect, string buttonText, GUIContent[] listContent, GUIStyle listStyle )
		{
			return List( rect, new GUIContent( buttonText ), listContent, "button", "box", listStyle );
		}

		public int List( Rect rect, GUIContent buttonContent, GUIContent[] listContent, GUIStyle listStyle )
		{
			return List( rect, buttonContent, listContent, "button", "box", listStyle);
		}

		public int List( Rect rect, string buttonText, GUIContent[] listContent, GUIStyle buttonStyle, GUIStyle boxStyle, GUIStyle listStyle )
		{
			return List( rect, new GUIContent( buttonText ), listContent, buttonStyle, boxStyle, listStyle );
		}

		public int List( Rect rect, GUIContent buttonContent, GUIContent[] listContent,
			GUIStyle buttonStyle, GUIStyle boxStyle, GUIStyle listStyle )
		{
			if (current != null) {
				if (current != this)
					return selectedItemIndex;
			} 
			

			if( forceToUnShow )
			{
				forceToUnShow = false;
				isClickedComboButton = false;        
			}

			bool done = false;
			int controlID = GUIUtility.GetControlID( FocusType.Passive );    

			switch( Event.current.GetTypeForControl(controlID) )
			{
			case EventType.mouseUp:
				{  
					if( isClickedComboButton )
					{
						done = true;
					}
				}
				break;
			}      

			if (GUI.Button (rect, buttonContent, buttonStyle)) {
				if (current != null) {
					if (current != this)
						return selectedItemIndex;
				} else
					current = this;


				if (useControlID == -1) {
					useControlID = controlID;
					//isClickedComboButton = false;
				} 
					if (useControlID != controlID) {
						forceToUnShow = true;
						useControlID = controlID;
					}
				
					isClickedComboButton = true;

			}

			if( isClickedComboButton )
			{
				float items_height = listStyle.CalcHeight(listContent[0], 1.0f) * (listContent.Length + 5);

				Rect listRect = new Rect( rect.x, rect.y + listStyle.CalcHeight(listContent[0], 1.0f), rect.width, items_height);
			
				scrollViewVector = GUI.BeginScrollView (new Rect (rect.x, rect.y + rect.height, rect.width /* * 1.4f */, 200), scrollViewVector,
					new Rect (rect.x, rect.y, rect.width, items_height + rect.height), false, false);  

				GUI.Box( new Rect(rect.x, rect.y, rect.width, items_height + rect.height) , "", boxStyle );

				int newSelectedItemIndex = GUI.SelectionGrid( listRect, selectedItemIndex, listContent, 1, listStyle );

				if( newSelectedItemIndex != selectedItemIndex ) {
					selectedItemIndex = newSelectedItemIndex;
					done = true;
				}

				GUI.EndScrollView();
			}

			if (done) {
				isClickedComboButton = false;
				forceToUnShow = false;
				useControlID = -1;
				current = null;
			}

			return GetSelectedItemIndex();
		}

		public int GetSelectedItemIndex()
		{
			return selectedItemIndex;
		}

		public  void SetSelectedItemIndex(string str, GUIContent[] listContent)
		{
			for (int i = 0; i < listContent.Length; i++) {
				if (listContent [i].text == str) {
					selectedItemIndex = i;
					return;
				}
			}
			selectedItemIndex = 0;
		}
	}


}

#endif