using UnityEngine;

namespace Behaviours
{
    // This class is attached to the road sprites. It takes care of setting up collision
    // edges.
    public class TrackBehaviour : MonoBehaviour
    {
        protected const float BorderMargin = 0.18f;

        // Normalizes the input vectors so that (0, 0) is top left and (1, 1) is bottom
        // right. Center is set to (0.5, 0.5) and scale is taken into account.
        public static Vector2[] Normalize(Vector2[] input)
        {
            Vector2[] normalizedVec = new Vector2[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                normalizedVec[i] = input[i];
                normalizedVec[i] -= new Vector2(0.5f, 0.5f);
                normalizedVec[i].x *= 10f;
            }
            return normalizedVec;
        }

        private void AddEdgeCollider(Vector2[] points) {
            EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
            edgeCollider.isTrigger = false;
            edgeCollider.points = Normalize(points);
        }

        // Add the collision edges to this game object.
        protected void AddEdges(Vector2[] pointsAbove, Vector2[] pointsBelow)
        {
            Rigidbody2D rigidbody = gameObject.AddComponent<Rigidbody2D>();
            rigidbody.isKinematic = true;
            rigidbody.gravityScale = 0;
            AddEdgeCollider(pointsAbove);
            AddEdgeCollider(pointsBelow);
        }

		public virtual void Start()	
		{

		}
    }
}