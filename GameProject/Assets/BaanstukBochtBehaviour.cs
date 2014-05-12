using UnityEngine;
using System.Collections;

public class BaanstukBochtBehaviour : BaanBehaviour {

	private const float f = 0.7f;
	protected new static Vector2[] pointsAbove = new Vector2[] {
		new Vector2(0, unit - margin),
		new Vector2(margin, unit)
	};
	protected new static Vector2[] pointsBelow = new Vector2[] {
		new Vector2(0 - margin, 0),
		new Vector2(unit*f, unit*(1-f)),
		new Vector2(unit - margin, unit)
	};
	
	protected new void Start () {
		addEdges(pointsAbove, pointsBelow);
	}
	
    private void Update () {

	}
}
