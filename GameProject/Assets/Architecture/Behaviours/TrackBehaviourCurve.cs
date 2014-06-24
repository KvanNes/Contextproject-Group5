using UnityEngine;
using Utilities;

namespace Behaviours
{
    public class TrackBehaviourCurve : TrackBehaviour
    {

        // Set in Unity Inspector to rotate the collision edges appropriately.
        public int RotateTimes = 0;

        // The inner curve.
        private Vector2[] pointsAbove =
        {
            new Vector2(0, 1 - BorderMargin),
            new Vector2(BorderMargin, 1)
        };

        // The outer curve.
        private Vector2[] _pointsBelow;

        public void Start()
        {
            Vector2[] pointsBelow = new Vector2[GameData.COLLISION_POINTS_AMOUNT];
            for (int i = 0; i < GameData.COLLISION_POINTS_AMOUNT; i++)
            {
                float angleBetweenPoints = -((i) / (float)(GameData.COLLISION_POINTS_AMOUNT - 1) * 90) * Mathf.Deg2Rad;
                pointsBelow[i] = MathUtils.PointOnCircle(new Vector2(0, 1), 1 - BorderMargin, angleBetweenPoints);
            }

            AddEdges(
                MathUtils.RotateVectors(pointsAbove, RotateTimes),
                MathUtils.RotateVectors(pointsBelow, RotateTimes)
            );
        }
    }
}
