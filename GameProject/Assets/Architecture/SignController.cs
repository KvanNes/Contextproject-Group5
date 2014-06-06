using System;
using UnityEngine;
using System.Collections.Generic;

public class SignController : MonoBehaviour {
    private Dictionary<Vector2, Texture2D> Textures = new Dictionary<Vector2, Texture2D>();
    
    protected void AddArrow(float x, float y, Texture2D texture) {
        Textures.Add(new Vector2(x * 0.3f, y * 0.3f), texture);
    }

    public virtual void Start() {

    }

    public virtual bool Enabled() {
        return false;
    }
    
    private static Vector2 GetCenter(Vector2 point) {
        const float half = 0.3f / 2f;
        Vector2 p = point + new Vector2(half, half);
        bool xWasNegative = p.x < 0;
        bool yWasNegative = p.y < 0;
        p.x -= p.x % 0.3f;
        if (xWasNegative) {
            p.x -= 0.3f;
        }
        p.y -= p.y % 0.3f;
        if (yWasNegative) {
            p.y -= 0.3f;
        }
        return p;
    }
    
    public void OnGUI() {
        if(MainScript.selfPlayer == null || MainScript.selfCar == null || MainScript.selfCar.CarObject == null) {
            return;
        }

        if (!Enabled()) {
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
