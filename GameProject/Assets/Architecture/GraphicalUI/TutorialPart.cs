using System.Collections.Generic;
using UnityEngine;
using Main;

namespace GraphicalUI
{
    public class TutorialPart : GraphicalUIPart
    {
        private readonly List<string> _tutorialStrings = new List<string>
        {  
                "To play DuoDrive, pair up with a mate and sit back-to-back.\n" +
                "Pick either the driver or throttler job.\n" +
                "Make sure to pick the same team color.",

                "Tell your partner how fast to go.\n" +
                "In case mud is coming ahead, you see a 'SLOW' warning.\n " +
                "Touch the left/right part of the screen to steer.",

                "Tell your partner how to steer.\n" + 
                "There are arrows visible which indicate this.\n" +
                "Touch the left/right part of the screen to decelerate/accelerate.",

            "Be the first team to reach the finish!"
        };

        private readonly List<string> _tutorialTitleStrings = new List<string>
        {
            "DuoDrive", "Driver", "Throttler", "DuoDrive"
        };

        private int _currentStringIndex;
        private GUISkin _tutorialButtonSkin;
        private GUISkin _tutorialTextSkin;
        private GUISkin _tutorialTitleTextSkin;

        public override void Initialize()
        {
            _tutorialButtonSkin = Resources.Load("tutorialButton") as GUISkin;
            _tutorialTextSkin = Resources.Load("tutorialText") as GUISkin;
            _tutorialTitleTextSkin = Resources.Load("tutorialTitleText") as GUISkin;
        }

        private void DrawNextButton()
        {
            // Button
            float buttonWidth = (float)Screen.width / 7;
            float buttonHeight = (float)Screen.height / 5;
            bool nextButton = GUI.Button(
                new Rect(Screen.width - buttonWidth - 20f, 20f, buttonWidth, buttonHeight),
                new GUIContent("N E X T"),
                _tutorialButtonSkin.GetStyle("Button"));

            if (nextButton)
            {
                if (_currentStringIndex == _tutorialStrings.Count - 1)
                {
                    MainScript.GuiController.Remove();
                    _currentStringIndex = 0;
                }
                else
                {
                    _currentStringIndex++;
                }
            }
        }

        private void DrawBackground()
        {
            // The image of the background 
            Texture2D image = Resources.Load("titleBackground") as Texture2D;

            // Fill the background 
            Rect backgroundRect = new Rect(0, 0, Screen.width, Screen.height);
            GUI.DrawTexture(backgroundRect, image);
        }

        private void DrawCurrentString()
        {
            const float leftFraction = 1f / 28f;
            const float topFraction = 1f / 5f;
            const float widthFraction = 24f / 25f;

            Rect rect = new Rect(
                Screen.width * leftFraction,
                Screen.height * 2.5f * topFraction,
                Screen.width * widthFraction,
                Screen.height
            );
            GUIContent content = new GUIContent(_tutorialStrings[_currentStringIndex]);
            GUIContent titleContent = new GUIContent(_tutorialTitleStrings[_currentStringIndex]);

            GUI.Label(new Rect(Screen.width * leftFraction, Screen.height * 2 * topFraction, Screen.width * widthFraction, Screen.height),
                titleContent,
                _tutorialTitleTextSkin.GetStyle("Label"));
            GUI.Label(rect, content, _tutorialTextSkin.GetStyle("Label"));
        }

        public override void BecomeVisible()
        {
            Camera.main.backgroundColor = Color.black;
        }

        public override void DrawGraphicalUI()
        {
            DrawBackground();
            DrawNextButton();
            DrawCurrentString();
        }
    }
}
