using UnityEngine;
using System.Collections.Generic;
using NetworkManager;
using Cars;
using Utilities;

namespace GraphicalUI {

	public class FinishedPart : GraphicalUIPart {

		public override void DrawGraphicalUI() {
			if (MainScript.SelfCar.CarObject != null) {
                string text = null;
                /*
				if (MainScript.SelfCar.CarObject.state == Behaviours.AutoBehaviour.FinishedState.won) {
                    text = "YOU WIN";
				} else if (MainScript.SelfCar.CarObject.state == Behaviours.AutoBehaviour.FinishedState.lost) {
                    text = "YOU LOSE";
				}
                 */

			    if (MainScript.SelfCar.CarObject.Finished)
			    {
			        text = "<b><size=60>";
			        text += "You: " + Utils.TimeToString(MainScript.SelfCar.CarObject.FinishedTime) + "\n";
                    text += "</size></b>";
			    }

			    if (MainScript.AllFinished())
			    {
                    text = "<b><size=60>";
                    text += "You: " + Utils.TimeToString(MainScript.SelfCar.CarObject.FinishedTime) + "\n";
                    text += "Other: " + Utils.TimeToString(MainScript.OtherCar().CarObject.FinishedTime) + "\n";
                    text += "</size></b>";
			    }

			    if(text != null) {
                    GUI.Label (
                        new Rect((Screen.width / 2) - 125, (Screen.height / 2) - 50, (Screen.width / 2), (Screen.height / 2)),
                        text
                    );
                }
			}
		}
	}
}
