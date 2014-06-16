using UnityEngine;
using System.Collections.Generic;
using NetworkManager;
using Cars;

namespace GraphicalUI {
	public class FinishedPart : GraphicalUIPart {

		public override void DrawGraphicalUI() {
			if (MainScript.SelfCar.CarObject.state == Behaviours.AutoBehaviour.FinishedState.won) {
				GUI.Label (
					new Rect (Screen.width - 50, 0, 50, 30),
					new GUIContent ("YOU WON")
				);
			} else if (MainScript.SelfCar.CarObject.state == Behaviours.AutoBehaviour.FinishedState.lost) {
				GUI.Label (
					new Rect (Screen.width - 50, 0, 50, 30),
					new GUIContent ("YOU LOST")
				);
			}
		}
	}
}
