using UnityEngine;
using System.Collections.Generic;
using NetworkManager;
using Cars;

namespace GraphicalUI {

	public class FinishedPart : GraphicalUIPart {

		public override void DrawGraphicalUI() {
			if (MainScript.SelfCar.CarObject != null) {
                string text = null;
				if (MainScript.SelfCar.CarObject.state == Behaviours.CarBehaviour.FinishedState.won) {
                    text = "YOU WON";
				} else if (MainScript.SelfCar.CarObject.state == Behaviours.CarBehaviour.FinishedState.lost) {
                    text = "YOU LOSE";
				}

                if(text != null) {
                    GUI.Label (
                        new Rect((Screen.width / 2) - 125, (Screen.height / 2) - 50, (Screen.width / 2), (Screen.height / 2)),
                        "<b><size=60>" + text + "</size></b>"
                    );
                }
			}
		}
	}
}
