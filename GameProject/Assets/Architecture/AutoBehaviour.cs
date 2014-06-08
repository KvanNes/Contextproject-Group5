using UnityEngine;
using System.Collections.Generic;
using System;

public class AutoBehaviour : MonoBehaviour {
    
    public int CarNumber = -1;

    // The current speed and acceleration of this car.
    public float Speed = 0f;
    public float Acceleration = 0.02f;

    public INetworkView NetworkView;

    [RPC]
    public void setCarNumber(int number) {
        CarNumber = number;
        MainScript.Cars[number].CarObject = this;
    }
    
    [RPC]
    public void requestInitialPositions(NetworkMessageInfo info) {
        foreach (Car car in MainScript.Cars) {
            AutoBehaviour ab = car.CarObject;
            ab.NetworkView.RPC("UpdatePosition", info.sender, ab.transform.position, ab.Speed, ab.CarNumber);
            ab.NetworkView.RPC("UpdateRotation", info.sender, ab.transform.rotation, ab.CarNumber);
        }
    }
    
    
    // These are used to recover to the last position/rotation when a
    // collision occurs.
    private const int QUEUE_SIZE = 3;
    private Queue<Quaternion> _lastRotations = new Queue<Quaternion>(QUEUE_SIZE);
    private Queue<Vector3> _lastPositions = new Queue<Vector3>(QUEUE_SIZE);
    
    // Store current position and rotation.
    public void StoreConfiguration() {
        if(_lastRotations.Count >= 3) {
            _lastRotations.Dequeue();
        }
        if(_lastPositions.Count >= 3) {
            _lastPositions.Dequeue();
        }
        _lastRotations.Enqueue(Utils.Copy(transform.rotation));
        _lastPositions.Enqueue(Utils.Copy(transform.position));
    }
    
    // Restore last position and rotation.
    public void RestoreConfiguration() {
        try {
            transform.rotation = Utils.Copy(_lastRotations.Dequeue());
            RotationUpdated();
            transform.position = Utils.Copy(_lastPositions.Dequeue());
            PositionUpdated();
        } catch(InvalidOperationException) {
            // Ignore if not possible.
        }
    }

    private void SetNetworkView()
    {
        NetworkView = new NetworkViewWrapper();
        NetworkView.SetNativeNetworkView(GetComponent<NetworkView>());
    }

    private void Start() {
        SetNetworkView();

        NetworkView.RPC("requestInitialPositions", RPCMode.Server);
    }

    private void Update() {
        if (!MainScript.selectionIsFinal || MainScript.SelfCar.CarObject != this) {
            return;
        }

        StoreConfiguration();

        // Make sure speed is in constrained interval.
        Speed = Utils.ForceInInterval(Speed, GameData.MIN_SPEED, GameData.MAX_SPEED);

        if (MainScript.isDebug) {
            new Driver().HandlePlayerAction(this);
            new Throttler().HandlePlayerAction(this);
        } else {
            MainScript.SelfPlayer.Role.HandlePlayerAction(this);
        }
    }

    // Occurs when bumping into something (another car, or a track border).
    private void OnTriggerEnter2D(Collider2D collider) {
        if (MainScript.selfType == MainScript.PlayerType.Client) {
            MainScript.SelfPlayer.Role.HandleCollision(this, collider);
        }
    }

    public int initialized = 0;
	public static readonly int POSITION_INITIALIZED = 1 << 0;
	public static readonly int ROTATION_INITIALIZED = 1 << 1;

    [RPC]
    public void UpdateRotation(Quaternion rot, int carNumber) {
        if (this.CarNumber == carNumber) {
            transform.rotation = rot;
            RotationUpdated();
            initialized |= ROTATION_INITIALIZED;
        }
    }

    [RPC]
    public void UpdatePosition(Vector3 pos, float speed, int carNumber) {
        if (this.CarNumber == carNumber) {
            transform.position = pos;
            this.Speed = speed;
            PositionUpdated();
            initialized |= POSITION_INITIALIZED;
        }
    }
    
    public void PositionUpdated() {
        if (MainScript.selfType == MainScript.PlayerType.Server || MainScript.SelfCar == null) {
            return;
        }
        bool isSelf = MainScript.SelfCar.CarObject == this;
        MainScript.SelfPlayer.Role.PositionUpdated(this, isSelf);
    }

    public void RotationUpdated() {
        if (MainScript.selfType == MainScript.PlayerType.Server || MainScript.SelfCar == null) {
            return;
        }
        bool isSelf = MainScript.SelfCar.CarObject == this;
        MainScript.SelfPlayer.Role.RotationUpdated(this, isSelf);
    }

    public void AddToQueues(int count)
    {
        for (var i = 0; i < count; i++)
        {
            _lastRotations.Enqueue(Utils.Copy(transform.rotation));
            _lastPositions.Enqueue(Utils.Copy(transform.position));
        }
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

    public GameObject GetSphere() {
        // Gebaseerd op: http://answers.unity3d.com/questions/183649/how-to-find-a-child-gameobject-by-name.html
        Component[] components = transform.GetComponentsInChildren<Component>();
        foreach(Component component in components) {
            if(component.name == "Sphere") {
                return component.gameObject;
            }
        }
        return null;
    }
}
