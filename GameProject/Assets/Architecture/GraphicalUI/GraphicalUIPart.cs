using UnityEngine;

namespace GraphicalUI {
    public abstract class GraphicalUIPart {
        public bool Initialized = false;
        public abstract void DrawGraphicalUI();
        public virtual void Initialize() { }
    }
}
