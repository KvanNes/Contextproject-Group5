﻿using UnityEngine;

namespace Behaviours
{
    // This class is attached to the road sprites. It takes care of setting up collision
    // edges.
    public class TrackBehaviour : MonoBehaviour
    {

        // The margin that the collision lines have with respect to the border.
        protected const float Margin = 0.18f;

        // Normalizes the input vectors so that (0, 0) is top left and (1, 1) is bottom
        // right.
        public static Vector2[] Normalize(Vector2[] input)
        {
            Vector2[] res = new Vector2[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                res[i] = input[i];
                res[i] -= new Vector2(0.5f, 0.5f); // Make (0.5, 0.5) the center.
                res[i].x *= 10f; // Take scale ratio into account.
            }
            return res;
        }

        private void AddEdgeCollider(Vector2[] points) {
            EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
            edgeCollider.isTrigger = false;
            edgeCollider.points = Normalize(points);
        }

        // Add the collision edges to this game object.
        protected void AddEdges(Vector2[] pointsAbove, Vector2[] pointsBelow)
        {
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;
            rb.gravityScale = 0;
            AddEdgeCollider(pointsAbove);
            AddEdgeCollider(pointsBelow);
        }

        public virtual void Start()
        {

        }
    }
}