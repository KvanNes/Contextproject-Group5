using System;
using UnityEngine;

namespace Controllers
{
    public class TimeController
    {

        private static TimeController _instance;

        private double _startTime;
        private double _stopTime = -1;

        private const float Delta = 0.0001f;

        private TimeController()
        {
            Reset();
        }

        public static TimeController GetInstance()
        {
            return _instance ?? (_instance = new TimeController());
        }

        public void ResetTimer()
        {
            _startTime = Network.time;
        }

        public void StopTimer()
        {
            _stopTime = Network.time;
        }

        public void ResetStopTime()
        {
            _stopTime = -1;
        }

        public void ResetStartTime()
        {
            _startTime = 0;
        }

        public void Reset()
        {
            ResetTimer();
            ResetStopTime();
        }

        public double GetTime()
        {
            if (Math.Abs(_stopTime - (-1)) < Delta)
            {
                return Network.time - _startTime;
            }
            return _stopTime - _startTime;
        }

        public double GetStartTime()
        {
            return _startTime;
        }
        public double GetStopTime()
        {
            return _stopTime;
        }

        public void SetStopTime(double stopTime)
        {
            _stopTime = stopTime;
        }

        public void Clear()
        {
            _instance = null;
        }
    }
}