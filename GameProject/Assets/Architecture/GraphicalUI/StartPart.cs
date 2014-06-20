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

        public override void Initialize()
        {

        }

        public int AmountPlayers()
        {
            int amount = 0;
            foreach (Car car in MainScript.Cars)
            {
                if (car.Driver != null)
                {
                    Debug.Log("driver not null");
                    amount++;
                }
                if (car.Throttler != null)
                {
                    Debug.Log("throttler not null");
                    amount++;
                }
            }
            return amount;
        }

        public override void DrawGraphicalUI()
        {
            if (MainScript.AmountPlayersConnected < GameData.PLAYERS_AMOUNT)
            {
                Rect waitingRect = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 75, 300, 150);
                GUI.Box(waitingRect, "");

                var centeredStyle = GUI.skin.GetStyle("Label");
                centeredStyle.alignment = TextAnchor.UpperCenter;
                GUI.Label(
                    new Rect(Screen.width / 2 - 150 + 20, Screen.height / 2 + -75 + 20, 250, 100),
                    new GUIContent("Waiting for other players\nWaiting for " + (4 - MainScript.AmountPlayersConnected) + " players"),
                    centeredStyle
                );
            }
        }
    }
}