using System;
using UnityEngine;
using NetworkManager;

namespace GraphicalUI {
	
	public class CountDownPart : GraphicalUIPart {
		
		public override void DrawGraphicalUI() {
			if(MainScript.NetworkController.countDownValue == 0) {
				GUI.Label(new Rect(Screen.width / 2 - 50, 75, 100, 60), new GUIContent("GO!"));
			}else{
				GUI.Label(new Rect(Screen.width / 2 - 50, 75, 100, 60), new GUIContent(MainScript.NetworkController.countDownValue.ToString()));
			}
		}
	}
}