using Mock;
using UnityEngine;

namespace Cars
{
    public enum PlayerAction
    {
        SpeedUp,
        SpeedDown,
        SteerLeft,
        SteerRight,
        None
    };

    public class Player
    {

        public Player()
        {
            Role = null;
            Car = null;
        }

        public ICar Car { get; set; }

        public NetworkPlayer NetworkPlayer { get; set; }

        public IPlayerRole Role { get; set; }

        public Player(Car car, IPlayerRole role)
        {
            Car = car;
            Role = role;
        }

        public Player(ICar car, IPlayerRole role)
        {
            Car = car;
            Role = role;
        }
    }
}