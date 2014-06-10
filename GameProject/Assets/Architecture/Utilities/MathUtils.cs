using System;
using UnityEngine;
using System.Collections.Generic;

namespace Utilities
{
	public class MathUtils 
	{
		public static Vector2 Vector3To2(Vector3 v)
		{
			return new Vector2(v.x, v.y);
		}
		
		public static Vector3 Vector2To3(Vector2 v)
		{
			return new Vector3(v.x, v.y, 0f);
		}
		
		public static Vector2 Rotate(Vector2 point, Vector2 midpoint, float angle)
		{
			// Formules komen van: https://www.siggraph.org/education/materials/HyperGraph/modeling/mod_tran/2drota.htm
			Vector2 p = point - midpoint;
			float xNew = p.x * Mathf.Cos(angle) - p.y * Mathf.Sin(angle);
			float yNew = p.y * Mathf.Cos(angle) + p.x * Mathf.Sin(angle);
			return new Vector2(xNew, yNew) + midpoint;
		}
		
		public static Vector2[] RotateVectors(Vector2[] vectors, int count)
		{
			Vector2[] res = new Vector2[vectors.Length];
			for (int i = 0; i < vectors.Length; i++)
			{
				res[i] = Rotate(vectors[i], new Vector2(0.5f, 0.5f), Mathf.Deg2Rad * 90 * count);
			}
			return res;
		}

		// Copies (clones) a Quaternion.
		public static Quaternion Copy(Quaternion v)
		{
			return new Quaternion(v.x, v.y, v.z, v.w);
		}
		
		// Copies (clones) a Vector3.
		public static Vector3 Copy(Vector3 v)
		{
			return new Vector3(v.x, v.y, v.z);
		}
		
		public static float ForceInInterval(float x, float min, float max)
		{
			return Mathf.Min(Mathf.Max(min, x), max);
		}
	}
}

