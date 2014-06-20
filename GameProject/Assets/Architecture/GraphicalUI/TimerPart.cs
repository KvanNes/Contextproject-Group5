using Cars;
using NetworkManager;
using UnityEngine;
using Controllers;
using Utilities;

namespace GraphicalUI
{
    public class TimerPart : GraphicalUIPart
    {
        private double _timeRunning;
        private bool _gameStarted;
        private TimeController _timeController;

        public override void Initialize()
        {
            _timeController = TimeController.GetInstance();
        }

        public override void DrawGraphicalUI()
        {
            if (!MainScript.AllFinished() && MainScript.CountdownController.AllowedToDrive())
            {
                _gameStarted = true;
                _timeRunning = _timeController.GetTime();
            }
            else
            {
                _gameStarted = false;
            }
            GUI.Label(
                new Rect(Screen.width - 50, 0, 50, 30),
                new GUIContent(_gameStarted ? Utils.TimeToString(_timeRunning) : "")
            );
        }
    }
}