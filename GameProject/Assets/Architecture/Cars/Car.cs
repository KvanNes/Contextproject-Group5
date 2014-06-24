using Behaviours;
using Interfaces;
using Main;
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

        // Constructor that sets the carnumber.
        public Car(int carNumber)
        {
            CarNumber = carNumber;
        }

        // Regular constructor to create a Car-object.
        public Car()
        {
            CarNumber = ++_carNumberGenerator;
        }

        // Create Car with crtain CarBehaviour
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
                   && CarObject.PositionInitialized
                   && CarObject.RotationInitialized;
        }

        // Send to the other players what role this player is.
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

        // Reset the position of the Car.
        public void ResetCar(Vector3 pos)
        {
            Quaternion rot = Quaternion.identity;
            CarObject.Acceleration = 0f;
            CarObject.NetworkView.RPC("UpdatePosition", RPCMode.All, pos, 0f, CarNumber - 1);
            CarObject.NetworkView.RPC("UpdateRotation", RPCMode.All, rot, CarNumber - 1);
        }
    }
}