using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Driver : PlayerRole {

    public void Initialize() {

    }
    
    private Quaternion lastSentRotation;
    public void SendToOther(Car car) {
        Quaternion currentRotation = car.CarObject.transform.rotation;
        if (currentRotation != lastSentRotation) {
            lastSentRotation = Utils.copy(currentRotation);
            car.CarObject.networkView.RPC("UpdateRotation", RPCMode.Others, currentRotation, car.carNumber);
        }
    }

    public PlayerAction GetPlayerAction() {
        int separatingColumn = Screen.width / 2;
        
        // When touching with one finger: check whether on left/right half.
        if (Input.touchCount >= 1) {
            Vector2 pos = Input.GetTouch(0).position;
            if(pos.x <= separatingColumn) {
                return PlayerAction.steerLeft;
            } else {
                return PlayerAction.steerRight;
            }
        }
        
        // When left key pressed: steer left.
        if (Input.GetKey(KeyCode.LeftArrow)) {
            return PlayerAction.steerLeft;
        }
        
        // When right key pressed: steer right.
        if (Input.GetKey(KeyCode.RightArrow)) {
            return PlayerAction.steerRight;
        }
        
        // If none of the above applies, do nothing with respect to steering.
        return PlayerAction.None;
    }
    
    // Rotate (steer) this car.
    private void rotate(AutoBehaviour ab, float factor) {
        float angle = factor * Mathf.Min(3, ab.speed * 10f);
        ab.transform.Rotate(new Vector3(0, 0, angle));
        ab.RotationUpdated();
    }

    public void HandlePlayerAction(AutoBehaviour ab) {
        PlayerAction action = GetPlayerAction();
        if (action == PlayerAction.steerLeft) {
            rotate(ab, 1f);
        } else if (action == PlayerAction.steerRight) {
            rotate(ab, -1f);
        }
    }

	public void HandleCollision(AutoBehaviour ab) {

	}

    public void PositionUpdated(AutoBehaviour ab, bool isSelf) {
        if (!isSelf) {
            return;
        }
        // Move camera along with car.
        Camera.main.transform.position = ab.transform.position;
        Camera.main.transform.Translate(new Vector3(0, 0, -2));
    }
    
    public void RotationUpdated(AutoBehaviour ab, bool isSelf) {

    }
}
