using System;
using System.Linq;
using Cars;
using NetworkManager;
using UnityEngine;
using Controllers;
using Utilities;

namespace GraphicalUI
{
    public class StartPart : GraphicalUIPart
    {
        private GUISkin _titleSkin;
        private GUISkin _textSkin;

        public override void Initialize()
        {
            _titleSkin = Resources.LoadAssetAtPath("Assets/GUISkins/startTitleSkin.guiskin", typeof(GUISkin)) as GUISkin;
            _textSkin = Resources.LoadAssetAtPath("Assets/GUISkins/startTextSkin.guiskin", typeof(GUISkin)) as GUISkin;
        }

        public override void DrawGraphicalUI()
        {
            const float leftPadding = 20;
            const float topPadding = 35;
            const float leftMargin = 50;
            const float topMargin = 100;

            Rect finishRect = new Rect(leftMargin, topMargin, Screen.width - (2 * leftMargin), Screen.height - (2 * topMargin));
            Rect totalRect = new Rect(0, 0, Screen.width, Screen.height);

            GUI.Box(totalRect, ""); // Make the background transparant
            GUI.Box(finishRect, "Waiting for other players", _titleSkin.GetStyle("Label")); // Create the title

            // BEGIN GROUP
            GUI.BeginGroup(finishRect);

            // Set the text for waiting for players
            GUI.Label(
                new Rect(leftPadding, 100 + topPadding, finishRect.width - leftPadding, 100),
                new GUIContent("<size=45>" + (GameData.PLAYERS_AMOUNT - MainScript.AmountPlayersConnected) + "</size>   players left"), _textSkin.GetStyle("Label")
            );

            // END GROUP
            GUI.EndGroup();

        }
    }
}