using UnityEngine;

namespace GraphicalUI
{
    public abstract class GraphicalUIPart
    {
        public bool Initialized = false;
        public abstract void DrawGraphicalUI();
        public virtual void Initialize() { }
        public virtual void OnPush() { }

        protected bool DrawTextureButton(Rect rect, Texture2D texture) {
            GUI.DrawTexture(rect, texture);
            return GUI.Button(rect, "", GUIStyle.none);
        }
    }
}
