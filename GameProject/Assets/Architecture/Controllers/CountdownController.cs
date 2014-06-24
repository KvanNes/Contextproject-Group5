using Main;
using UnityEngine;
using Utilities;

namespace Controllers
{

    public class CountdownController : MonoBehaviour
    {

        public int CountDownValue;

        public void Start()
        {
            StopCountdown();
        }

        public void StartCountdown()
        {
            StopCountdown();
            CountDownValue = 6;
            InvokeRepeating("DecrementCounter", 1f, 1f);
        }

        private void StopCountdown()
        {
            CountDownValue = -100;  // Make sure not to show countdown when stopped.
            CancelInvoke("DecrementCounter");
        }

        public void DecrementCounter()
        {
            CountDownValue--;
            if (CountDownValue == -3)
            {
                StopCountdown();
            }
            if (CountDownValue == 0)
            {
                TimeController.GetInstance().Reset();
            }
        }

        public bool AllowedToDrive()
        {
            return MainScript.AmountPlayersConnected == GameData.PLAYERS_AMOUNT && CountDownValue <= 0;
        }
    }
}
