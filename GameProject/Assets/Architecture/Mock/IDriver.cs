using Behaviours;
using Cars;

namespace Mock
{
    public interface IDriver
    {
        PlayerAction GetPlayerAction();
        void HandlePlayerAction(AutoBehaviour ab);
    }
}