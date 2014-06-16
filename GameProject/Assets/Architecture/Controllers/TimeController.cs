using System;
using UnityEngine;

namespace Controllers {
	public class TimeController {
		
		private static TimeController instance;
		
		private double StartTime = 0;
		private double StopTime = -1;
		
		private TimeController() {
			Reset();
		}
		
		public static TimeController getInstance() {
			if(instance == null) {
				instance = new TimeController();
			}
			return instance;
		}
		
		public void resetTimer() {
			this.StartTime = Network.time;
		}
		
		public void stopTimer() {
			this.StopTime = Network.time;
		}
		
		public void resetStopTime() {
			this.StopTime = -1;
		}

        public void Reset() {
            resetTimer();
            resetStopTime();
        }
		
		public double getTime() {
			if(StopTime == -1)
            {
				return Network.time - StartTime;
			} 
			else 
			{
				return StopTime - StartTime;
			}
		}	
	}
}