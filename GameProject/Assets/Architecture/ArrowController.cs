using System;
using UnityEngine;
using System.Collections.Generic;

public class ArrowController : MonoBehaviour {
    private Dictionary<Vector2, Texture2D> Textures;
    private Texture2D TextureStraight, TextureUTurnLeft, TextureUTurnRight, TextureLeftCurve, TextureRightCurve;

    private void AddArrow(float x, float y, Texture2D texture) {
        Textures.Add(new Vector2(x, y), texture);
    }

    public void Start() {
        Textures = new Dictionary<Vector2, Texture2D>();
        TextureStraight = Utils.LoadTexture("ArrowTextureStraight");
        TextureUTurnLeft = Utils.LoadTexture("ArrowTextureUTurnLeft");
        TextureUTurnRight = Utils.LoadTexture("ArrowTextureUTurnRight");
        TextureLeftCurve = Utils.LoadTexture("ArrowTextureCurveLeft");
        TextureRightCurve = Utils.LoadTexture("ArrowTextureCurveRight");

        AddArrow(0.9f, 0.0f, TextureStraight);
        AddArrow(1.2f, 0.0f, TextureStraight);
        AddArrow(1.5f, 0.0f, TextureUTurnLeft);
        AddArrow(1.8f, 0.0f, TextureUTurnLeft);
        AddArrow(1.8f, 0.3f, TextureLeftCurve);
        AddArrow(1.5f, 0.3f, TextureRightCurve);
        AddArrow(1.2f, 0.3f, TextureRightCurve);
        AddArrow(1.2f, 0.6f, TextureStraight);
        AddArrow(1.2f, 0.9f, TextureUTurnLeft);
        AddArrow(0.9f, 0.9f, TextureLeftCurve);
    }

    private static Vector2 GetCenter(Vector2 point) {
        const float half = 0.3f / 2f;
        Vector2 p = point + new Vector2(half, half);
        p.x -= p.x % 0.3f;
        p.y -= p.y % 0.3f;
        return p;
    }

    public void OnGUI() {
        if (MainScript.selfCar == null || MainScript.selfCar.CarObject == null || MainScript.selfPlayer.Role is Driver) {
            return;
        }

        Vector3 p = MainScript.selfCar.CarObject.transform.position;
        Vector2 point = GetCenter(new Vector2(p.x, p.y));
        Texture2D texture = null;

        // Using Utils.getDictionaryValue yields null for some reason.
        foreach(KeyValuePair<Vector2, Texture2D> k in Textures) {
            if(k.Key == point) {
                texture = k.Value;
                break;
            }
        }

        if (texture == null) {
            return;
        }

        GUI.DrawTexture(new Rect(0, 0, 145, 135), texture);
    }
}
