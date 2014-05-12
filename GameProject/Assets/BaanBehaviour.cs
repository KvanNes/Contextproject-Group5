using UnityEngine;
using System.Collections;

public class BaanBehaviour : MonoBehaviour {
	
    protected const float unit = 750f / 2500f;
    protected const float margin = (150f - 50f) / 2500f;
    private const float m = unit / 2f;
    private static Vector2[] moveCenter(Vector2[] input) {
		Vector2[] res = new Vector2[input.Length];
		for (int i = 0; i < input.Length; i++) {
			res[i] = input[i] - new Vector2(m, m);
		}
		return res;
	}
	protected static Vector2[] pointsAbove, pointsBelow;

	protected void addEdges(Vector2[] pointsAbove, Vector2[] pointsBelow) {
		EdgeCollider2D ec1 = (EdgeCollider2D)gameObject.AddComponent(typeof(EdgeCollider2D));
		EdgeCollider2D ec2 = (EdgeCollider2D)gameObject.AddComponent(typeof(EdgeCollider2D));
		Rigidbody2D rb = (Rigidbody2D)gameObject.AddComponent(typeof(Rigidbody2D));

		ec1.points = moveCenter(pointsAbove);
		ec2.points = moveCenter(pointsBelow);
		ec1.isTrigger = ec2.isTrigger = true;
		rb.gravityScale = 0;
	}

	// Use this for initialization
	protected void Start () {

	}
	
	// Update is called once per frame
    private void Update () {
	    
	}
}
