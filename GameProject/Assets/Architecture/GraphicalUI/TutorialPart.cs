using System.Collections.Generic;
using UnityEngine;
using NetworkManager;

namespace GraphicalUI {
    public class TutorialPart : GraphicalUIPart {
        
        private static readonly int MARGIN = 10;
        private static readonly int BUTTON_WIDTH = 150;
        private static readonly int BUTTON_HEIGHT = 50;
        
        private readonly List<string> TutorialStrings = new List<string>() {
            { "To play DuoDrive, pair up with a mate and sit back-to-back.\n\nPick either the driver or throttler job, and make sure to pick the same car number." },
            { "As a driver, say to your partner how fast to go. When your partner needs to slow down, you see a 'SLOW' warning." },
            { "As a throttler, say to your partner how to control the steering wheel - you see arrows that signify this." }
        };
        
        private int CurrentStringIndex = 0;
        
        private void DrawNextButton() {
            if (GUI.Button(new Rect(Screen.width - BUTTON_WIDTH, 0, BUTTON_WIDTH, BUTTON_HEIGHT), new GUIContent("Next"))) {
                if (CurrentStringIndex == TutorialStrings.Count - 1) {
                    MainScript.GUIController.Remove();
                    CurrentStringIndex = 0;
                } else {
                    CurrentStringIndex++;
                }
            }
        }
        
        private void DrawCurrentString() {
            Rect rect = new Rect(
                MARGIN,
                BUTTON_HEIGHT + MARGIN,
                Screen.width - 2 * MARGIN,
                Screen.height - BUTTON_HEIGHT - 2 * MARGIN
            );
            GUIContent content = new GUIContent(TutorialStrings[CurrentStringIndex]);
            GUI.Label(rect, content);
        }
        
        public override void DrawGraphicalUI() {
            DrawNextButton();
            DrawCurrentString();
        }
    }
}
