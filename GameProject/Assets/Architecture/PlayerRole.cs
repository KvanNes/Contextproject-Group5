using System;

public interface PlayerRole {
	void SendToOther(Car car);
	PlayerAction GetPlayerAction();
	void HandlePlayerAction(AutoBehaviour ab);
	void HandleCollision(AutoBehaviour ab);
}
