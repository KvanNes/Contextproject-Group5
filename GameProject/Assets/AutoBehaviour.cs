using UnityEngine;
using System.Collections;
using System;

public class AutoBehaviour : MonoBehaviour {

    private const int queueSize = 2;
    private Queue lastRotations = new Queue(queueSize);
    private Queue lastPositions = new Queue(queueSize);
    private float speed = 0f;
    private float acceleration = 0.02f;
    private float backAcceleration = 0.02f;
    private const float minSpeed = -0.03f;
    private const float maxSpeed = 0.07f;
    private const float maxAcceleration = 0.005f;
    private const float minAcceleration = 0.005f;
    private const float accelerationIncrease = 1f;
    private const float accelerationDecrease = 1f;
	
	private Quaternion copy(Quaternion v) {
		return new Quaternion(v.x, v.y, v.z, v.w);
	}

    private Vector3 copy(Vector3 v) {
		return new Vector3(v.x, v.y, v.z);
	}
	
	private void Start () {
		
	}

    private void Update () {
        if(!networkView.isMine) {
            return;
        }

		float dt = Time.deltaTime;

		if (lastPositions.Count > queueSize) {
			lastPositions.Dequeue();
		}
		if (lastRotations.Count > queueSize) {
			lastRotations.Dequeue();
		}
		lastRotations.Enqueue(copy(transform.rotation));
		lastPositions.Enqueue(copy(transform.position));

		speed = Mathf.Max(Mathf.Min(speed, maxSpeed), minSpeed);
		
		if (Input.GetKey(KeyCode.UpArrow)) {
			acceleration = Mathf.Min(acceleration + accelerationIncrease * dt, maxAcceleration);

			if (minSpeed <= speed && speed < 0) {
				speed = speed + 10 * acceleration * dt;
			} else if (0 <= speed && speed <= maxSpeed) {
				speed = speed + 5 * acceleration * dt;
			}
		} else if (Input.GetKey(KeyCode.DownArrow)) {
			backAcceleration = Mathf.Min(minAcceleration, backAcceleration + accelerationDecrease * dt);
			
			if (minSpeed <= speed && speed < 0){
				speed = speed - 10 * backAcceleration * dt;
			} else if(0 <= speed && speed <= maxSpeed) {
				speed = speed - 20 * backAcceleration * dt;
			}
		} else {
			if (speed > 0) {
				speed = Mathf.Max(0, speed - 5 * acceleration * dt);
			} else if (speed < 0) {
				speed = Mathf.Min(0, speed + 5 * acceleration * dt);
			}
		}

		Debug.Log(speed);

		if (Input.GetKey(KeyCode.LeftArrow)) {
            rotate(1, speed);
		} else if (Input.GetKey(KeyCode.RightArrow)) {
            rotate(-1, speed);
		}

		transform.Translate(speed / 10, 0, 0);

		Camera.main.transform.position = transform.position;
		Camera.main.transform.Translate(new Vector3(0, 0, -2));
	}

    private void rotate(int factor, float speed) {
        float angle = factor * Mathf.Min(3, speed * 50f);
        transform.Rotate(new Vector3(0, 0, angle));
    }

    private void recover() {
		try {
			transform.rotation = (Quaternion) lastRotations.Dequeue();
			transform.position = (Vector3) lastPositions.Dequeue();
		} catch(InvalidOperationException) {
            // Ignore if not possible.
		}
	}
	
    private void OnTriggerEnter2D(Collider2D col) {
		speed = -speed / 1.2f;
		recover();
	}
	
    private void OnTriggerExit2D(Collider2D col) {
        
	}
}
