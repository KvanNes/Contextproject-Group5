using System;
using System.ComponentModel;
using Interfaces;
using Wrappers;

namespace Controllers
{
    public class TimeController
    {

        public INetwork Network;
        private static TimeController _instance;

        private double _startTime;
        private double _stopTime = -1;

        private const float Delta = 0.0001f;

        private TimeController()
        {
            if (Network != null)
                Reset();
        }

        public static TimeController GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TimeController { Network = new NetworkWrapper() };
            }
            return _instance;
        }

        public void ResetTimer()
        {
            _startTime = Network.GetTime();
        }

        public void StopTimer()
        {
            _stopTime = Network.GetTime();
        }

        public void ResetStopTime()
        {
            _stopTime = -1;
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
                return Network.GetTime() - _startTime;
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