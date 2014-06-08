using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AutoBehaviour : MonoBehaviour {
    
    public int carNumber = -1;

    // The current speed and acceleration of this car.
    public float speed = 0f;
    public float acceleration = 0.02f;

    [RPC]
    public void setCarNumber(int number) {
        this.carNumber = number;
        MainScript.cars[number].CarObject = this;
    }
    
    [RPC]
    public void requestInitialPositions(NetworkMessageInfo info) {
        foreach (Car car in MainScript.cars) {
            AutoBehaviour ab = car.CarObject;
            ab.networkView.RPC("UpdatePosition", info.sender, ab.transform.position, ab.speed, ab.carNumber);
            ab.networkView.RPC("UpdateRotation", info.sender, ab.transform.rotation, ab.carNumber);
        }
    }
    
    
    // These are used to recover to the last position/rotation when a
    // collision occurs.
    private const int QUEUE_SIZE = 3;
    private Queue<Quaternion> lastRotations = new Queue<Quaternion>(QUEUE_SIZE);
    private Queue<Vector3> lastPositions = new Queue<Vector3>(QUEUE_SIZE);
    
    // Store current position and rotation.
    private void storeConfiguration() {
        if(lastRotations.Count >= 3) {
            lastRotations.Dequeue();
        }
        if(lastPositions.Count >= 3) {
            lastPositions.Dequeue();
        }
        lastRotations.Enqueue(Utils.copy(transform.rotation));
        lastPositions.Enqueue(Utils.copy(transform.position));
    }
    
    // Restore last position and rotation.
    public void restoreConfiguration() {
        try {
            transform.rotation = Utils.copy(lastRotations.Dequeue());
            RotationUpdated();
            transform.position = Utils.copy(lastPositions.Dequeue());
            PositionUpdated();
        } catch(InvalidOperationException) {
            // Ignore if not possible.
        }
    }

    private void Start() {
        networkView.RPC("requestInitialPositions", RPCMode.Server);
    }

    private void Update() {
        if (!MainScript.selectionIsFinal || MainScript.selfCar.CarObject != this) {
            return;
        }

        storeConfiguration();

        // Make sure speed is in constrained interval.
        speed = Utils.forceInInterval(speed, GameData.MIN_SPEED, GameData.MAX_SPEED);

        if (MainScript.isDebug) {
            new Driver().HandlePlayerAction(this);
            new Throttler().HandlePlayerAction(this);
        } else {
            MainScript.selfPlayer.Role.HandlePlayerAction(this);
        }
    }

    // Occurs when bumping into something (another car, or a track border).
    private void OnTriggerEnter2D(Collider2D collider) {
        if (MainScript.selfType == MainScript.PlayerType.Client) {
            MainScript.selfPlayer.Role.HandleCollision(this, collider);
        }
    }

    public int initialized = 0;
	public static readonly int POSITION_INITIALIZED = 1 << 0;
	public static readonly int ROTATION_INITIALIZED = 1 << 1;

    [RPC]
    public void UpdateRotation(Quaternion rot, int carNumber) {
        if (this.carNumber == carNumber) {
            transform.rotation = rot;
            RotationUpdated();
            initialized |= ROTATION_INITIALIZED;
        }
    }

    [RPC]
    public void UpdatePosition(Vector3 pos, float speed, int carNumber) {
        if (this.carNumber == carNumber) {
            transform.position = pos;
            this.speed = speed;
            PositionUpdated();
            initialized |= POSITION_INITIALIZED;
        }
    }
    
    public void PositionUpdated() {
        if (MainScript.selfType == MainScript.PlayerType.Server || MainScript.selfCar == null) {
            return;
        }
        bool isSelf = MainScript.selfCar.CarObject == this;
        MainScript.selfPlayer.Role.PositionUpdated(this, isSelf);
    }

    public void RotationUpdated() {
        if (MainScript.selfType == MainScript.PlayerType.Server || MainScript.selfCar == null) {
            return;
        }
        bool isSelf = MainScript.selfCar.CarObject == this;
        MainScript.selfPlayer.Role.RotationUpdated(this, isSelf);
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
