using UnityEngine;
using System.Collections.Generic;
using NetworkManager;
using Cars;

namespace GraphicalUI {
    public class RestartButtonPart : GraphicalUIPart {

        private void ResetCar(Car car, Vector3 pos) {
            Quaternion rot = Quaternion.identity;
            car.CarObject.transform.rotation = rot;
            car.CarObject.Speed = 0f;
            car.CarObject.Acceleration = 0f;
            car.CarObject.GetSphere().transform.localPosition = new Vector3(25f / 0.3f, 0f, -0.3f);
            car.CarObject.GetSphere().transform.localRotation = Quaternion.identity;
            car.CarObject.networkView.RPC("UpdatePosition", RPCMode.Others, pos, 0f, car.CarNumber - 1);
            car.CarObject.networkView.RPC("UpdateRotation", RPCMode.Others, rot, car.CarNumber - 1);
            car.CarObject.networkView.RPC("ResetCar", RPCMode.Others);
        }

        public override void DrawGraphicalUI() {

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
                    Car.hasFinished = false;
                }
            }
        }
    }
}
