using Behaviours;
using Interfaces;
using UnityEngine;
using Utilities;
using Wrappers;
using NetworkManager;
using GraphicalUI;

namespace Cars
{
    public class Driver : IPlayerRole
    {
        private Quaternion _lastSentRotation;

        public void Initialize()
        {
            Camera.main.orthographicSize = 0.3f;
            MainScript.GUIController.Add(GraphicalUIController.DriverConfiguration);
        }
        
        public void Finished()
        {

        }

        public void Restart()
        {

        }

        public Quaternion GetLastSentRotation()
        {
            return _lastSentRotation;
        }

        public void SendToOther(Car car)
        {
            Quaternion currentRotation = car.CarObject.transform.rotation;
            if (currentRotation != _lastSentRotation)
            {
                _lastSentRotation = MathUtils.Copy(currentRotation);
                car.CarObject.NetworkView.RPC("UpdateRotation", RPCMode.Others, currentRotation, car.CarNumber);
            }
        }

        // When touching with one finger: check whether on left/right half.
        private PlayerAction GetTouchAction()
        {
            if (InputWrapper.GetTouchCount() >= 1)
            {
                Vector2 pos = InputWrapper.GetTouchPosition(0);
                if (pos.x <= GameData.SCREEN_MIDDLE_COLUMN)
                {
                    return PlayerAction.SteerLeft;
                }
                return PlayerAction.SteerRight;
            }
            return PlayerAction.None;
        }

        private PlayerAction GetKeyboardAction()
        {
            if (InputWrapper.GetKey(KeyCode.LeftArrow))
            {
                return PlayerAction.SteerLeft;
            }

            if (InputWrapper.GetKey(KeyCode.RightArrow))
            {
                return PlayerAction.SteerRight;
            }
            return PlayerAction.None;
        }

        public PlayerAction GetPlayerAction()
        {
            if (!MainScript.CountdownController.AllowedToDrive())
            {
                return PlayerAction.None;
            }

            return (GetTouchAction() != PlayerAction.None) ? GetTouchAction() : GetKeyboardAction();
        }

        private void rotateCar(CarBehaviour carObj, float rotateFactor)
        {
            float angle = rotateFactor * MathUtils.ForceInInterval(carObj.Speed * 3f, -3, 3);
            carObj.transform.Rotate(new Vector3(0, 0, angle));
            carObj.RotationUpdated();
        }

        public void HandlePlayerAction(CarBehaviour carObj)
        {
            PlayerAction action = GetPlayerAction();
            if (action == PlayerAction.SteerLeft)
            {
                rotateCar(carObj, Time.deltaTime * GameData.ROTATION_SPEED_FACTOR);
            }
            else if (action == PlayerAction.SteerRight)
            {
                rotateCar(carObj, Time.deltaTime * -GameData.ROTATION_SPEED_FACTOR);
            }
        }

        public void HandleCollision(CarBehaviour carObj, Collision2D collision)
        {

        }

        public void HandleTrigger(CarBehaviour carObj, Collider2D collider)
        {

        }

        public void MoveCameraWithCar(CarBehaviour carObj)
        {
            Camera.main.transform.position = carObj.transform.position;
            Camera.main.transform.Translate(new Vector3(0f, 0f, -1f));
        }

        public void PositionUpdated(CarBehaviour carObj, bool isSelf)
        {
            if (!isSelf)
            {
                return;
            }
            MoveCameraWithCar(carObj);
        }

        public void RotationUpdated(CarBehaviour carObj, bool isSelf)
        {

        }
    }
}
