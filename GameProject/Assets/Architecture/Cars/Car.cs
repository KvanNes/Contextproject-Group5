using Behaviours;
using Interfaces;
using NetworkManager;
using UnityEngine;

namespace Cars
{
    public class Car : ICar
    {

        // Players for the car variables
        public Player Driver { get; set; }
        public Player Throttler { get; set; }

        // The Unity variables
        public AutoBehaviour CarObject { get; set; }

        // Variables
        private int _amountPlayers;

        private static int _carNumberGenerator;
        public int CarNumber { get; set; }

        public Car(int carNumber)
        {
            CarNumber = carNumber;
        }

        public Car()
        {
            CarNumber = ++_carNumberGenerator; // Start with 1.
        }

        public Car(AutoBehaviour gameObject)
        {
            CarObject = gameObject;
        }

        private bool ShouldSend()
        {
            return CarObject != null
                   && MainScript.SelfCar == this
                   && Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork
                   && CarObject.Initialized == (AutoBehaviour.PositionInitialized | AutoBehaviour.RotationInitialized);
        }

        public void SendToOther()
        {
            if (ShouldSend())
            {
                MainScript.SelfPlayer.Role.SendToOther(this);
            }
        }

        public int GetAmountPlayers()
        {
            return _amountPlayers;
        }

        public void Clear()
        {
            Driver = null;
            Throttler = null;
            CarObject = null;
            _amountPlayers = 0;
            CarNumber = 0;
            _carNumberGenerator = 0;
        }
    }
}