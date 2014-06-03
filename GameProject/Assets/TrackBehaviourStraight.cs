using UnityEngine;
using System.Collections;

public class TrackBehaviourStraight : TrackBehaviour {

    // From left to right.
	private Vector2[] pointsAbove = new Vector2[] {
		new Vector2(0, margin),
        new Vector2(1, margin)
	};

    // From left to right.
    private Vector2[] pointsBelow = new Vector2[] {
        new Vector2(0, 1 - margin),
        new Vector2(1, 1 - margin)
	};

	public override void Start() {
		addEdges(pointsAbove, pointsBelow);
	}
}
