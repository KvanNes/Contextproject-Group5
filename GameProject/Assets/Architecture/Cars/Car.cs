using Behaviours;
using Mock;
using NetworkManager;
using UnityEngine;
using Utilities;
using UnityException = UnityEngine.UnityException;

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
            this.CarNumber = carNumber;
        }

        public Car()
        {
            CarNumber = ++_carNumberGenerator; // Start with 1.
        }

        public Car(AutoBehaviour gameObject)
        {
            // The car may not have more than the maximum allowed players.
            if (_amountPlayers >= GameData.MAX_PLAYERS_PER_CAR)
            {
                throw new UnityException(GameData.ERROR_AMOUNT_PLAYERS);
            }

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

        public void AddPlayer(Player p)
        {
            // The amount of players should not exceed the maximum allowed.
            if (_amountPlayers >= GameData.MAX_PLAYERS_PER_CAR)
            {
                throw new UnityException(GameData.ERROR_AMOUNT_PLAYERS);
            }

            if (p != null)
            {
                if (p.Role is Driver)
                {
                    Driver = p;
                    Driver.Car = this;
                    ++_amountPlayers;
                }
                else if (p.Role is Throttler)
                {
                    Throttler = p;
                    Throttler.Car = this;
                    ++_amountPlayers;
                }
                else
                {
                    throw new UnityException("Failsafe: The player is not a driver or throttler!");
                }
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