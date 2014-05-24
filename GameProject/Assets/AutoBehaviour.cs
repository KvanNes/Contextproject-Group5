using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AutoBehaviour : MonoBehaviour {

    // These are used to recover to the last position/rotation when a
    // collision occurs.
    private const int QUEUE_SIZE = 3;
    private Queue<Quaternion> lastRotations = new Queue<Quaternion>(QUEUE_SIZE);
    private Queue<Vector3> lastPositions = new Queue<Vector3>(QUEUE_SIZE);

    // The current speed and acceleration of this car.
    private float speed = 0f;
    private float acceleration = 0.02f;

    // Constants used for physics.
    private const float minSpeed = -0.03f;
    private const float maxSpeed = 0.07f;
    private const float minAcceleration = -0.005f;
    private const float maxAcceleration = 0.005f;
    private const float accelerationIncrease = 1f;
    private const float accelerationDecrease = 1f;

    public int carNumber = -1;

    private float forceInInterval(float x, float min, float max) {
        return Mathf.Min(Mathf.Max(min, x), max);
    }

    // Copies (clones) a Quaternion.
    private Quaternion copy(Quaternion v) {
        return new Quaternion(v.x, v.y, v.z, v.w);
    }

    // Copies (clones) a Vector3.
    private Vector3 copy(Vector3 v) {
        return new Vector3(v.x, v.y, v.z);
    }
    
    // Rotate (steer) this car.
    private void rotate(float factor, float speed) {
        float angle = factor * Mathf.Min(3, speed * 50f);
        transform.Rotate(new Vector3(0, 0, angle));
        RotationUpdated();
    }

    // Store current position and rotation.
    private void storeConfiguration() {
        if(lastRotations.Count >= 3) {
            lastRotations.Dequeue();
        }
        if(lastPositions.Count >= 3) {
            lastPositions.Dequeue();
        }
        lastRotations.Enqueue(copy(transform.rotation));
        lastPositions.Enqueue(copy(transform.position));
    }

    // Restore last position and rotation.
    private void restoreConfiguration() {
        try {
            transform.rotation = copy(lastRotations.Dequeue());
            RotationUpdated();
            transform.position = copy(lastPositions.Dequeue());
            PositionUpdated();
        } catch(InvalidOperationException) {
            // Ignore if not possible.
        }
    }

    private enum speedAction {
        speedUp,
        speedDown,
        noAction
    };

    private enum steerAction {
        steerLeft,
        steerRight,
        noAction
    };

    private speedAction getSpeedAction() {
        if (NetworkManager.type == "steerer") {
            return speedAction.noAction;
        }

        int separatingColumn = Screen.width / 2;

        // When touching with one finger: check whether on left/right half.
        if (Input.touchCount >= 1) {
            for(int i = 0; i < Input.touchCount; i++) {
                Vector2 pos = Input.GetTouch(i).position;
                if(pos.x <= separatingColumn) {
                    return speedAction.speedUp;
                } else {
                    return speedAction.speedDown;
                }
            }
        }

        // When down key pressed: speed down.
        if (Input.GetKey(KeyCode.DownArrow)) {
            return speedAction.speedDown;
        }

        // When up key pressed: speed up.
        if (Input.GetKey(KeyCode.UpArrow)) {
            return speedAction.speedUp;
        }

        // If none of the above applies, do nothing with respect to throttling.
        return speedAction.noAction;
    }

    private steerAction getSteerAction() {
        if (NetworkManager.type == "throttler") {
            return steerAction.noAction;
        }

        int separatingColumn = Screen.width / 2;
        
        // When touching with one finger: check whether on left/right half.
        if (Input.touchCount >= 1) {
            for(int i = 0; i < Input.touchCount; i++) {
                Vector2 pos = Input.GetTouch(i).position;
                if(pos.x <= separatingColumn) {
                    return steerAction.steerLeft;
                } else {
                    return steerAction.steerRight;
                }
            }
        }
        
        // When left key pressed: steer left.
        if (Input.GetKey(KeyCode.LeftArrow)) {
            return steerAction.steerLeft;
        }
        
        // When right key pressed: steer right.
        if (Input.GetKey(KeyCode.RightArrow)) {
            return steerAction.steerRight;
        }
        
        // If none of the above applies, do nothing with respect to steering.
        return steerAction.noAction;
    }

    private void applySpeedUpDown(float deltaTime, float accelerationIncrease,
                                  float backAccelerationFactor,
                                  float forwardAccelerationFactor) {
        // Calculate acceleration.
        acceleration = acceleration + accelerationIncrease * deltaTime;
        acceleration = forceInInterval(acceleration, minAcceleration,
                                       maxAcceleration);
        
        if (minSpeed <= speed && speed < 0) {
            speed = speed + backAccelerationFactor * acceleration * deltaTime;
        } else if (0 <= speed && speed <= maxSpeed) {
            speed = speed + forwardAccelerationFactor * acceleration
                * deltaTime;
        }
    }

    private void applyFriction(float deltaTime, float delta) {
        float signBefore = Mathf.Sign(speed);
        speed += deltaTime * delta;
        float signAfter = Mathf.Sign(speed);

        if (signAfter != signBefore) {
            // Friction can only slow down the car, not moving it in
            // the other direction.
            speed = 0;
        }
    }

    [RPC]
    public void setCarNumber(int number) {
        this.carNumber = number;
    }
    
    [RPC]
    public void requestInitialPositions(NetworkMessageInfo info) {
        foreach (GameObject car in GameObject.FindGameObjectsWithTag("Player")) {
            AutoBehaviour ab = (AutoBehaviour) car.GetComponent(typeof(AutoBehaviour));
            ab.networkView.RPC("UpdatePosition", info.sender, car.transform.position, ab.carNumber);
            ab.networkView.RPC("UpdateRotation", info.sender, car.transform.rotation, ab.carNumber);
        }
    }

    private void Start() {
        networkView.RPC("requestInitialPositions", RPCMode.Server);
    }

    private void Update() {
        if (this.carNumber == -1 || this.carNumber != NetworkManager.car) {
            return;
        }

        storeConfiguration();

        // Make sure speed is in constrained interval.
        speed = forceInInterval(speed, minSpeed, maxSpeed);

        string playerType = NetworkManager.type;
        if(playerType == "steerer") {
            // Steering.
            steerAction action = getSteerAction();
            if(action != steerAction.noAction) {
                if (action == steerAction.steerLeft) {
                    rotate(1f, 0.1f);
                } else if (getSteerAction() == steerAction.steerRight) {
                    rotate(-1f, 0.1f);
                }
                RotationUpdated();
            }
        } else if(playerType == "throttler") {
            // Speeding up or down.
            speedAction action = getSpeedAction();
            if (action == speedAction.speedUp) {
                applySpeedUpDown(Time.deltaTime, accelerationIncrease, 10, 5);
            } else if (action == speedAction.speedDown) {
                applySpeedUpDown(Time.deltaTime, -accelerationDecrease, 10, 20);
            } else {
                if (speed > 0) {
                    applyFriction(Time.deltaTime, -0.05f);
                } else if (speed < 0) {
                    applyFriction(Time.deltaTime, 0.05f);
                }
            }
            
            // Move the car according to current speed.
            transform.Translate(speed * Time.deltaTime * 4f, 0, 0);
            PositionUpdated();
        }
    }

    // Occurs when bumping into something (another car, or a track border).
    private void OnTriggerEnter2D(Collider2D col) {
        if (NetworkManager.type == "throttler") {
            // Go back a little.
            speed = -(speed + Mathf.Sign(speed) * 0.005f) / 1.2f;
            restoreConfiguration();
        }
    }

    int initialized = 0;

    [RPC]
    public void UpdateRotation(Quaternion rot, int carNumber) {
        if (carNumber == this.carNumber) {
            transform.rotation = rot;
            RotationUpdated();
            initialized |= 1;
        }
    }

    [RPC]
    public void UpdatePosition(Vector3 pos, int carNumber) {
        if (carNumber == this.carNumber) {
            transform.position = pos;
            PositionUpdated();
            initialized |= 2;
        }
    }

    public void PositionUpdated() {
        if (carNumber == NetworkManager.car) {
            // Move camera along with car.
            Camera.main.transform.position = transform.position;
            Camera.main.transform.Translate(new Vector3(0, 0, -2));
            
            if (NetworkManager.type == "throttler" && initialized == 3) {
                networkView.RPC("UpdatePosition", RPCMode.Others, transform.position, this.carNumber);
            }
        }
    }

    public void RotationUpdated() {
        if (carNumber == NetworkManager.car && NetworkManager.type == "steerer" && initialized == 3) {
            networkView.RPC("UpdateRotation", RPCMode.Others, transform.rotation, this.carNumber);
        }
    }
}
