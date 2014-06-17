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
		void HandlePlayerAction(AutoBehaviour autoBehaviour);
		void HandleCollision(AutoBehaviour autoBehaviour, Collision2D collision);
		void HandleTrigger(AutoBehaviour autoBehaviour, Collider2D collider);
		void MoveCameraWhenPositionUpdated(AutoBehaviour autoBehaviour, bool isSelf);
		void MoveCameraWhenRotationUpdated(AutoBehaviour autoBehaviour, bool isSelf);
    }
}