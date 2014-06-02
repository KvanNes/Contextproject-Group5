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

	private void CreateRestartButton() {
		if(GUI.Button(new Rect(Screen.width / 2 - 75, 10, 150, 25), "Restart Game")) {
			List<Car> cars = MainScript.cars;
			Transform spawnObject = (Transform) GameObject.Find("SpawnPositionBase").GetComponent("Transform");
			foreach(Car car in cars) {
				float y = 0.07f - 0.05f * car.carNumber;
				Vector3 pos = spawnObject.position + new Vector3(0, y, 0);
				car.CarObject.transform.position = pos;
				car.CarObject.transform.rotation = Quaternion.identity;
				car.CarObject.speed = 0f;
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
