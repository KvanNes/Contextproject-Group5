using Main;
using UnityEngine;
using Utilities;

namespace GraphicalUI
{

    public class CountDownPart : GraphicalUIPart
    {
        private GUISkin _countDownSkin;

        private const float LeftMargin = 50;
        private const float TopMargin = 100;

        public override void Initialize()
        {
            _countDownSkin = Resources.Load("countDownSkin") as GUISkin;
        }

        public override void DrawGraphicalUI()
        {
            int countdownValue = MainScript.CountdownController.CountDownValue;
            string text = null;

            if (countdownValue > 3)
            {
                text = "GET READY!";
            }
            else if (countdownValue > 0)
            {
                text = countdownValue.ToString();
            }
            else if (-3 <= countdownValue && countdownValue <= 0)
            {
                text = "GO!";
            }

            if (text != null && MainScript.AmountPlayersConnected == GameData.PLAYERS_AMOUNT)
            {
                Rect finishRect = new Rect(LeftMargin, TopMargin, Screen.width - (2*LeftMargin),
                    Screen.height - (2*TopMargin));
                Rect totalRect = new Rect(0, 0, Screen.width, Screen.height);

                GUI.Box(totalRect, ""); // Make the background transparant
                GUI.Box(finishRect, text, _countDownSkin.GetStyle("Label")); // Create the title
            }
        }
    }
}