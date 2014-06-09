using UnityEngine;
using System.Collections;

public class TrackBehaviourCurve : TrackBehaviour {

	private const float curveFactor = 0.7f;
    // Set in Editor to rotate the collision edges appropriately.
    public int rotateTimes = 0;

    // The inner curve.
	private Vector2[] pointsAbove = new Vector2[] {
		new Vector2(0, 1 - margin),
        new Vector2(margin, 1)
	};

    // The outer curve.
    private Vector2[] pointsBelow = new Vector2[] {
		new Vector2(0, margin),
        new Vector2(curveFactor, 1 - curveFactor),
        new Vector2(1 - margin, 1)
	};
	
	public override void Start() {
		addEdges(
            MathUtils.RotateVectors(pointsAbove, rotateTimes),
            MathUtils.RotateVectors(pointsBelow, rotateTimes)
        );
	}
}
