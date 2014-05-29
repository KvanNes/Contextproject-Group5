using UnityEngine;
using System;

public class ControlButtonsGUI : MonoBehaviour {
    
    Texture2D ThrottlerNormalTexture, ThrottlerPressedTexture;
    Texture2D DriverNormalLeftTexture, DriverPressedLeftTexture;
    Texture2D DriverNormalRightTexture, DriverPressedRightTexture;
    
    private static Texture2D LoadTexture(String path) {
        return (Texture2D) Resources.Load<Texture2D>(path);
    }
    
    private void Start() {
        ThrottlerNormalTexture = LoadTexture("gaspedaal-normaal");
        ThrottlerPressedTexture = LoadTexture("gaspedaal-ingedrukt");
        DriverNormalLeftTexture = LoadTexture("stuur-links-normaal");
        DriverPressedLeftTexture = LoadTexture("stuur-links-ingedrukt");
        DriverNormalRightTexture = LoadTexture("stuur-rechts-normaal");
        DriverPressedRightTexture = LoadTexture("stuur-rechts-ingedrukt");
    }
    
    private void DrawControls(Texture2D leftTexture, Texture2D rightTexture) {
        GUI.DrawTexture(new Rect(0, Screen.height - leftTexture.height, leftTexture.width, leftTexture.height), leftTexture);
        GUI.DrawTexture(new Rect(Screen.width - rightTexture.width, Screen.height - rightTexture.height, rightTexture.width, rightTexture.height), rightTexture);
    }
    
    private void DrawDriverControls() {
        PlayerAction currentAction = MainScript.selfPlayer.Role.GetPlayerAction();
        
        DrawControls(
            currentAction == PlayerAction.steerLeft ? DriverPressedLeftTexture : DriverNormalLeftTexture,
            currentAction == PlayerAction.steerRight ? DriverPressedRightTexture : DriverNormalRightTexture
            );
    }
    
    private void DrawThrottlerControls() {
        PlayerAction currentAction = MainScript.selfPlayer.Role.GetPlayerAction();
        
        DrawControls(
            currentAction == PlayerAction.speedDown ? ThrottlerPressedTexture : ThrottlerNormalTexture,
            currentAction == PlayerAction.speedUp ? ThrottlerPressedTexture : ThrottlerNormalTexture
            );
    }
    
    public void OnGUI() {
        if (MainScript.selfPlayer == null) {
            return;
        } else if (MainScript.selfPlayer.Role is Driver) {
            DrawDriverControls();
        } else if(MainScript.selfPlayer.Role is Throttler) {
            DrawThrottlerControls();
        }
    }
}

