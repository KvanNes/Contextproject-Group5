using System;
using Behaviours;
using Controllers;
using GraphicalUI;
using Interfaces;
using Main;
using UnityEngine;
using Utilities;

namespace Cars
{
    public class Throttler : IPlayerRole
    {
        private Vector3 _lastSentPosition;

        public void Initialize()
        {
            RenderSettings.ambientLight = Color.white;
            Camera.main.transform.position = new Vector3(27.5f, 3.5f, -8f);
            Camera.main.orthographicSize = 4.5f;
            MainScript.GuiController.Add(GraphicalUIController.ThrottlerConfiguration);
        }

        public PlayerAction GetPlayerAction()
        {
            return Action.GetPlayerAction(PlayerType.Throttler);
        }

        public void Finished()
        {
            RenderSettings.ambientLight = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }

        public void Restart()
        {
            RenderSettings.ambientLight = Color.white;
        }

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

        private void ApplySpeed(CarBehaviour carObj, float accelerationIncrease, float accelerationFactor)
        {
            // Calculate acceleration.
            carObj.Acceleration = carObj.Acceleration + accelerationIncrease * Time.deltaTime;
            carObj.Acceleration = MathUtils.ForceInInterval(carObj.Acceleration, GameData.MIN_ACCELERATION,
                GameData.MAX_ACCELERATION);

            // Calculate speed.
            if (GameData.MIN_SPEED <= carObj.Speed && carObj.Speed <= GameData.MAX_SPEED)
            {
                carObj.Speed = carObj.Speed + accelerationFactor * carObj.Acceleration
                    * Time.deltaTime;
            }
        }

        private void ApplyFriction(CarBehaviour carObj, float delta)
        {
            int signBefore = (int)Mathf.Sign(carObj.Speed);
            carObj.Speed += Time.deltaTime * delta;
            int signAfter = (int)Mathf.Sign(carObj.Speed);

            if (signAfter != signBefore)
            {
                // Friction can only slow down the car, not moving it in
                // the other direction.
                carObj.Speed = 0;
            }
        }

        private void HandleSpeed(CarBehaviour carObj)
        {
            if (carObj.Speed > 0)
            {
                ApplyFriction(carObj, -GameData.FRICTION_AMOUNT);
            }
            else if (carObj.Speed < 0)
            {
                ApplyFriction(carObj, GameData.FRICTION_AMOUNT);
            }
            else
            {
                carObj.Acceleration = 0f;
            }
        }

        private void ControlPlayerSpeed(CarBehaviour carObj)
        {
            PlayerAction action = GetPlayerAction();

            ApplySpeed(carObj, Action.GetAccelerationIncrease(action), Action.GetAccelerationFactor(action, carObj));
            if (action != PlayerAction.SpeedUp && action != PlayerAction.SpeedDown)
            {
                HandleSpeed(carObj);
            }
        }

        private void MoveCar(CarBehaviour carObj)
        {
            carObj.transform.Translate(carObj.Speed * Time.deltaTime * 4f, 0, 0);
            carObj.PositionUpdated();
        }

        public void HandlePlayerAction(CarBehaviour carObj)
        {
            ControlPlayerSpeed(carObj);
            MoveCar(carObj);
        }

        private void CollisionFinish(CarBehaviour carObj)
        {
            foreach (Car car in MainScript.Cars)
            {
                car.CarObject.NetworkView.RPC("notifyHasFinished", RPCMode.All, carObj.CarNumber, (float)TimeController.GetInstance().GetTime());
            }
        }

        private void CollisionFinishEnd(CarBehaviour carObj)
        {
            carObj.Speed = 0;
            carObj.Acceleration = 0;
        }

        private void CollisionMud(CarBehaviour carObj)
        {
            if (carObj.Speed > GameData.MAX_SPEED * GameData.MUD_SLOWDOWN_FACTOR)
            {
                carObj.Speed = 0;
            }
        }

        private void BounceCollision(CarBehaviour carObj)
        {
            carObj.RestorePosRot();
            carObj.gameObject.transform.Translate(Mathf.Sign(carObj.Speed) * -GameData.BOUNCE_AMOUNT, 0f, 0f);
            carObj.Speed = -carObj.Speed * GameData.COLLISION_FACTOR;
            carObj.PositionUpdated();
        }

        private void HandleCollisionAngle(CarBehaviour carObj, float angle)
        {
            if (90 - GameData.MINIMUM_SLIDE_ANGLE <= angle && angle <= 90 + GameData.MINIMUM_SLIDE_ANGLE)
            {
                if (carObj.Speed < 0)
                {
                    carObj.Speed = Mathf.Min(0, carObj.Speed + GameData.SLIDE_SLOWDOWN);
                }
                else
                {
                    carObj.Speed = Mathf.Max(0, carObj.Speed - GameData.SLIDE_SLOWDOWN);
                }
            }
            else
            {
                BounceCollision(carObj);
            }
        }

        private void CollisionEdge(CarBehaviour carObj, Collision2D collision)
        {
            Vector2 normal = collision.contacts[0].normal;
            float a = MathUtils.CalculateAngle(normal) * Mathf.Rad2Deg;
            float b = carObj.transform.rotation.eulerAngles.z % 360;
            a = (a + 360) % 180;
            b = (b + 360) % 180;
            if (carObj.Speed < 0)
            {
                b = 180 - b;
            }
            float d = Math.Abs(a - b);
            HandleCollisionAngle(carObj, d);
        }

        public void HandleCollision(CarBehaviour carObj, Collision2D collision)
        {
            if (collision.gameObject.tag == GameData.TAG_FINISH)
            {
                CollisionFinishEnd(carObj);
            }
            else
            {
                CollisionEdge(carObj, collision);
            }
        }

        public void HandleTrigger(CarBehaviour carObj, Collider2D collider)
        {
            if (collider.tag == GameData.TAG_FINISH)
            {
                CollisionFinish(carObj);
            }
            else if (collider.tag == GameData.TAG_MUD)
            {
                CollisionMud(carObj);
            }
        }

        // Since the sphere child of a car rotates along, and the midpoint of rotation
        // is not the midpoint of the sphere, the sphere has to be rotated back so that
        // only the position is synchronized with the car.
        private void NormalizeChild(CarBehaviour carObj)
        {
            GameObject child = carObj.GetChild(GameData.NAME_SPHERE);
            Transform carTransform = carObj.transform;
            float angle = Mathf.Deg2Rad * carTransform.rotation.eulerAngles.z;
            Vector3 normalizedVec = MathUtils.Vector2To3(MathUtils.Rotate(new Vector2(25f / 0.2f, 0f), Vector2.zero, -angle));
            normalizedVec.y *= 2f;
            normalizedVec.z = -0.3f;
            child.transform.localPosition = normalizedVec;
        }

        public void PositionUpdated(CarBehaviour carObj, bool isSelf)
        {
            NormalizeChild(carObj);
        }

        public void RotationUpdated(CarBehaviour carObj, bool isSelf)
        {
            NormalizeChild(carObj);
        }

        public Vector3 GetLastSentPosition()
        {
            return _lastSentPosition;
        }
    }
}