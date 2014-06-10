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

	int seconds = 0;
	int minutes = 0;

	private void Start() {
		ThrottlerNormalTexture = TextureUtils.LoadTexture("gaspedaal-normaal");
		ThrottlerPressedTexture = TextureUtils.LoadTexture("gaspedaal-ingedrukt");
		DriverNormalLeftTexture = TextureUtils.LoadTexture("stuur-links-normaal");
		DriverPressedLeftTexture = TextureUtils.LoadTexture("stuur-links-ingedrukt");
		DriverNormalRightTexture = TextureUtils.LoadTexture("stuur-rechts-normaal");
		DriverPressedRightTexture = TextureUtils.LoadTexture("stuur-rechts-ingedrukt");
		InvokeRepeating("Timer", 1.0f, 1.0f);
	}

	private void Timer() {
		if(((int)Network.time % 60) == 0 && !(seconds==0)) {
			this.seconds = 0;
			this.minutes++;
		}

		if(this.minutes > 15) {
			//TODO?
		}
	}

	private void DrawTimer() {
		GUI.Label(new Rect(Screen.width - 50, 0, 50, 30), new GUIContent(this.minutes.ToString("D2") + ":" + this.seconds.ToString("D2")));
	}

	private void Update() {
		Debug.Log("Minutes: " + this.minutes + ", Seconds: " + this.seconds);
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
				float yPos = Server.GetStartingPosition(car.carNumber);
				Vector3 pos = spawnObject.position + new Vector3(0, yPos, 0);
				car.CarObject.transform.position = pos;
				Quaternion rot = Quaternion.identity;
				car.CarObject.transform.rotation = rot;
				car.CarObject.speed = 0f;
				car.CarObject.acceleration = 0f;
				car.CarObject.GetSphere().transform.localPosition = new Vector3(5f / 0.07f, 0f, -0.3f);
				car.CarObject.GetSphere().transform.localRotation = Quaternion.identity;
				car.CarObject.networkView.RPC("UpdatePosition", RPCMode.Others, pos, 0f, car.carNumber - 1);
				car.CarObject.networkView.RPC("UpdateRotation", RPCMode.Others, rot, car.carNumber - 1);
			}
		}
	}

    public void DrawLightControl() {
        if (GUI.Button(new Rect(0, 50, 300, 50), new GUIContent("Toggle light"))) {
            MainScript.networkController.networkView.RPC("ToggleLight", RPCMode.Others);
        }
    }

    public void DrawOverviewControl() {
        if (GUI.Button(new Rect(0, 100, 300, 50), new GUIContent("Toggle overview"))) {
            MainScript.networkController.networkView.RPC("ToggleOverview", RPCMode.Others);
        }
    }

	public void OnGUI() {
		if (MainScript.selfPlayer == null) {
			if(MainScript.selfType == MainScript.PlayerType.Server) {
                DrawLightControl();
                DrawOverviewControl();
                CreateRestartButton();
				DrawTimer();
            }
            return;
		} else if (MainScript.selfPlayer.Role is Driver) {
			DrawDriverControls();
		} else if(MainScript.selfPlayer.Role is Throttler) {
			DrawThrottlerControls();
		}
		CreateRestartButton();
		DrawTimer();
	}
}
