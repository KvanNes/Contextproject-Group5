using System.Collections.Generic;
using UnityEngine;
using NetworkManager;

namespace GraphicalUI
{
    public class TutorialPart : GraphicalUIPart
    {
        private const int Margin = 10;
        private const int ButtonWidth = 150;
        private const int ButtonHeight = 50;

        private readonly List<string> _tutorialStrings = new List<string>
        {
            "To play DuoDrive, pair up with a mate and sit back-to-back.\n\nPick either the driver or throttler job, and make sure to pick the same car number.",
            "As a driver, say to your partner how fast to go. When your partner needs to slow down, you see a 'SLOW' warning." ,
            "As a throttler, say to your partner how to control the steering wheel - you see arrows that signify this."
        };

        private int _currentStringIndex;

        private void DrawNextButton()
        {
            if (GUI.Button(new Rect(Screen.width - ButtonWidth, 0, ButtonWidth, ButtonHeight), new GUIContent("Next")))
            {
                if (_currentStringIndex == _tutorialStrings.Count - 1)
                {
                    MainScript.GUIController.Remove();
                    _currentStringIndex = 0;
                }
                else
                {
                    _currentStringIndex++;
                }
            }
        }

        private void DrawCurrentString()
        {
            Rect rect = new Rect(
                Margin,
                ButtonHeight + Margin,
                Screen.width - 2 * Margin,
                Screen.height - ButtonHeight - 2 * Margin
            );
            GUIContent content = new GUIContent(_tutorialStrings[_currentStringIndex]);
            GUI.Label(rect, content);
        }
        
        public override void OnPush()
        {
            Camera.main.backgroundColor = Color.black;
        }

        public override void DrawGraphicalUI()
        {
            DrawNextButton();
            DrawCurrentString();
        }
    }
}
