using UnityEngine;
using System.Collections;

public class BaanstukRechtBehaviour : BaanBehaviour {

    // From left to right.
	protected new static Vector2[] pointsAbove = new Vector2[] {
		new Vector2(0, margin),
        new Vector2(spriteSize, margin)
	};

    // From left to right.
	protected new static Vector2[] pointsBelow = new Vector2[] {
        new Vector2(0, spriteSize - margin),
        new Vector2(spriteSize, spriteSize - margin)
	};

	protected new void Start() {
		addEdges(pointsAbove, pointsBelow);
	}
}
