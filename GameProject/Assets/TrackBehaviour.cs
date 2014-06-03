using UnityEngine;
using System.Collections;

// This class is attached to the road sprites. It takes care of setting up collision
// edges.
public class TrackBehaviour : MonoBehaviour {

    // The margin that the collision lines have with respect to the border.
    protected const float margin = 0.1f;

    // Normalizes the input vectors so that (0, 0) is top left and (1, 1) is bottom
    // right.
    public static Vector2[] normalize(Vector2[] input) {
		Vector2[] res = new Vector2[input.Length];
        for (int i = 0; i < input.Length; i++) {
            const float unit = 10f;
            res[i] = input[i];
            res[i] -= new Vector2(0.5f, 0.5f); // Make (0.5, 0.5) the center.
            res[i] *= unit;
            res[i].y *= 0.03f; // Take scale ratio into account.
		}
		return res;
	}

    // Add the collision edges to this game object.
	protected void addEdges(Vector2[] pointsAbove, Vector2[] pointsBelow) {
		EdgeCollider2D ec1 = (EdgeCollider2D) gameObject.AddComponent(typeof(EdgeCollider2D));
		EdgeCollider2D ec2 = (EdgeCollider2D) gameObject.AddComponent(typeof(EdgeCollider2D));
		Rigidbody2D rb = (Rigidbody2D) gameObject.AddComponent(typeof(Rigidbody2D));

        ec1.points = normalize(pointsAbove);
        ec2.points = normalize(pointsBelow);
        ec1.isTrigger = true;
        ec2.isTrigger = true;
		rb.gravityScale = 0;
	}
}
