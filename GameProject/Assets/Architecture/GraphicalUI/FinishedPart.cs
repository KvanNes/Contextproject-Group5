using UnityEngine;
using NetworkManager;
using Cars;
using Utilities;

namespace GraphicalUI
{

    public class FinishedPart : GraphicalUIPart
    {

        public override void DrawGraphicalUI()
        {
            if (MainScript.SelfCar.CarObject != null)
            {
                float screenWidth = (float)Screen.width / 2;
                float screenHeight = (float)Screen.height / 2;

                if (MainScript.AllFinished())
                {
                    string timeRankings = "<b><size=30>";
                    timeRankings += "First place: " + Utils.TimeToString(MainScript.TimeAtPlace(true)) + "\n";
                    timeRankings += "Second place: " + Utils.TimeToString(MainScript.TimeAtPlace(false)) + "\n";
                    timeRankings += "</size></b>";


                    GUI.Label(
                    new Rect((Screen.width / 2) - 125, (Screen.height / 2) - 50, screenWidth, screenHeight),
                        timeRankings
                    );
                }

                if (MainScript.SelfCar.CarObject.Finished)
                {
                    string personalTimeText = "Your time: " + Utils.TimeToString(MainScript.SelfCar.CarObject.FinishedTime) + "\n";
                    GUI.Label(
                    new Rect((Screen.width / 2) - 100, 50, screenWidth, screenHeight),
                        personalTimeText
                    );
                    MainScript.SelfPlayer.Role.Finished();
                }


            }
        }
    }
}
