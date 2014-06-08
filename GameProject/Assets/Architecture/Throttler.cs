﻿using UnityEngine;

public class Throttler : PlayerRole
{

    public void Initialize()
    {
        RenderSettings.ambientLight = Color.white;
        Camera.main.orthographicSize = 0.7f;
    }

    private Vector3 _lastSentPosition;

    public void SendToOther(Car car)
    {
        Vector3 currentPosition = car.CarObject.transform.position;
        if (currentPosition != _lastSentPosition)
        {
            _lastSentPosition = Utils.Copy(currentPosition);
            car.CarObject.NetworkView.RPC("UpdatePosition", RPCMode.Others, currentPosition, car.CarObject.Speed, car.carNumber);
        }
    }

    public PlayerAction GetPlayerAction()
    {
        int separatingColumn = Screen.width / 2;

        // When touching with one finger: check whether on left/right half.
        if (InputWrapper.GetTouchCount() >= 1)
        {
            var pos = InputWrapper.GetTouchPosition(0); //Input.GetTouch(0).position;
            return pos.x > separatingColumn ? PlayerAction.speedUp : PlayerAction.speedDown;
        }

        // When down key pressed: speed down.
        if (InputWrapper.GetKey(KeyCode.DownArrow))
        {
            return PlayerAction.speedDown;
        }

        // When up key pressed: speed up.
        if (InputWrapper.GetKey(KeyCode.UpArrow))
        {
            return PlayerAction.speedUp;
        }

        // If none of the above applies, do nothing with respect to throttling.
        return PlayerAction.None;
    }

    private void applySpeedUpDown(AutoBehaviour ab, float deltaTime, float accelerationIncrease,
                                  float backAccelerationFactor,
                                  float forwardAccelerationFactor)
    {
        // Calculate acceleration.
        ab.Acceleration = ab.Acceleration + accelerationIncrease * deltaTime;
        ab.Acceleration = Utils.ForceInInterval(ab.Acceleration, GameData.MIN_ACCELERATION,
                                                GameData.MAX_ACCELERATION);

        // Calculate speed.
        if (GameData.MIN_SPEED <= ab.Speed && ab.Speed < 0)
        {
            ab.Speed = ab.Speed + backAccelerationFactor * ab.Acceleration * deltaTime;
        }
        else if (0 <= ab.Speed && ab.Speed <= GameData.MAX_SPEED)
        {
            ab.Speed = ab.Speed + forwardAccelerationFactor * ab.Acceleration
                * deltaTime;
        }
    }

    private void applyFriction(AutoBehaviour ab, float deltaTime, float delta)
    {
        float signBefore = Mathf.Sign(ab.Speed);
        ab.Speed += deltaTime * delta;
        float signAfter = Mathf.Sign(ab.Speed);

        if (signAfter != signBefore)
        {
            // Friction can only slow down the car, not moving it in
            // the other direction.
            ab.Speed = 0;
        }
    }

    public void HandlePlayerAction(AutoBehaviour ab)
    {
        PlayerAction action = GetPlayerAction();
        if (action == PlayerAction.speedUp)
        {
            applySpeedUpDown(ab, Time.deltaTime, GameData.ACCELERATION_INCREASE, 10, 5);
        }
        else if (action == PlayerAction.speedDown)
        {
            applySpeedUpDown(ab, Time.deltaTime, GameData.ACCELERATION_DECREASE, 10, 20);
        }
        else
        {
            if (ab.Speed > 0)
            {
                applyFriction(ab, Time.deltaTime, -GameData.FRICTION_AMOUNT);
            }
            else if (ab.Speed < 0)
            {
                applyFriction(ab, Time.deltaTime, GameData.FRICTION_AMOUNT);
            }
        }

        // Move the car according to current speed.
        ab.transform.Translate(ab.Speed * Time.deltaTime * 4f, 0, 0);
        ab.PositionUpdated();
    }

    public void HandleCollision(AutoBehaviour ab, Collider2D collider)
    {
        if (collider.gameObject.tag == "Mud")
        {
            if (ab.Speed > GameData.MAX_SPEED - 0.01)
            {
                ab.Speed = 0;
            }
        }
        else
        {
            // Go back a little.
            ab.Speed = -(ab.Speed + Mathf.Sign(ab.Speed) * GameData.COLLISION_CONSTANT) * GameData.COLLISION_FACTOR;
            ab.RestoreConfiguration();
        }
    }

    public void PositionUpdated(AutoBehaviour ab, bool isSelf)
    {
        if (!isSelf)
        {
            return;
        }

        Camera.main.transform.position = new Vector3(5.6f, 1f, -8f);
        Camera.main.orthographicSize = 1.4f;
    }

    public void RotationUpdated(AutoBehaviour ab, bool isSelf)
    {
        GameObject sphere = ab.GetSphere();
        Transform carTransform = ab.transform;
        float angle = Mathf.Deg2Rad * carTransform.rotation.eulerAngles.z;
        Vector3 v = Utils.Vector2To3(Utils.Rotate(new Vector2(5f / 0.07f, 0f), Vector2.zero, -angle));
        v.y *= 0.07f / 0.03f;  // Scale ratio of Auto needs to be taken into account here.
        v.z = -0.3f;
        sphere.transform.localPosition = v;
    }

    public Vector3 GetLastSentPosition()
    {
        return _lastSentPosition;
    }
}
