public interface IDriver
{
    PlayerAction GetPlayerAction();
    void HandlePlayerAction(AutoBehaviour ab);
}