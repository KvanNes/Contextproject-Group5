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
        void HandleCollision(AutoBehaviour ab, Collision2D collision);
        void HandleTrigger(AutoBehaviour ab, Collider2D collider);
		void MoveCameraWhenPositionUpdated(AutoBehaviour ab, bool isSelf);
		void MoveCameraWhenRotationUpdated(AutoBehaviour ab, bool isSelf);
    }
}