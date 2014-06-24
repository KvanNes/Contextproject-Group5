using UnityEngine;

namespace GraphicalUI
{
    public abstract class ClientPart : GraphicalUIPart
    {
        private const float WidthFactor = (float)1 / 5;
        private const float HeightFactor = (float)1 / 4;
        private const float WidthFactorThrottle = (float)1 / 9;
        private const float HeightFactorThrottle = (float)1 / 4;


        private void DrawControl(Texture2D texture, float left, float top)
        {
            float width = texture.width == 128 ? WidthFactorThrottle : WidthFactor;
            float height = texture.width == 128 ? HeightFactorThrottle : HeightFactor;

            GUI.DrawTexture(
                new Rect(left, top, Screen.width * width, Screen.height * height),
                texture
            );

        }

        protected void DrawControls(Texture2D leftTexture, Texture2D rightTexture)
        {
            float leftTop = leftTexture.width == 128 ? (1 - HeightFactorThrottle) : (1 - HeightFactor);
            float rightLeft = leftTexture.width == 128 ? (1 - WidthFactorThrottle) : (1 - WidthFactor);
            float rightTop = leftTexture.width == 128 ? (1 - HeightFactorThrottle) : (1 - HeightFactor);

            DrawControl(leftTexture, 0, Screen.height * leftTop);
            DrawControl(rightTexture,
                Screen.width * rightLeft,
                Screen.height * rightTop);
        }
    }
}
