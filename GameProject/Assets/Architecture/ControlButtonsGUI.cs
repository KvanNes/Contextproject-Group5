using UnityEngine;
using System;

public class ControlButtonsGUI : MonoBehaviour {

    Texture2D ThrottlerNormalTexture, ThrottlerPressedTexture;
    Texture2D DriverNormalLeftTexture, DriverPressedLeftTexture;
    Texture2D DriverNormalRightTexture, DriverPressedRightTexture;

    private static Texture2D LoadTexture(String path) {
        return (Texture2D) Resources.LoadAssetAtPath(path, typeof(Texture2D));
    }

    private void Start() {
        ThrottlerNormalTexture = LoadTexture("Assets/gaspedaal-normaal.png");
        ThrottlerPressedTexture = LoadTexture("Assets/gaspedaal-ingedrukt.png");
        DriverNormalLeftTexture = LoadTexture("Assets/stuur-links-normaal.png");
        DriverPressedLeftTexture = LoadTexture("Assets/stuur-links-ingedrukt.png");
        DriverNormalRightTexture = LoadTexture("Assets/stuur-rechts-normaal.png");
        DriverPressedRightTexture = LoadTexture("Assets/stuur-rechts-ingedrukt.png");
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

