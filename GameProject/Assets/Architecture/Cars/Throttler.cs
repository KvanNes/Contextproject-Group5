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

            if (InputWrapper.GetTouchCount() >= 1)
            {
				var pos = InputWrapper.GetTouchPosition(0);
                return pos.x > separatingColumn ? PlayerAction.SpeedUp : PlayerAction.SpeedDown;
            }

            if (InputWrapper.GetKey(KeyCode.DownArrow))
            {
                return PlayerAction.SpeedDown;
            }

            if (InputWrapper.GetKey(KeyCode.UpArrow))
            {
                return PlayerAction.SpeedUp;
            }

            return PlayerAction.None;
        }

		// FIXME
        private void applySpeedUpDown(AutoBehaviour ab, float deltaTime, float accelerationIncrease,
            float backAccelerationFactor,
            float forwardAccelerationFactor)
        {

            ab.Acceleration = ab.Acceleration + accelerationIncrease * deltaTime;
            ab.Acceleration = MathUtils.ForceInInterval(ab.Acceleration, GameData.MIN_ACCELERATION,
                GameData.MAX_ACCELERATION);
				
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
                ab.Speed = 0;
            }
        }

		// FIXME
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

			ab.transform.Translate(ab.Speed * Time.deltaTime * 4f, 0, 0);
			ab.PositionUpdated();
        }
        
		private void CollisionFinish(AutoBehaviour autoBehaviour) {
            foreach (Car car in MainScript.Cars) {
				car.CarObject.NetworkView.RPC ("notifySomeCarHasFinished", RPCMode.All, autoBehaviour.CarNumber);
            }
			autoBehaviour.Speed = 0;
			autoBehaviour.Acceleration = 0;
        }

		private void CollisionMud(AutoBehaviour autoBehaviour) {
			if (autoBehaviour.Speed > GameData.MAX_SPEED * 0.5f)
            {
				autoBehaviour.Speed = 0;
            }
        }

		// FIXME
		private void CollisionEdge(AutoBehaviour autoBehaviour, Collision2D collision) {
            Vector2 normal = collision.contacts[0].normal;
            float a = MathUtils.CalculateAngle(normal) * Mathf.Rad2Deg;
			float b = autoBehaviour.transform.rotation.eulerAngles.z % 360;
            a = (a + 360) % 180;
            b = (b + 360) % 180;
			if (autoBehaviour.Speed < 0) {
                b = 180 - b;  // Backward angle is the opposite.
            }
            float d = Math.Abs(a - b);
			const float minAngleToSlide = 60f;
			if (90 - minAngleToSlide <= d && d <= 90 + minAngleToSlide) {
				if(autoBehaviour.Speed < 0) {
					autoBehaviour.Speed = Mathf.Min(0, autoBehaviour.Speed + GameData.SLIDE_SLOWDOWN);
                } else {
					autoBehaviour.Speed = Mathf.Max(0, autoBehaviour.Speed - GameData.SLIDE_SLOWDOWN);
                }
            } else {
				autoBehaviour.RestoreConfiguration();
				autoBehaviour.gameObject.transform.Translate(Mathf.Sign(autoBehaviour.Speed) * -GameData.BOUNCE_AMOUNT, 0f, 0f);
				autoBehaviour.Speed = -autoBehaviour.Speed * GameData.COLLISION_FACTOR;
				autoBehaviour.PositionUpdated();
            }
        }

        public void HandleCollision(AutoBehaviour ab, Collision2D collision)
        {
            CollisionEdge(ab, collision);
        }

        public void HandleTrigger(AutoBehaviour ab, Collider2D collider)
        {
            if (collider.tag == "Mud")
            {
                CollisionMud(ab);
            }
            else if (collider.tag == "Finish")
            {
                CollisionFinish(ab);
            }
        }

		private void NormalizeSphere(AutoBehaviour autoBehaviour)
        {
			GameObject sphere = autoBehaviour.GetSphere();
			Transform carTransform = autoBehaviour.transform;
            float angle = Mathf.Deg2Rad * carTransform.rotation.eulerAngles.z;
			Vector3 vectorPos = MathUtils.Vector2To3(MathUtils.Rotate(new Vector2(25f / 0.3f, 0f), Vector2.zero, -angle));
			vectorPos.y *= 2f; // Scale ratio of Auto needs to be taken into account here.
			vectorPos.z = -0.3f;
			sphere.transform.localPosition = vectorPos;
        }

		public void MoveCameraWhenPositionUpdated(AutoBehaviour autoBehaviour, bool isSelf)
        {
			NormalizeSphere(autoBehaviour);
        }

		public void MoveCameraWhenRotationUpdated(AutoBehaviour autoBehaviour, bool isSelf)
        {
			NormalizeSphere(autoBehaviour);
        }

        public Vector3 GetLastSentPosition()
        {
            return _lastSentPosition;
        }
    }
}