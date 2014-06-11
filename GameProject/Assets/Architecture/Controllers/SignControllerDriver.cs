using Cars;
using NetworkManager;
using UnityEngine;
using Utilities;

namespace Controllers
{
    public class SignControllerDriver : SignController
    {
        public override bool Enabled()
        {
            return MainScript.SelfPlayer.Role is Driver;
        }

        public override void Start()
        {
            Texture2D textureSlow = TextureUtils.LoadTexture("TextureSlow");

            AddArrow(1, 3, textureSlow);
            AddArrow(0, 3, textureSlow);

            AddArrow(1, 6, textureSlow);
            AddArrow(2, 6, textureSlow);
        }
    }
}
