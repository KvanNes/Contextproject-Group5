using Behaviours;
using Cars;
using UnityEngine;

namespace Interfaces
{
    public interface IPlayerRole
    {
        void Initialize();
        void Finished();
        void Restart();
        void SendToOther(Car car);
        PlayerAction GetPlayerAction();
        void HandlePlayerAction(CarBehaviour ab);
        void HandleCollision(CarBehaviour ab, Collision2D collision);
        void HandleTrigger(CarBehaviour ab, Collider2D collider);
        void PositionUpdated(CarBehaviour ab, bool isSelf);
        void RotationUpdated(CarBehaviour ab, bool isSelf);
    }
}