using UnityEngine;
using System.Collections;

// This class is attached to the road sprites. It takes care of setting up collision
// edges.
public class BaanBehaviour : MonoBehaviour {
	
    // Calculated from sprite data in scene editor.
    protected const float spriteSize = 750f / 2500f;

    // The margin that the collision lines have with respect to the border.
    protected const float margin = (150f - 50f) / 2500f;

    // Move the center of an array of vectors so that (0, 0) is the top left.
    public static Vector2[] moveCenter(Vector2[] input) {
		Vector2[] res = new Vector2[input.Length];
        for (int i = 0; i < input.Length; i++) {
            const float halfSize = spriteSize / 2f;
            res[i] = input[i] - new Vector2(halfSize, halfSize);
		}
		return res;
	}

    // These should be set in derived classes.
	protected static Vector2[] pointsAbove, pointsBelow;

    // Add the collision edges to this game object.
	protected void addEdges(Vector2[] pointsAbove, Vector2[] pointsBelow) {
		EdgeCollider2D ec1 = (EdgeCollider2D)gameObject.AddComponent(typeof(EdgeCollider2D));
		EdgeCollider2D ec2 = (EdgeCollider2D)gameObject.AddComponent(typeof(EdgeCollider2D));
		Rigidbody2D rb = (Rigidbody2D)gameObject.AddComponent(typeof(Rigidbody2D));

		ec1.points = moveCenter(pointsAbove);
		ec2.points = moveCenter(pointsBelow);
		ec1.isTrigger = ec2.isTrigger = true;
		rb.gravityScale = 0;
	}

	protected void Start() {

	}
}
