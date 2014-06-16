using UnityEngine;
using NetworkManager;
using Cars;
using Utilities;

namespace GraphicalUI
{
    public class ThrottlerPart : ClientPart
    {
        private Texture2D TextureNormal, TexturePressed;

        public override void Initialize()
        {
            TextureNormal = TextureUtils.LoadTexture("gaspedaal-normaal");
            TexturePressed = TextureUtils.LoadTexture("gaspedaal-ingedrukt");
        }

        public override void DrawGraphicalUI()
        {
            PlayerAction currentAction = MainScript.SelfPlayer.Role.GetPlayerAction();

            DrawControls(
                currentAction == PlayerAction.SpeedDown ? TexturePressed : TextureNormal,
                currentAction == PlayerAction.SpeedUp ? TexturePressed : TextureNormal
            );
        }
    }
}
