using UnityEngine;

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

        public static float CalculateAngle(Vector2 v)
        {
            return Mathf.Atan2(v.y, v.x);
        }

        public static Vector2 Rotate(Vector2 point, Vector2 midpoint, float angle)
        {
            //Formulas found on: https://www.siggraph.org/education/materials/HyperGraph/modeling/mod_tran/2drota.htm
            Vector2 rotatePoint = point - midpoint;
            float xNew = rotatePoint.x * Mathf.Cos(angle) - rotatePoint.y * Mathf.Sin(angle);
            float yNew = rotatePoint.y * Mathf.Cos(angle) + rotatePoint.x * Mathf.Sin(angle);
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

        public static Vector2 PointOnCircle(Vector2 midpoint, float radius, float angle)
        {
            return new Vector2(
                Mathf.Cos(angle),
                Mathf.Sin(angle)
            ) * radius + midpoint;
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

