using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Main;

namespace GraphicalUI
{
    public class SignPart : GraphicalUIPart
    {
        private Dictionary<Vector2, Texture2D> Textures = new Dictionary<Vector2, Texture2D>();

        protected void AddArrow(float x, float y, Texture2D texture)
        {
            Textures.Add(new Vector2(x, y), texture);
        }

        private static Vector2 GetCenter(Vector2 point)
        {
            const float half = 1f / 2f;
            Vector2 centerPoint = point + new Vector2(half, half);
            bool xWasNegative = centerPoint.x < 0;
            bool yWasNegative = centerPoint.y < 0;
            centerPoint.x -= centerPoint.x % 1f;
            if (xWasNegative)
            {
                centerPoint.x -= 1f;
            }
            centerPoint.y -= centerPoint.y % 1f;
            if (yWasNegative)
            {
                centerPoint.y -= 1f;
            }
            return centerPoint;
        }

        public override void DrawGraphicalUI()
        {
            if (MainScript.SelfPlayer == null || MainScript.SelfCar == null || MainScript.SelfCar.CarObject == null)
            {
                return;
            }

            Vector3 carPos = MainScript.SelfCar.CarObject.transform.position;
            Vector2 point = GetCenter(new Vector2(carPos.x, carPos.y));

            Texture2D texture = (from k in Textures where k.Key == point select k.Value).FirstOrDefault();

            if (texture == null)
            {
                return;
            }

            GUI.DrawTexture(new Rect(0, 0, (float) Screen.width / 7 + 40f, (float) Screen.height / 4 + 40f), texture);
        }
    }
}
