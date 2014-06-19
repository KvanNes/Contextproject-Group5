using UnityEngine;

namespace Behaviours
{
    public class TrackBehaviourStraight : TrackBehaviour
    {
        private Vector2[] pointsAbove = 
        {
            new Vector2(0, BorderMargin),
            new Vector2(1, BorderMargin)
        };

        private Vector2[] pointsBelow = 
        {
            new Vector2(0, 1 - BorderMargin),
            new Vector2(1, 1 - BorderMargin)
        };

        public override void Start()
        {
            AddEdges(pointsAbove, pointsBelow);
        }
    }
}