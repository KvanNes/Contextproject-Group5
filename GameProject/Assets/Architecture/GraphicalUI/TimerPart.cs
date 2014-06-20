using Main;
using UnityEngine;
using Controllers;
using Utilities;

namespace GraphicalUI
{
    public class TimerPart : GraphicalUIPart
    {
        private double _timeRunning;
        private TimeController _timeController;

        public override void Initialize()
        {
            _timeController = TimeController.GetInstance();
        }

        public override void DrawGraphicalUI()
        {
            if (!MainScript.AllFinished())
            {
                _timeRunning = _timeController.GetTime();
            }
            GUI.Label(
                new Rect(Screen.width - 50, 0, 50, 30),
                new GUIContent(Utils.TimeToString(_timeRunning))
            );
        }
    }
}