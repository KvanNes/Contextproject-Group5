using System;
using UnityEngine;
using Controllers;

namespace GraphicalUI {
	public class TimerPart : GraphicalUIPart {
		
        private TimeController timeController;

        public override void Initialize() {
            timeController = TimeController.getInstance();
        }
		
		public override void DrawGraphicalUI() {
			double diff = timeController.getTime();
			int minutes = (int)Math.Floor(diff / 60);
			int seconds = (int)Math.Floor(diff % 60);
			GUI.Label(
				new Rect(Screen.width - 50, 0, 50, 30),
				new GUIContent(minutes.ToString("D2") + ":" + seconds.ToString("D2"))
			);
		}
	}
}