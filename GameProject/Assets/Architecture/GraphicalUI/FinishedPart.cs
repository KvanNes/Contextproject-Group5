using System;
using Main;
using UnityEngine;
using Utilities;

namespace GraphicalUI
{

    public class FinishedPart : GraphicalUIPart
    {
        private GUISkin _titleSkin;
        private GUISkin _textSkin;
        private GUISkin _winSkin;

        private float _personalFinishTime;

        private const float LeftPadding = 20;
        private const float TopPadding = 35;
        private const float LeftMargin = 50;
        private const float TopMargin = 100;

        public override void Initialize()
        {
            _titleSkin = Resources.Load("finishTitleSkin") as GUISkin;
            _textSkin = Resources.Load("finishTextSkin") as GUISkin;
            _winSkin = Resources.Load("finishWinTextSkin") as GUISkin;
        }

        public override void DrawGraphicalUI()
        {
            Rect finishRect = new Rect(LeftMargin, TopMargin, Screen.width - (2 * LeftMargin), Screen.height - (2 * TopMargin));
            Rect totalRect = new Rect(0, 0, Screen.width, Screen.height);


            if (MainScript.SelfCar.CarObject != null && MainScript.AmountPlayersConnected == GameData.PLAYERS_AMOUNT)
            {
                if (MainScript.SelfCar.CarObject.Finished)
                {
                    // Set the personal finishing time
                    _personalFinishTime = MainScript.SelfCar.CarObject.FinishedTime;

                    GUI.Box(totalRect, ""); // Make the background transparant
                    GUI.Box(finishRect, "Ranking board", _titleSkin.GetStyle("Label")); // Create the title

                    // BEGIN GROUP
                    GUI.BeginGroup(finishRect);

                    string personalTimeText = "Your time: " + Utils.TimeToString(_personalFinishTime);
                    GUI.Label(
                        new Rect(LeftPadding, 100 - 20, finishRect.width - LeftPadding, 100),
                        new GUIContent(personalTimeText), _textSkin.GetStyle("Label")
                    );

                    MainScript.SelfPlayer.Role.Finished();


                    // In case that all cars have finished
                    if (MainScript.AllFinished())
                    {
                        string firstPlace = "1.   " + Utils.TimeToString(MainScript.TimeAtPlace(true));
                        string secondPlace = "2.   " + Utils.TimeToString(MainScript.TimeAtPlace(false));

                        GUIStyle firstPlacestyle = Math.Abs(_personalFinishTime - MainScript.TimeAtPlace(true)) < 0.001f
                            ? _winSkin.GetStyle("Label")
                            : _textSkin.GetStyle("Label");
                        GUIStyle secondPlacestyle = Math.Abs(_personalFinishTime - MainScript.TimeAtPlace(true)) < 0.001f
                            ? _textSkin.GetStyle("Label")
                            : _winSkin.GetStyle("Label");


                        GUI.Label(
                            new Rect(LeftPadding, 100 + TopPadding, finishRect.width - LeftPadding, 100),
                            new GUIContent(firstPlace), firstPlacestyle
                        );

                        GUI.Label(
                            new Rect(LeftPadding, 100 + (2 * TopPadding), finishRect.width - LeftPadding, 100),
                            new GUIContent(secondPlace), secondPlacestyle
                        );

                    }

                    // CLOSE GROUP
                    GUI.EndGroup();
                }
            }
        }
    }
}
