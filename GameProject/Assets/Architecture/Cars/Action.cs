using Behaviours;
using Main;
using UnityEngine;
using Utilities;
using Wrappers;

namespace Cars
{
    public enum PlayerAction
    {
        SpeedUp,
        SpeedDown,
        SteerLeft,
        SteerRight,
        None
    };

    public enum PlayerType
    {
        Driver,
        Throttler
    }

    public static class Action
    {

        // When touching with one finger: check whether on left/right half.
        public static PlayerAction GetTouchAction(PlayerType playerType)
        {
            Vector2 pos = InputWrapper.GetTouchPosition(0);
            switch (playerType)
            {
                case PlayerType.Driver:
                    if (InputWrapper.GetTouchCount() < 1) return PlayerAction.None;
                    return pos.x <= GameData.SCREEN_MIDDLE_COLUMN ? PlayerAction.SteerLeft : PlayerAction.SteerRight;

                case PlayerType.Throttler:
                    // When touching with one finger: check whether on left/right half.
                    if (InputWrapper.GetTouchCount() < 1) return PlayerAction.None;
                    return pos.x > GameData.SCREEN_MIDDLE_COLUMN ? PlayerAction.SpeedUp : PlayerAction.SpeedDown;
            }
            return PlayerAction.None;
        }

        public static PlayerAction GetKeyboardAction(PlayerType playerType)
        {
            switch (playerType)
            {
                case PlayerType.Driver:
                    if (InputWrapper.GetKey(KeyCode.LeftArrow))
                    {
                        return PlayerAction.SteerLeft;
                    }

                    return InputWrapper.GetKey(KeyCode.RightArrow) ? PlayerAction.SteerRight : PlayerAction.None;

                case PlayerType.Throttler:
                    if (InputWrapper.GetKey(KeyCode.DownArrow))
                    {
                        return PlayerAction.SpeedDown;
                    }

                    return InputWrapper.GetKey(KeyCode.UpArrow) ? PlayerAction.SpeedUp : PlayerAction.None;
            }
            return PlayerAction.None;
        }

        public static PlayerAction GetPlayerAction(PlayerType playerType)
        {
            if (!MainScript.CountdownController.AllowedToDrive() || MainScript.SelfCar == null
                || MainScript.SelfCar.CarObject == null || MainScript.SelfCar.CarObject.Finished)
            {
                return PlayerAction.None;
            }
            return (GetTouchAction(playerType) != PlayerAction.None) ? GetTouchAction(playerType) : GetKeyboardAction(playerType);
        }

        public static float GetAccelerationIncrease(PlayerAction action)
        {
            if (action == PlayerAction.SpeedUp)
            {
                return GameData.ACCELERATION_INCREASE;
            }

            return action == PlayerAction.SpeedDown ? GameData.ACCELERATION_DECREASE : 0;
        }

        public static float GetAccelerationFactor(PlayerAction action, CarBehaviour carObj)
        {
            if (action == PlayerAction.SpeedUp)
            {
                return carObj.Speed < 0 ? 10 : 5;
            }

            return action == PlayerAction.SpeedDown ? (carObj.Speed < 0 ? 10 : 50) : 0;
        }

        public static float GetRotationSpeedFactor(PlayerAction action)
        {
            if (action == PlayerAction.SteerLeft)
            {
                return GameData.ROTATION_SPEED_FACTOR;
            }
            return action == PlayerAction.SteerRight ? -GameData.ROTATION_SPEED_FACTOR : 0;
        }
    }
}