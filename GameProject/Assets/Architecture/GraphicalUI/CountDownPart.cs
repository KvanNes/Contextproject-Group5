using System;
using UnityEngine;
using NetworkManager;

namespace GraphicalUI {
	
	public class CountDownPart : GraphicalUIPart {
		
		public override void DrawGraphicalUI() {
            int countdownValue = MainScript.NetworkController.countDownValue;
            if(-3 <= countdownValue && countdownValue <= 0) {
				GUI.Label(new Rect(Screen.width / 2 - 50, 75, 100, 60), new GUIContent("GO!"));
			} else if(countdownValue > 0) {
                GUI.Label(new Rect(Screen.width / 2 - 50, 75, 100, 60), new GUIContent(countdownValue.ToString()));
			}
		}
	}
}