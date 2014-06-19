using Behaviours;
using Interfaces;
using NetworkManager;
using UnityEngine;

namespace Cars
{
    public class Car : ICar
    {
        public Player Driver { get; set; }
        public Player Throttler { get; set; }

        public CarBehaviour CarObject { get; set; }

        private static int _carNumberGenerator;
        public int CarNumber { get; set; }

        public Car(int carNumber)
        {
            CarNumber = carNumber;
        }

        public Car()
        {
            CarNumber = ++_carNumberGenerator;
        }

        public Car(CarBehaviour gameObject)
        {
            CarObject = gameObject;
        }

        //Checks whether a player is connected to a LAN.
        private bool ShouldSend()
        {
            return CarObject != null
                   && MainScript.SelfCar == this
                   && Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork
                   && CarObject.Initialized == (CarBehaviour.PositionInitialized | CarBehaviour.RotationInitialized);
        }

        public void SendToOther()
        {
            if (ShouldSend())
            {
                MainScript.SelfPlayer.Role.SendToOther(this);
            }
        }

        // Method needed for the test, so that the static carNumberGenerator does not keep incrementing.
        public void Reset()
        {
            _carNumberGenerator = 0;
        }
    }
}