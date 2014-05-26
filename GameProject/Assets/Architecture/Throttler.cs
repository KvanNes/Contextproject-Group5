﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Throttler : Player
{

    public Throttler(Car car)
    {
        this.Car = car;
    }

    // rotation, accelaration, speed, position

    private static Vector3 lastSentPosition;
    public override void SendToOther() {
        Vector3 currentPosition = Car.CarObject.transform.position;
        if (currentPosition != lastSentPosition) {
            lastSentPosition = Utils.copy(currentPosition);
            Car.CarObject.networkView.RPC("UpdatePosition", RPCMode.Others, currentPosition, Car.CarObject.speed, Car.carNumber);
        }
    }

    public override PlayerAction GetPlayerAction() {
        int separatingColumn = Screen.width / 2;
        
        // When touching with one finger: check whether on left/right half.
        if (Input.touchCount >= 1) {
            Vector2 pos = Input.GetTouch(0).position;
            if(pos.x > separatingColumn) {
                return PlayerAction.speedUp;
            } else {
                return PlayerAction.speedDown;
            }
        }
        
        // When down key pressed: speed down.
        if (Input.GetKey(KeyCode.DownArrow)) {
            return PlayerAction.speedDown;
        }
        
        // When up key pressed: speed up.
        if (Input.GetKey(KeyCode.UpArrow)) {
            return PlayerAction.speedUp;
        }
        
        // If none of the above applies, do nothing with respect to throttling.
        return PlayerAction.None;
    }
    
    private void applySpeedUpDown(AutoBehaviour ab, float deltaTime, float accelerationIncrease,
                                  float backAccelerationFactor,
                                  float forwardAccelerationFactor) {
        // Calculate acceleration.
        ab.acceleration = ab.acceleration + accelerationIncrease * deltaTime;
        ab.acceleration = Utils.forceInInterval(ab.acceleration, GameData.minAcceleration,
                                                GameData.maxAcceleration);

        // Calculate speed.
        if (GameData.minSpeed <= ab.speed && ab.speed < 0) {
            ab.speed = ab.speed + backAccelerationFactor * ab.acceleration * deltaTime;
        } else if (0 <= ab.speed && ab.speed <= GameData.maxSpeed) {
            ab.speed = ab.speed + forwardAccelerationFactor * ab.acceleration
                * deltaTime;
        }
    }
    
    private void applyFriction(AutoBehaviour ab, float deltaTime, float delta) {
        float signBefore = Mathf.Sign(ab.speed);
        ab.speed += deltaTime * delta;
        float signAfter = Mathf.Sign(ab.speed);
        
        if (signAfter != signBefore) {
            // Friction can only slow down the car, not moving it in
            // the other direction.
            ab.speed = 0;
        }
    }

    public override void HandlePlayerAction(AutoBehaviour ab) {
        PlayerAction action = GetPlayerAction();
        if (action == PlayerAction.speedUp) {
            applySpeedUpDown(ab, Time.deltaTime, GameData.accelerationIncrease, 10, 5);
        } else if (action == PlayerAction.speedDown) {
            applySpeedUpDown(ab, Time.deltaTime, GameData.accelerationDecrease, 10, 20);
        } else {
            if (ab.speed > 0) {
                applyFriction(ab, Time.deltaTime, -0.05f);
            } else if (ab.speed < 0) {
                applyFriction(ab, Time.deltaTime, 0.05f);
            }
        }
        
        // Move the car according to current speed.
        ab.transform.Translate(ab.speed * Time.deltaTime * 4f, 0, 0);
        ab.PositionUpdated();
    }

    public override void HandleCollision(AutoBehaviour ab) {
        // Go back a little.
        ab.speed = -(ab.speed + Mathf.Sign(ab.speed) * 0.005f) / 1.2f;
        ab.restoreConfiguration();
    }
}
