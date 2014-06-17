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

        public void Initialize()
        {
            Camera.main.orthographicSize = 0.3f;
            MainScript.GUIController.Add(GraphicalUIController.DriverConfiguration);
        }

        private Quaternion _lastSentRotation;

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
			
        public PlayerAction GetPlayerAction()
        {
            int separatingColumn = Screen.width / 2;


			if(!MainScript.CountdownController.AllowedToDrive()) {
				return PlayerAction.None;
			}

            if (InputWrapper.GetTouchCount() >= 1)
            {
                Vector2 pos = InputWrapper.GetTouchPosition(0);
                if (pos.x <= separatingColumn)
                {
                    return PlayerAction.SteerLeft;
                }
                return PlayerAction.SteerRight;
            }
				
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
			
        private void rotate(AutoBehaviour ab, float factor)
        {
            float angle = factor * MathUtils.ForceInInterval(ab.Speed * 3f, -3, 3);
            ab.transform.Rotate(new Vector3(0, 0, angle));
            ab.RotationUpdated();
        }

        public void HandlePlayerAction(AutoBehaviour ab)
        {
            PlayerAction action = GetPlayerAction();
            if (action == PlayerAction.SteerLeft)
            {
                rotate(ab, Time.deltaTime * 125f);
            }
            else if (action == PlayerAction.SteerRight)
            {
                rotate(ab, Time.deltaTime * -125f);
            }
        }

        public void HandleCollision(AutoBehaviour ab, Collision2D collision)
        {

        }

        public void HandleTrigger(AutoBehaviour ab, Collider2D collider)
        {

        }

		public void MoveCameraWhenPositionUpdated(AutoBehaviour ab, bool isSelf)
        {
            if (!isSelf)
            {
                return;
            }

            Camera.main.transform.position = ab.transform.position;
            Camera.main.transform.Translate(new Vector3(0f, 0f, -1f));
        }

		public void MoveCameraWhenRotationUpdated(AutoBehaviour ab, bool isSelf)
        {

        }
    }
}
