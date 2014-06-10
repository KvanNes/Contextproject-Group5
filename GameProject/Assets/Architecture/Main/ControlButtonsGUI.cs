using Cars;
using NetworkManager;
using UnityEngine;
using System.Collections.Generic;
using Utilities;

public class ControlButtonsGUI : MonoBehaviour
{

    private const float ButtonsFactor = 0.4f;

    Texture2D _throttlerNormalTexture, _throttlerPressedTexture;
    Texture2D _driverNormalLeftTexture, _driverPressedLeftTexture;
    Texture2D _driverNormalRightTexture, _driverPressedRightTexture;

    public void Start()
    {
		_throttlerNormalTexture = TextureUtils.LoadTexture("gaspedaal-normaal");
		_throttlerPressedTexture = TextureUtils.LoadTexture("gaspedaal-ingedrukt");
		_driverNormalLeftTexture = TextureUtils.LoadTexture("stuur-links-normaal");
		_driverPressedLeftTexture = TextureUtils.LoadTexture("stuur-links-ingedrukt");
		_driverNormalRightTexture = TextureUtils.LoadTexture("stuur-rechts-normaal");
		_driverPressedRightTexture = TextureUtils.LoadTexture("stuur-rechts-ingedrukt");
    }

    private void DrawControl(Texture2D texture, float left, float top)
    {
        GUI.DrawTexture(new Rect(left, top, texture.width * ButtonsFactor, texture.height * ButtonsFactor), texture);
    }

    private void DrawControls(Texture2D leftTexture, Texture2D rightTexture)
    {
        DrawControl(leftTexture, 0, Screen.height - leftTexture.height * ButtonsFactor);
        DrawControl(rightTexture, Screen.width - rightTexture.width * ButtonsFactor, Screen.height - rightTexture.height * ButtonsFactor);
    }

    private void DrawDriverControls()
    {
        PlayerAction currentAction = MainScript.SelfPlayer.Role.GetPlayerAction();

        DrawControls(
            currentAction == PlayerAction.SteerLeft ? _driverPressedLeftTexture : _driverNormalLeftTexture,
            currentAction == PlayerAction.SteerRight ? _driverPressedRightTexture : _driverNormalRightTexture
        );
    }

    private void DrawThrottlerControls()
    {
        PlayerAction currentAction = MainScript.SelfPlayer.Role.GetPlayerAction();

        DrawControls(
            currentAction == PlayerAction.SpeedDown ? _throttlerPressedTexture : _throttlerNormalTexture,
            currentAction == PlayerAction.SpeedUp ? _throttlerPressedTexture : _throttlerNormalTexture
        );
    }

    private void CreateRestartButton()
    {
        if (GUI.Button(new Rect(Screen.width / 2 - 75, 10, 150, 25), "Restart Game"))
        {
            List<Car> cars = MainScript.Cars;
            Transform spawnObject = (Transform)GameObject.Find("SpawnPositionBase").GetComponent("Transform");
            foreach (Car car in cars)
            {
                float y = Server.GetStartingPosition(car.CarNumber);
                Vector3 pos = spawnObject.position + new Vector3(0, y, 0);
                car.CarObject.transform.position = pos;
                Quaternion rot = Quaternion.identity;
                car.CarObject.transform.rotation = rot;
                car.CarObject.Speed = 0f;
                car.CarObject.Acceleration = 0f;
                car.CarObject.GetSphere().transform.localPosition = new Vector3(5f / 0.07f, 0f, -0.3f);
                car.CarObject.GetSphere().transform.localRotation = Quaternion.identity;
                car.CarObject.networkView.RPC("UpdatePosition", RPCMode.Others, pos, 0f, car.CarNumber - 1);
                car.CarObject.networkView.RPC("UpdateRotation", RPCMode.Others, rot, car.CarNumber - 1);
            }
        }
    }

    public void DrawLightControl()
    {
        if (GUI.Button(new Rect(0, 50, 300, 50), new GUIContent("Toggle light")))
        {
            MainScript.NetworkController.networkView.RPC("ToggleLight", RPCMode.Others);
        }
    }

    public void DrawOverviewControl()
    {
        if (GUI.Button(new Rect(0, 100, 300, 50), new GUIContent("Toggle overview")))
        {
            MainScript.NetworkController.networkView.RPC("ToggleOverview", RPCMode.Others);
        }
    }

    public void OnGUI()
    {
        if (MainScript.SelfPlayer == null)
        {
            if (MainScript.SelfType == MainScript.PlayerType.Server)
            {
                DrawLightControl();
                DrawOverviewControl();
                CreateRestartButton();
            }
            return;
        }

        if (MainScript.SelfPlayer.Role is Driver)
        {
            DrawDriverControls();
        }
        else if (MainScript.SelfPlayer.Role is Throttler)
        {
            DrawThrottlerControls();
        }
        CreateRestartButton();
    }
}
