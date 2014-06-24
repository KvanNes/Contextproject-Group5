using Behaviours;
using GraphicalUI;
using Interfaces;
using Main;
using UnityEngine;
using Utilities;

namespace Cars
{
    public class Driver : IPlayerRole
    {
        private Quaternion _lastSentRotation;

        public void Initialize()
        {
            Camera.main.orthographicSize = 0.3f;
            MainScript.GuiController.Add(GraphicalUIController.DriverConfiguration);
        }

        public void Finished() { }
        public void Restart() { }

        public PlayerAction GetPlayerAction()
        {
            return Action.GetPlayerAction(PlayerType.Driver);
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

        private void rotateCar(CarBehaviour carObj, float rotateFactor)
        {
            float angle = rotateFactor * MathUtils.ForceInInterval(carObj.Speed * 3f, -3, 3);
            carObj.transform.Rotate(new Vector3(0, 0, angle));
            carObj.RotationUpdated();
        }

        public void HandlePlayerAction(CarBehaviour carObj)
        {
            PlayerAction action = GetPlayerAction();
            rotateCar(carObj, Time.deltaTime * Action.GetRotationSpeedFactor(action));
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

        public void RotationUpdated(CarBehaviour carObj, bool isSelf) { }
        public void HandleCollision(CarBehaviour carObj, Collision2D collision) { }
        public void HandleTrigger(CarBehaviour carObj, Collider2D collider) { }
    }
}
