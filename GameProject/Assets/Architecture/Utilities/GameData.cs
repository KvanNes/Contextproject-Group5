/**
 * This class contains all the data that does not change during gameplay. They are all static and readonly.
 */
using UnityEngine;

namespace Utilities
{
    public class GameData
    {

        // Constants used for physics.
        public static readonly int COLLISION_POINTS_AMOUNT = 12;
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
        public static readonly float ROTATION_SPEED_FACTOR = 200f;
        public static readonly float MUD_SLOWDOWN_FACTOR = 0.5f;
        public static readonly float MINIMUM_SLIDE_ANGLE = 60f;

        // Game name used for networking.
        public static readonly string GAME_NAME = "DuoDrive";

        // Component strings.
        public static readonly string TAG_FINISH = "Finish";
        public static readonly string TAG_MUD = "Mud";
        public static readonly string NAME_SPHERE = "Sphere";

        // Connection options.
        public static readonly bool USE_HARDCODED_IP = false;
        public static readonly string IP = "127.0.0.1";
        public static readonly int PORT = 25001;

        // Miscellaneous.
        public static readonly int CARS_AMOUNT = 2;
        public static readonly int PLAYERS_AMOUNT = 4;
        public static readonly float UPDATE_TIME_DELTA = 0.05f;
        public static readonly int SCREEN_MIDDLE_COLUMN = Screen.width / 2;
    }
}

