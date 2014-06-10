using UnityEngine;

namespace Wrappers
{
    public class InputWrapper
    {
        private static int _touchCount;

        private static bool _downArrow;
        private static bool _upArrow;
        private static bool _leftArrow;
        private static bool _rightArrow;

        private static Touch[] _touches = new Touch[10];
        private static Vector2[] _touchPositions = new Vector2[10];

        public static void Clear()
        {
            _downArrow = false;
            _upArrow = false;
            _leftArrow = false;
            _rightArrow = false;
            _touchCount = 0;
            _touches = new Touch[10];
            _touchPositions = new Vector2[10];
        }

        public static int GetTouchCount()
        {
            return Input.touchCount + _touchCount;
        }

        public static void SetTouchCount(int num)
        {
            _touchCount = num;
        }

        public static void SetTouch(int index, Vector2 pos)
        {
            if (Input.touches.Length > 0)
            {
                _touches = Input.touches;
            }
            else
            {
                _touches[index] = new Touch();
                _touchPositions[index] = pos;
            }
        }

        public static Touch GetTouch(int index)
        {
            if (Input.touches.Length > 0)
            {
                return Input.GetTouch(index);
            }
            return _touches[0];
        }

        public static Vector2 GetTouchPosition(int index)
        {
            if (Input.touches.Length > 0)
            {
                return Input.GetTouch(index).position;
            }
            return _touchPositions[0];
        }

        public static void SetKey(KeyCode kc, bool b)
        {
            switch (kc)
            {
                case KeyCode.DownArrow:
                    _downArrow = b;
                    break;
                case KeyCode.UpArrow:
                    _upArrow = b;
                    break;
                case KeyCode.LeftArrow:
                    _leftArrow = b;
                    break;
                case KeyCode.RightArrow:
                    _rightArrow = b;
                    break;
            }
        }

        public static bool GetKey(KeyCode keyCode)
        {
            switch (keyCode)
            {
                case KeyCode.DownArrow:
                    return _downArrow || Input.GetKey(KeyCode.DownArrow);
                case KeyCode.UpArrow:
                    return _upArrow || Input.GetKey(KeyCode.UpArrow);
                case KeyCode.LeftArrow:
                    return _leftArrow || Input.GetKey(KeyCode.LeftArrow);
                case KeyCode.RightArrow:
                    return _rightArrow || Input.GetKey(KeyCode.RightArrow);
            }
            return false;
        }
    }
}