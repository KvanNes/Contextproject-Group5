using UnityEngine;

namespace Behaviours
{
    public class TrackBehaviourStraight : TrackBehaviour
    {

        // From left to right.
        private Vector2[] pointsAbove = 
        {
            new Vector2(0, Margin),
            new Vector2(1, Margin)
        };

        // From left to right.
        private Vector2[] pointsBelow = 
        {
            new Vector2(0, 1 - Margin),
            new Vector2(1, 1 - Margin)
        };

        public override void Start()
        {
            AddEdges(pointsAbove, pointsBelow);
        }
    }
}