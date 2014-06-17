using UnityEngine;
using Utilities;

namespace Behaviours
{
    public class TrackBehaviourCurve : TrackBehaviour
    {

        private const float CurveFactor = 0.7f;
        // Set in Editor to rotate the collision edges appropriately.
        public int RotateTimes = 0;

        // The inner curve.
        private Vector2[] pointsAbove =
        {
            new Vector2(0, 1 - Margin),
            new Vector2(Margin, 1)
        };

        // The outer curve.
        private Vector2[] pointsBelow;

        public override void Start()
        {
            int POINTS_AMOUNT = 12;
            Vector2[] pointsBelow = new Vector2[POINTS_AMOUNT];
            for(int i = 0; i < POINTS_AMOUNT; i++) {
                float angle = -((float)(i) / (float)(POINTS_AMOUNT - 1) * 90) * Mathf.Deg2Rad;
                pointsBelow[i] = MathUtils.PointOnCircle(new Vector2(0, 1), 1 - Margin, angle);
            }

            AddEdges(
                MathUtils.RotateVectors(pointsAbove, RotateTimes),
                MathUtils.RotateVectors(pointsBelow, RotateTimes)
            );
        }
    }
}
