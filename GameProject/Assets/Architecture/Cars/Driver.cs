using Behaviours;
using Mock;
using UnityEngine;
using Utilities;

namespace Cars
{
    public class Driver : IPlayerRole
    {

        public void Initialize()
        {
            Camera.main.orthographicSize = 0.1f;
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

            // When touching with one finger: check whether on left/right half.
            if (InputWrapper.GetTouchCount() >= 1)
            {
                Vector2 pos = InputWrapper.GetTouchPosition(0); // Input.GetTouch(0).position;
                if (pos.x <= separatingColumn)
                {
                    return PlayerAction.SteerLeft;
                }
                return PlayerAction.SteerRight;
            }

            // When left key pressed: steer left.
            if (InputWrapper.GetKey(KeyCode.LeftArrow))
            {
                return PlayerAction.SteerLeft;
            }

            // When right key pressed: steer right.
            if (InputWrapper.GetKey(KeyCode.RightArrow))
            {
                return PlayerAction.SteerRight;
            }

            // If none of the above applies, do nothing with respect to steering.
            return PlayerAction.None;
        }

        // Rotate (steer) this car.
        private void rotate(AutoBehaviour ab, float factor)
        {
            float angle = factor * Mathf.Min(3, ab.Speed * 10f);
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

        public void HandleCollision(AutoBehaviour ab, Collider2D collider)
        {

        }

        public void PositionUpdated(AutoBehaviour ab, bool isSelf)
        {
            if (!isSelf)
            {
                return;
            }

            // Move camera along with car.
            Camera.main.transform.position = ab.transform.position;
            Camera.main.transform.Translate(new Vector3(0f, 0f, -1f));
        }

        public void RotationUpdated(AutoBehaviour ab, bool isSelf)
        {

        }
    }
}
