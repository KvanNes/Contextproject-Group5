using UnityEngine;

namespace GraphicalUI
{
    public abstract class ClientPart : GraphicalUIPart
    {
        private const float ButtonsFactor = 0.4f;

        private void DrawControl(Texture2D texture, float left, float top)
        {
            GUI.DrawTexture(
                new Rect(left, top, texture.width * ButtonsFactor, texture.height * ButtonsFactor),
                texture
            );
        }

        protected void DrawControls(Texture2D leftTexture, Texture2D rightTexture)
        {
            DrawControl(leftTexture, 0, Screen.height - leftTexture.height * ButtonsFactor);
            DrawControl(rightTexture, Screen.width - rightTexture.width * ButtonsFactor, Screen.height - rightTexture.height * ButtonsFactor);
        }
    }
}
