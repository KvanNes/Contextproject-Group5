using UnityEngine;
using System.Collections.Generic;
using NetworkManager;
using Cars;

namespace GraphicalUI {
	public class FinishedPart : GraphicalUIPart {
		public GUIStyle style;
		public override void DrawGraphicalUI() {
			if (MainScript.SelfCar.CarObject != null) {
				if (MainScript.SelfCar.CarObject.state == Behaviours.AutoBehaviour.FinishedState.won) {
					GUI.Label (
						new Rect ((Screen.width / 2) - 125, (Screen.height / 2) - 50, (Screen.width / 2), (Screen.height / 2)),
						"<b><size=60>YOU WON</size></b>"
					);
				} else if (MainScript.SelfCar.CarObject.state == Behaviours.AutoBehaviour.FinishedState.lost) {
					GUI.Label (
						new Rect ((Screen.width / 2) - 125, (Screen.height / 2) - 50, (Screen.width / 2), (Screen.height / 2)),
						"<b><size=60>YOU LOSE</size></b>"
					);
				}
			}
		}
	}
}
