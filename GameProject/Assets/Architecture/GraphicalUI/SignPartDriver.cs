using UnityEngine;
using Utilities;

namespace GraphicalUI
{
    public class SignControllerDriver : SignController
    {
        public override void Initialize()
        {
            Texture2D textureSlow = TextureUtils.LoadTexture("TextureSlow");

            AddArrow(1, 3, textureSlow);
            AddArrow(0, 3, textureSlow);

            AddArrow(1, 6, textureSlow);
            AddArrow(2, 6, textureSlow);
        }
    }
}
