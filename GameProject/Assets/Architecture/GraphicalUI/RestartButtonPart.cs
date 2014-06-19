using UnityEngine;
using System.Collections.Generic;
using NetworkManager;
using Cars;

namespace GraphicalUI
{
    public class RestartButtonPart : GraphicalUIPart
    {

        public void ResetCar(Car car, Vector3 pos)
        {
            Quaternion rot = Quaternion.identity;
            car.CarObject.Acceleration = 0f;
            car.CarObject.NetworkView.RPC("UpdatePosition", RPCMode.All, pos, 0f, car.CarNumber - 1);
            car.CarObject.NetworkView.RPC("UpdateRotation", RPCMode.All, rot, car.CarNumber - 1);
        }

        public override void DrawGraphicalUI()
        {

            if (GUI.Button(new Rect(Screen.width / 2 - 75, 10, 150, 25), "Restart Game"))
            {
                List<Car> cars = MainScript.Cars;
                Transform spawnObject = (Transform)GameObject.Find("SpawnPositionBase").GetComponent("Transform");
                foreach (Car car in cars)
                {
                    float yPos = Server.GetStartingPosition(car.CarNumber);
                    Vector3 resetPos = spawnObject.position + new Vector3(0, yPos, 0);
                    car.CarObject.transform.position = resetPos;
                    ResetCar(car, resetPos);
                    car.CarObject.NetworkView.RPC("ResetCar", RPCMode.All);
                    MainScript.NetworkController.NetworkView.RPC("RestartGame", RPCMode.All);
                }
            }
        }
    }
}
