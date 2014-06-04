using UnityEngine;
using System;

public interface PlayerRole {
    void Initialize();
	void SendToOther(Car car);
	PlayerAction GetPlayerAction();
	void HandlePlayerAction(AutoBehaviour ab);
    void HandleCollision(AutoBehaviour ab, Collider2D collider);
    void PositionUpdated(AutoBehaviour ab, bool isSelf);
    void RotationUpdated(AutoBehaviour ab, bool isSelf);
}
