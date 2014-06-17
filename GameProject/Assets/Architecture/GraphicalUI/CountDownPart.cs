using System;
using UnityEngine;
using NetworkManager;

namespace GraphicalUI {
	
	public class CountDownPart : GraphicalUIPart {
		
		public override void DrawGraphicalUI() {
            int countdownValue = MainScript.CountdownController.CountDownValue;

            string text = null;
            if (countdownValue > 0) {
                text = countdownValue.ToString();
            } else if (-3 <= countdownValue && countdownValue <= 0) {
                text = "GO!";
            }

			if(text != null) {
                GUI.Label(new Rect(Screen.width / 2 - 50, 75, 100, 60), new GUIContent(text));
			}
		}
	}
}