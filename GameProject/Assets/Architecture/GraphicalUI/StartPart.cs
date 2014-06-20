using Main;
using UnityEngine;
using Utilities;

namespace GraphicalUI
{
    public class StartPart : GraphicalUIPart
    {
        private GUISkin _titleSkin;
        private GUISkin _textSkin;

        private const float LeftPadding = 20;
        private const float TopPadding = 35;
        private const float LeftMargin = 50;
        private const float TopMargin = 100;

        public override void Initialize()
        {
            _titleSkin = Resources.LoadAssetAtPath("Assets/GUISkins/startTitleSkin.guiskin", typeof(GUISkin)) as GUISkin;
            _textSkin = Resources.LoadAssetAtPath("Assets/GUISkins/startTextSkin.guiskin", typeof(GUISkin)) as GUISkin;
        }

        public override void DrawGraphicalUI()
        {
            Rect finishRect = new Rect(LeftMargin, TopMargin, Screen.width - (2 * LeftMargin), Screen.height - (2 * TopMargin));
            Rect totalRect = new Rect(0, 0, Screen.width, Screen.height);

            GUI.Box(totalRect, ""); // Make the background transparant
            GUI.Box(finishRect, "Waiting for other players", _titleSkin.GetStyle("Label")); // Create the title

            // BEGIN GROUP
            GUI.BeginGroup(finishRect);

            // Set the text for waiting for players
            GUI.Label(
                new Rect(LeftPadding, 100 + TopPadding, finishRect.width - LeftPadding, 100),
                new GUIContent("<size=45>" + (GameData.PLAYERS_AMOUNT - MainScript.AmountPlayersConnected) + "</size>   players left"), _textSkin.GetStyle("Label")
            );

            // END GROUP
            GUI.EndGroup();

        }
    }
}