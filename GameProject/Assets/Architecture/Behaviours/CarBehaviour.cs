using Cars;
using Interfaces;
using NetworkManager;
using UnityEngine;
using System.Collections.Generic;
using System;
using Utilities;
using Wrappers;

namespace Behaviours
{

	//TODO PIM
    public class CarBehaviour : MonoBehaviour
    {

        public int CarNumber = -1;
		public enum FinishedState { won, lost, inprogress }
		public FinishedState state = FinishedState.inprogress;

        // The current speed and acceleration of this car.
        public float Speed = 0f;
        public float Acceleration = 0.02f;

        public INetworkView NetworkView;

        [RPC]
        public void setCarNumber(int number)
        {
            CarNumber = number;
            MainScript.Cars[number].CarObject = this;
        }
		
        [RPC]
        public void requestInitialPositions(NetworkMessageInfo info)
        {
            foreach (Car car in MainScript.Cars)
            {
                CarBehaviour carObj = car.CarObject;
                carObj.NetworkView.RPC("UpdatePosition", info.sender, carObj.transform.position, carObj.Speed, carObj.CarNumber);
                carObj.NetworkView.RPC("UpdateRotation", info.sender, carObj.transform.rotation, carObj.CarNumber);
            }
        }

		[RPC]
		public void notifyHasFinished(int CarNumber) {
			if (CarNumber == this.CarNumber) {
				state = FinishedState.won;
			} else
				state = FinishedState.lost;
		}


        // These are used to recover to the last position/rotation when a
        // collision occurs.
        private const int QueueSize = 3;
        private Queue<Quaternion> _lastRotations = new Queue<Quaternion>(QueueSize);
        private Queue<Vector3> _lastPositions = new Queue<Vector3>(QueueSize);

        public void StorePosRot()
        {
            if (_lastRotations.Count >= 3)
            {
                _lastRotations.Dequeue();
            }
            if (_lastPositions.Count >= 3)
            {
                _lastPositions.Dequeue();
            }
            _lastRotations.Enqueue(MathUtils.Copy(transform.rotation));
            _lastPositions.Enqueue(MathUtils.Copy(transform.position));
        }

        public void RestorePosRot()
        {
            try
            {
                transform.rotation = MathUtils.Copy(_lastRotations.Dequeue());
                RotationUpdated();
                transform.position = MathUtils.Copy(_lastPositions.Dequeue());
                PositionUpdated();
            }
            catch (InvalidOperationException)
            {
                // Ignore if not possible.
            }
        }

        private void SetNetworkView()
        {
            NetworkView = new NetworkViewWrapper();
            NetworkView.SetNativeNetworkView(GetComponent<NetworkView>());
        }

        public void Start()
        {
            SetNetworkView();
            NetworkView.RPC("requestInitialPositions", RPCMode.Server);
        }

        public void Update()
        {
            if (!MainScript.SelectionIsFinal || MainScript.SelfCar.CarObject != this)
            {
                return;
            }

            StorePosRot();

            // Make sure speed is in constrained interval.
            Speed = MathUtils.ForceInInterval(Speed, GameData.MIN_SPEED, GameData.MAX_SPEED);
            
			MainScript.SelfPlayer.Role.HandlePlayerAction(this);
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (MainScript.SelfType == MainScript.PlayerType.Client)
            {
                MainScript.SelfPlayer.Role.HandleCollision(this, collision);
            }
        }

        public int Initialized = 0;
        public static readonly int PositionInitialized = 1 << 0;
        public static readonly int RotationInitialized = 1 << 1;

        [RPC]
        public void ResetCar() {
			state = FinishedState.inprogress;
        }

        [RPC]
        public void UpdateRotation(Quaternion rot, int carNumber)
        {
            if (CarNumber != carNumber) return;
            transform.rotation = rot;
            RotationUpdated();
            Initialized |= RotationInitialized;
        }

        [RPC]
        public void UpdatePosition(Vector3 pos, float speed, int carNumber)
        {
            if (CarNumber != carNumber) return;
            transform.position = pos;
            Speed = speed;
            PositionUpdated();
            Initialized |= PositionInitialized;
        }

        public void PositionUpdated()
        {
            if (MainScript.SelfType == MainScript.PlayerType.Server || MainScript.SelfCar == null)
            {
                return;
            }
            bool isSelf = MainScript.SelfCar.CarObject == this;
            MainScript.SelfPlayer.Role.PositionUpdated(this, isSelf);
        }

        public void RotationUpdated()
        {
            if (MainScript.SelfType == MainScript.PlayerType.Server || MainScript.SelfCar == null)
            {
                return;
            }
            bool isSelf = MainScript.SelfCar.CarObject == this;
            MainScript.SelfPlayer.Role.RotationUpdated(this, isSelf);
        }

        public Queue<Quaternion> GetLastRotations()
        {
            return _lastRotations;
        }

        public void ResetLastRotations()
        {
            _lastRotations.Clear();
        }

        public Queue<Vector3> GetLastPositions()
        {
            return _lastPositions;
        }

        public void ResetLastPositions()
        {
            _lastPositions.Clear();
        }

        public GameObject GetChild(string child)
        {
            //Based on: http://answers.unity3d.com/questions/183649/how-to-find-a-child-gameobject-by-name.html
            Component[] components = transform.GetComponentsInChildren<Component>();
            foreach (Component component in components)
            {
                if (component.name == "Sphere")
                {
                    return component.gameObject;
                }
            }
            return null;
        }
    }
}