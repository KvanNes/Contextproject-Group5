using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class ControlButtonsGUI : MonoBehaviour {
    private static readonly float BUTTONS_FACTOR = 0.4f;

	Texture2D ThrottlerNormalTexture, ThrottlerPressedTexture;
	Texture2D DriverNormalLeftTexture, DriverPressedLeftTexture;
	Texture2D DriverNormalRightTexture, DriverPressedRightTexture;

	private void Start() {
		ThrottlerNormalTexture = Utils.LoadTexture("gaspedaal-normaal");
        ThrottlerPressedTexture = Utils.LoadTexture("gaspedaal-ingedrukt");
        DriverNormalLeftTexture = Utils.LoadTexture("stuur-links-normaal");
        DriverPressedLeftTexture = Utils.LoadTexture("stuur-links-ingedrukt");
        DriverNormalRightTexture = Utils.LoadTexture("stuur-rechts-normaal");
        DriverPressedRightTexture = Utils.LoadTexture("stuur-rechts-ingedrukt");
	}

    private void DrawControl(Texture2D texture, float left, float top) {
        GUI.DrawTexture(new Rect(left, top, texture.width * BUTTONS_FACTOR, texture.height * BUTTONS_FACTOR), texture);
    }

	private void DrawControls(Texture2D leftTexture, Texture2D rightTexture) {
        DrawControl(leftTexture, 0, Screen.height - leftTexture.height * BUTTONS_FACTOR);
        DrawControl(rightTexture, Screen.width - rightTexture.width * BUTTONS_FACTOR, Screen.height - rightTexture.height * BUTTONS_FACTOR);
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

	private void CreateRestartButton() {
		if(GUI.Button(new Rect(Screen.width / 2 - 75, 10, 150, 25), "Restart Game")) {
			List<Car> cars = MainScript.cars;
			Transform spawnObject = (Transform) GameObject.Find("SpawnPositionBase").GetComponent("Transform");
			foreach(Car car in cars) {
				float y = 0.07f - 0.05f * (car.carNumber - 1);
				Vector3 pos = spawnObject.position + new Vector3(0, y, 0);
				car.CarObject.transform.position = pos;
                Quaternion rot = Quaternion.identity;
                car.CarObject.transform.rotation = rot;
				car.CarObject.speed = 0f;
                car.CarObject.acceleration = 0f;
                car.CarObject.GetSphere().transform.localPosition = new Vector3(43f, 0f, -0.3f);
                car.CarObject.GetSphere().transform.localRotation = Quaternion.identity;
			}
		}
	}

	public void OnGUI() {
		if (MainScript.selfPlayer == null) {
			return;
		} else if (MainScript.selfPlayer.Role is Driver) {
			DrawDriverControls();
		} else if(MainScript.selfPlayer.Role is Throttler) {
			DrawThrottlerControls();
		}
		CreateRestartButton();
	}
}
