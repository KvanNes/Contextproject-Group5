using System;

public interface PlayerRole {
    void Initialize();
	void SendToOther(Car car);
	PlayerAction GetPlayerAction();
	void HandlePlayerAction(AutoBehaviour ab);
	void HandleCollision(AutoBehaviour ab);
    void PositionUpdated(AutoBehaviour ab);
    void RotationUpdated(AutoBehaviour ab);
}
