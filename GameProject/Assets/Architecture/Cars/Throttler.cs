using System;
using Behaviours;
using Interfaces;
using UnityEngine;
using Utilities;
using Wrappers;
using NetworkManager;
using GraphicalUI;

namespace Cars
{
    public class Throttler : IPlayerRole
    {
        public void Initialize()
        {
            RenderSettings.ambientLight = Color.white;
            Camera.main.transform.position = new Vector3(27f, 3.5f, -8f);
            Camera.main.orthographicSize = 4.5f;
            MainScript.GUIController.Add(GraphicalUIController.ThrottlerConfiguration);
        }

        private Vector3 _lastSentPosition;

        public void SendToOther(Car car)
        {
            Vector3 currentPosition = car.CarObject.transform.position;
            if (currentPosition != _lastSentPosition)
            {
                _lastSentPosition = MathUtils.Copy(currentPosition);
                car.CarObject.NetworkView.RPC("UpdatePosition", RPCMode.Others, currentPosition, car.CarObject.Speed,
                    car.CarNumber);
            }
        }

        public PlayerAction GetPlayerAction()
        {
            int separatingColumn = Screen.width / 2;

            if(!MainScript.CountdownController.AllowedToDrive()) {
				return PlayerAction.None;
			}
            // When touching with one finger: check whether on left/right half.
            if (InputWrapper.GetTouchCount() >= 1)
            {
                var pos = InputWrapper.GetTouchPosition(0); //Input.GetTouch(0).position;
                return pos.x > separatingColumn ? PlayerAction.SpeedUp : PlayerAction.SpeedDown;
            }

            // When down key pressed: speed down.
            if (InputWrapper.GetKey(KeyCode.DownArrow))
            {
                return PlayerAction.SpeedDown;
            }

            // When up key pressed: speed up.
            if (InputWrapper.GetKey(KeyCode.UpArrow))
            {
                return PlayerAction.SpeedUp;
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
            ab.Acceleration = MathUtils.ForceInInterval(ab.Acceleration, GameData.MIN_ACCELERATION,
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

            if (Math.Abs(signAfter - signBefore) > GameData.TOLERANCE)
            {
                // Friction can only slow down the car, not moving it in
                // the other direction.
                ab.Speed = 0;
            }
        }

        public void HandlePlayerAction(AutoBehaviour ab)
        {
            PlayerAction action = GetPlayerAction();
			if(ab.state == Behaviours.AutoBehaviour.FinishedState.won) {
				return;
			}

			if (action == PlayerAction.SpeedUp)
			{
				applySpeedUpDown(ab, Time.deltaTime, GameData.ACCELERATION_INCREASE, 10, 5);
			}
			else if (action == PlayerAction.SpeedDown)
			{
				applySpeedUpDown(ab, Time.deltaTime, GameData.ACCELERATION_DECREASE, 10, 50);
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
                else
                {
                    ab.Acceleration = 0f;
                }
			}
			
			// Move the car according to current speed.
			ab.transform.Translate(ab.Speed * Time.deltaTime * 4f, 0, 0);
			ab.PositionUpdated();
        }
        
        private void CollisionFinish(AutoBehaviour ab) {
            foreach (Car car in MainScript.Cars) {
                car.CarObject.NetworkView.RPC ("notifyHasFinished", RPCMode.All, ab.CarNumber);
            }
            ab.Speed = 0;
            ab.Acceleration = 0;
        }

        private void CollisionMud(AutoBehaviour ab) {
            if (ab.Speed > GameData.MAX_SPEED * 0.5f)
            {
                ab.Speed = 0;
            }
        }

        private void CollisionEdge(AutoBehaviour ab, Collision2D collision) {
            Vector2 normal = collision.contacts[0].normal;
            float a = MathUtils.CalculateAngle(normal) * Mathf.Rad2Deg;
            float b = ab.transform.rotation.eulerAngles.z % 360;
            a = (a + 360) % 180;
            b = (b + 360) % 180;
            if (ab.Speed < 0) {
                b = 180 - b;  // Backward angle is the opposite.
            }
            float d = Math.Abs(a - b);
            const float angle = 60f;  // This angle is approximately when sliding happens.
            if (90 - angle <= d && d <= 90 + angle) {
                // Slide, handled by Unity
                if(ab.Speed < 0) {
                    ab.Speed = Mathf.Min(0, ab.Speed + GameData.SLIDE_SLOWDOWN);
                } else {
                    ab.Speed = Mathf.Max(0, ab.Speed - GameData.SLIDE_SLOWDOWN);
                }
            } else {
                // Go back a little.
                ab.RestoreConfiguration();
                ab.gameObject.transform.Translate(Mathf.Sign(ab.Speed) * -GameData.BOUNCE_AMOUNT, 0f, 0f);
                ab.Speed = -ab.Speed * GameData.COLLISION_FACTOR;
                ab.PositionUpdated();
            }
        }

        public void HandleCollision(AutoBehaviour ab, Collision2D collision)
        {
            if (collision.gameObject.tag == "Finish")
            {
                CollisionFinish(ab);
            }
            else
            {
                CollisionEdge(ab, collision);
            }
        }

        public void HandleTrigger(AutoBehaviour ab, Collider2D collider)
        {
            if (collider.tag == "Mud")
            {
                CollisionMud(ab);
            }
        }

        private void NormalizeSphere(AutoBehaviour ab)
        {
            GameObject sphere = ab.GetSphere();
            Transform carTransform = ab.transform;
            float angle = Mathf.Deg2Rad * carTransform.rotation.eulerAngles.z;
            Vector3 v = MathUtils.Vector2To3(MathUtils.Rotate(new Vector2(25f / 0.3f, 0f), Vector2.zero, -angle));
            v.y *= 2f; // Scale ratio of Auto needs to be taken into account here.
            v.z = -0.3f;
            sphere.transform.localPosition = v;
        }

        public void PositionUpdated(AutoBehaviour ab, bool isSelf)
        {
            NormalizeSphere(ab);
        }

        public void RotationUpdated(AutoBehaviour ab, bool isSelf)
        {
            NormalizeSphere(ab);
        }

        public Vector3 GetLastSentPosition()
        {
            return _lastSentPosition;
        }
    }
}