using System;
using NetworkManager;
using UnityEngine;
using System.Collections.Generic;

public class SignController : MonoBehaviour
{
    private Dictionary<Vector2, Texture2D> Textures = new Dictionary<Vector2, Texture2D>();

    protected void AddArrow(float x, float y, Texture2D texture)
    {
        Textures.Add(new Vector2(x, y), texture);
    }

    public virtual void Start()
    {

    }

    public virtual bool Enabled()
    {
        return false;
    }

    private static Vector2 GetCenter(Vector2 point)
    {
        const float half = 1f / 2f;
        Vector2 p = point + new Vector2(half, half);
        bool xWasNegative = p.x < 0;
        bool yWasNegative = p.y < 0;
        p.x -= p.x % 1f;
        if (xWasNegative)
        {
            p.x -= 1f;
        }
        p.y -= p.y % 1f;
        if (yWasNegative)
        {
            p.y -= 1f;
        }
        return p;
    }

    public void OnGUI()
    {
        if (MainScript.SelfPlayer == null || MainScript.SelfCar == null || MainScript.SelfCar.CarObject == null)
        {
            return;
        }

        if (!Enabled())
        {
            return;
        }

        Vector3 p = MainScript.SelfCar.CarObject.transform.position;
        Vector2 point = GetCenter(new Vector2(p.x, p.y));
        Texture2D texture = null;

        // Using Utils.getDictionaryValue yields null for some reason.
        foreach (KeyValuePair<Vector2, Texture2D> k in Textures)
        {
            if (k.Key == point)
            {
                texture = k.Value;
                break;
            }
        }

        if (texture == null)
        {
            return;
        }

        GUI.DrawTexture(new Rect(0, 0, 145, 135), texture);
    }
}
