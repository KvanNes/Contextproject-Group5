using UnityEngine;

namespace Controllers {

	public class CountdownController : MonoBehaviour {
        
        public int CountDownValue;

        public void Start() {
            StopCountdown();
        }

        public void StartCountdown() {
            StopCountdown();
            CountDownValue = 3;
            InvokeRepeating("DecrementCounter", 1f, 1f);
        }

        private void StopCountdown() {
            CountDownValue = -100;  // Make sure not to show countdown when stopped.
            CancelInvoke("DecrementCounter");
        }
        
        public void DecrementCounter() {
            CountDownValue--;
            if (CountDownValue == -3) {
                StopCountdown();
            }
        }

        public bool AllowedToDrive() {
            return CountDownValue <= 0;
        }
    }
}
