using UnityEngine;

public class InputWrapper
{
    private static int touchCount = 0;

    private static bool downArrow = false;
    private static bool upArrow = false;
    private static bool leftArrow = false;
    private static bool rightArrow = false;

    private static Touch[] touches = new Touch[10];
    private static Vector2[] touchPositions = new Vector2[10];

    public static void Clear()
    {
        downArrow = false;
        upArrow = false;
        leftArrow = false;
        rightArrow = false;
        touchCount = 0;
        touches = new Touch[10];
        touchPositions = new Vector2[10];
    }

    public static int GetTouchCount()
    {
        return Input.touchCount + touchCount;
    }

    public static void SetTouchCount(int num)
    {
        touchCount = num;
    }

    public static void SetTouch(int index, Vector2 pos)
    {
        if (Input.touches.Length > 0)
        {
            touches = Input.touches;
        }
        else
        {
            touches[index] = new Touch();
            touchPositions[index] = pos;
        }
    }

    public static Touch GetTouch(int index)
    {
        if (Input.touches.Length > 0)
        {
            return Input.GetTouch(index);
        }
        else
        {
            return touches[0];
        }
    }

    public static Vector2 GetTouchPosition(int index)
    {
        if (Input.touches.Length > 0)
        {
            return Input.GetTouch(index).position;
        }
        else
        {
            return touchPositions[0];
        }
    }

    public static void SetKey(KeyCode kc, bool b)
    {
        switch (kc)
        {
            case KeyCode.DownArrow:
                downArrow = b;
                break;
            case KeyCode.UpArrow:
                upArrow = b;
                break;
            case KeyCode.LeftArrow:
                leftArrow = b;
                break;
            case KeyCode.RightArrow:
                rightArrow = b;
                break;
        }
    }

    public static bool GetKey(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.DownArrow:
                return downArrow || Input.GetKey(KeyCode.DownArrow);
            case KeyCode.UpArrow:
                return upArrow || Input.GetKey(KeyCode.UpArrow);
            case KeyCode.LeftArrow:
                return leftArrow || Input.GetKey(KeyCode.LeftArrow);
            case KeyCode.RightArrow:
                return rightArrow || Input.GetKey(KeyCode.RightArrow);
        }
        return false;
    }
}