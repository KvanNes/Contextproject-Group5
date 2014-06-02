using UnityEngine;
using System;

public class ControlButtonsGUI : MonoBehaviour {
    
    Texture2D ThrottlerNormalTexture, ThrottlerPressedTexture;
    Texture2D DriverNormalLeftTexture, DriverPressedLeftTexture;
    Texture2D DriverNormalRightTexture, DriverPressedRightTexture;
    private static readonly float BUTTONS_FACTOR = 0.4f;
    
    private void Start() {
        ThrottlerNormalTexture = Utils.LoadTexture("gaspedaal-normaal");
        ThrottlerPressedTexture = Utils.LoadTexture("gaspedaal-ingedrukt");
        DriverNormalLeftTexture = Utils.LoadTexture("stuur-links-normaal");
        DriverPressedLeftTexture = Utils.LoadTexture("stuur-links-ingedrukt");
        DriverNormalRightTexture = Utils.LoadTexture("stuur-rechts-normaal");
        DriverPressedRightTexture = Utils.LoadTexture("stuur-rechts-ingedrukt");
    }
    
    private void DrawControls(Texture2D leftTexture, Texture2D rightTexture) {
        GUI.DrawTexture(new Rect(0, Screen.height - leftTexture.height * BUTTONS_FACTOR, leftTexture.width * BUTTONS_FACTOR, leftTexture.height * BUTTONS_FACTOR), leftTexture);
        GUI.DrawTexture(new Rect(Screen.width - rightTexture.width * BUTTONS_FACTOR, Screen.height - rightTexture.height * BUTTONS_FACTOR, rightTexture.width * BUTTONS_FACTOR, rightTexture.height * BUTTONS_FACTOR), rightTexture);
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

