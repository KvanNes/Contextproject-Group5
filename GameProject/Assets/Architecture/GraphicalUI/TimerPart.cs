using System;
using UnityEngine;
using Controllers;
using Utilities;

namespace GraphicalUI
{
    public class TimerPart : GraphicalUIPart
    {

        private TimeController timeController;

        public override void Initialize()
        {
            timeController = TimeController.GetInstance();
        }

        public override void DrawGraphicalUI()
        {
            double diff = timeController.GetTime();
            GUI.Label(
                new Rect(Screen.width - 50, 0, 50, 30),
                new GUIContent(Utils.TimeToString(diff)
                    )
            );
        }
    }
}