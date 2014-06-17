/**
 * This class contains all the data that does not change during gameplay. They are all static and readonly.
 */
namespace Utilities
{
    public class GameData
    {
        // Final values
        public static readonly float TOLERANCE = 0.0001f;
        public static readonly int PORT = 25001;
        public static readonly int MAX_PLAYERS_PER_CAR = 2;
        public static readonly int CARS_AMOUNT = 2;
        public static readonly float UPDATE_TIME_DELTA = 0.05f;

        // Constants used for physics.
        public static readonly float MIN_SPEED = -0.15f;
        public static readonly float MAX_SPEED = 0.3f;
        public static readonly float MIN_ACCELERATION = -0.005f;
        public static readonly float MAX_ACCELERATION = 0.03f;
        public static readonly float ACCELERATION_DECREASE = -0.35f;
        public static readonly float ACCELERATION_INCREASE = 0.3f;
        public static readonly float FRICTION_AMOUNT = 0.07f;
        public static readonly float COLLISION_FACTOR = 0.8f;
        public static readonly float SLIDE_SLOWDOWN = 0.08f;
        public static readonly float BOUNCE_AMOUNT = 0.05f;

        // Game name used for networking.
        public static readonly string GAME_NAME = "CGCookie_DuoDrive";

        // The ERROR messages
        public static readonly string ERROR_AMOUNT_PLAYERS = "Caution: The maximum amount of players has been reached";

        // METHOD STRINGS
        public static readonly string MESSAGE_PING_TIME = "PingTime";
    }
}

