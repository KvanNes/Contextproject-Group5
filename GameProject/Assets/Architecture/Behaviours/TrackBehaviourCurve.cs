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
        private Vector2[] pointsBelow = 
        {
            new Vector2(0, Margin),
            new Vector2(CurveFactor, 1 - CurveFactor),
            new Vector2(1 - Margin, 1)
        };

        public override void Start()
        {
            AddEdges(
                Utils.RotateVectors(pointsAbove, RotateTimes),
                Utils.RotateVectors(pointsBelow, RotateTimes)
                );
        }
    }
}
