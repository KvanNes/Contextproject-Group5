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
    
    private bool isColliding = false;

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
            transform.position = copy(lastPositions.Dequeue());
        } catch(InvalidOperationException) {
            // Ignore if not possible.
        }
    }

    private enum speedAction {
        speedUp,
        speedDown,
        noAction
    };

    private speedAction getSpeedAction() {
        int separatingColumn = Screen.width / 2;

        // When mouse is pressed: check whether on left/right half.
        if (Input.mousePresent && Input.GetMouseButton(0)) {
            return Input.mousePosition.x >= separatingColumn
                ? speedAction.speedDown : speedAction.speedUp;
        }

        // When touching with one finger: check whether on left/right half.
        if (Input.touchCount == 1) {
            return Input.GetTouch(0).position.x >= separatingColumn
                ? speedAction.speedDown : speedAction.speedUp;
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
	
	private void Start() {
		
	}

    private void Update() {
        if(!networkView.isMine) {
            // Can only control own car.
            return;
        }

        storeConfiguration();

        // Make sure speed is in constrained interval.
        speed = forceInInterval(speed, minSpeed, maxSpeed);
		
        if (getSpeedAction() == speedAction.speedUp) {
            applySpeedUpDown(Time.deltaTime, accelerationIncrease, 10, 5);
        } else if (getSpeedAction() == speedAction.speedDown) {
            applySpeedUpDown(Time.deltaTime, -accelerationDecrease, 10, 20);
		} else {
			if (speed > 0) {
                applyFriction(Time.deltaTime, -0.05f);
			} else if (speed < 0) {
                applyFriction(Time.deltaTime, 0.05f);
			}
		}

        // Steering.
		if (Input.GetKey(KeyCode.LeftArrow)) {
            rotate(1f, speed);
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            rotate(-1f, speed);
		}

        // Move the car according to current speed.
		transform.Translate(speed * Time.deltaTime * 8f, 0, 0);

        // Move camera along with car.
		Camera.main.transform.position = transform.position;
		Camera.main.transform.Translate(new Vector3(0, 0, -2));
  	}
	
    // Occurs when bumping into something (another car, or a track border).
    private void OnTriggerEnter2D(Collider2D col) {
        if (networkView.isMine) {
            // Go back a little.
            speed = -(speed + Mathf.Sign(speed) * 0.005f) / 1.2f;
            restoreConfiguration();
        }
	}
}
