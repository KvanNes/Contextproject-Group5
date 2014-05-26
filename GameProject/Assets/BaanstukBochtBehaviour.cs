using UnityEngine;
using System.Collections;

public class BaanstukBochtBehaviour : BaanBehaviour {

	private const float curveFactor = 0.7f;

    // The inner curve.
	protected new static Vector2[] pointsAbove = new Vector2[] {
		new Vector2(0, spriteSize - margin),
        new Vector2(margin, spriteSize)
	};

    // The outer curve.
	protected new static Vector2[] pointsBelow = new Vector2[] {
		new Vector2(0 - margin, 0),
        new Vector2(spriteSize*curveFactor, spriteSize*(1-curveFactor)),
        new Vector2(spriteSize - margin, spriteSize)
	};
	
	protected new void Start() {
		addEdges(pointsAbove, pointsBelow);
	}
}
