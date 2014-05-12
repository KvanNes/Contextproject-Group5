using UnityEngine;
using System.Collections;

public class BaanstukRechtBehaviour : BaanBehaviour {

	protected new static Vector2[] pointsAbove = new Vector2[] {
		new Vector2(0, margin),
		new Vector2(unit, margin)
	};
	protected new static Vector2[] pointsBelow = new Vector2[] {
		new Vector2(0, unit - margin),
		new Vector2(unit, unit - margin)
	};

	protected new void Start () {
		addEdges(pointsAbove, pointsBelow);
	}
	
    private void Update () {
	
	}
}
