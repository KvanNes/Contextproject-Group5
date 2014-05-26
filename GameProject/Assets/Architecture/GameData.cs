using System;

/**
 * This class contains all the data that does not change during gameplay. They are all readonly.
 */
public class GameData
{
    // Final values
    public static readonly int PORT = 25001;
    public static readonly int MAX_PLAYERS_PER_CAR = 2;
    public static readonly int CARS_AMOUNT = 2;
    public static readonly float UPDATE_TIME_DELTA = 0.05f;
    
    // Constants used for physics.
    public static readonly float minSpeed = -0.1f;
    public static readonly float maxSpeed = 0.3f;
    public static readonly float minAcceleration = -0.005f;
    public static readonly float maxAcceleration = 0.005f;
    public static readonly float accelerationDecrease = -1f;
    public static readonly float accelerationIncrease = 0.4f;

    // The ERROR messages
    public static readonly string ERROR_AMOUNT_PLAYERS = "Caution: The maximum amount of players has been reached";

    // METHOD STRINGS
    public static readonly string MESSAGE_PING_TIME = "PingTime";


}

