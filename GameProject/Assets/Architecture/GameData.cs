using System;

/**
 * This class contains all the data that does not change during gameplay. They are all readonly.
 */
public class GameData
{
    // Final values
    public static readonly int PORT = 25001;
    public static readonly int MAX_PLAYERS_PER_CAR = 2;

    // The ERROR messages
    public static readonly string ERROR_AMOUNT_PLAYERS = "Caution: The maximum amount of players has been reached";

    // METHOD STRINGS
    public static readonly string MESSAGE_PLAYER_CONNECTED = "PlayerConnected";
    public static readonly string MESSAGE_AMOUNT_PLAYERS = "AmountPlayersConnected";
    public static readonly string MESSAGE_PING_TIME = "PingTime";


}

