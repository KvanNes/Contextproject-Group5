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
				Vector2 position = InputWrapper.GetTouchPosition(0);
				if (position.x <= separatingColumn)
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
			
		private void rotateCar(AutoBehaviour autoBehaviour, float factor)
        {
			float angle = factor * MathUtils.ForceInInterval(autoBehaviour.Speed * 3f, -3, 3);
			autoBehaviour.transform.Rotate(new Vector3(0, 0, angle));
			autoBehaviour.RotationUpdated();
        }

		public void HandlePlayerAction(AutoBehaviour autoBehaviour)
        {
            PlayerAction action = GetPlayerAction();
            if (action == PlayerAction.SteerLeft)
            {
				rotateCar(autoBehaviour, Time.deltaTime * 125f);
            }
            else if (action == PlayerAction.SteerRight)
            {
				rotateCar(autoBehaviour, Time.deltaTime * -125f);
            }
        }

		public void HandleCollision(AutoBehaviour autoBehaviour, Collision2D collision)
        {

        }

		public void HandleTrigger(AutoBehaviour autoBehaviour, Collider2D collider)
        {

        }

		public void MoveCameraWhenPositionUpdated(AutoBehaviour autoBehaviour, bool isSelf)
        {
            if (!isSelf)
            {
                return;
            }

			Camera.main.transform.position = autoBehaviour.transform.position;
            Camera.main.transform.Translate(new Vector3(0f, 0f, -1f));
        }

		public void MoveCameraWhenRotationUpdated(AutoBehaviour autoBehaviour, bool isSelf)
        {

        }
    }
}
