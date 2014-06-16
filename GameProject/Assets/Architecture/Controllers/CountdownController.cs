using UnityEngine;
using System;
using NetworkManager;

namespace Controllers {
    // Subclass MonoBehaviour so as to be able to use InvokeRepeating/CancelInvoke.
    public class CountdownController : MonoBehaviour {
        
        public int countDownValue;

        public void Start() {
            StopCountdown();
        }

        public void StartCountdown() {
            StopCountdown();
            countDownValue = 3;
            InvokeRepeating("DecrementCounter", 1f, 1f);
        }

        private void StopCountdown() {
            countDownValue = -100;  // Make sure not to show countdown.
            CancelInvoke("DecrementCounter");
        }
        
        private void DecrementCounter() {
            countDownValue--;
            if (countDownValue == -3) {
                StopCountdown();
            }
        }

        public bool allowedToDrive() {
            return countDownValue <= 0;
        }
    }
}
