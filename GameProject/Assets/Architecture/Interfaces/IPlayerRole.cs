using Behaviours;
using Cars;
using UnityEngine;

namespace Interfaces
{
    public interface IPlayerRole
    {
        void Initialize();
        void SendToOther(Car car);
        PlayerAction GetPlayerAction();
        void HandlePlayerAction(AutoBehaviour ab);
        void HandleCollision(AutoBehaviour ab, Collider2D collider);
        void PositionUpdated(AutoBehaviour ab, bool isSelf);
        void RotationUpdated(AutoBehaviour ab, bool isSelf);
    }
}