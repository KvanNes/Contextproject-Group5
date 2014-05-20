using UnityEngine;
using System.Collections;

public class OverviewCam : MonoBehaviour {

	public Transform target;
	public const float damping = 6.0f;
	public bool smooth = true;

	void LateUpdate () {
		if(target) {
			if(smooth) {
				Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, damping * Time.deltaTime);
			} else {
				transform.LookAt(target);
			}
			Vector3 vec = new Vector3(target.position.x, target.position.y, target.position.z);
			transform.position = vec;
		}

	}

	// Use this for initialization
	void Start () {
		if(rigidbody)
			rigidbody.freezeRotation = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
