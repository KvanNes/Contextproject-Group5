using UnityEngine;
using Cars;
using Utilities;

namespace GraphicalUI
{
    public class ThrottlerPart : ClientPart
    {
        public Texture2D TextureNormal, TexturePressed;

        public override void Initialize()
        {
            TextureNormal = TextureUtils.LoadTexture("gaspedaal-normaal");
            TexturePressed = TextureUtils.LoadTexture("gaspedaal-ingedrukt");
        }

        public override void BecomeVisible()
        {
            Camera.main.backgroundColor = Color.black;
        }

        public override void DrawGraphicalUI()
        {
            PlayerAction currentAction = Action.GetPlayerAction(PlayerType.Throttler); // MainScript.SelfPlayer.Role.GetPlayerAction();

            DrawControls(
                currentAction == PlayerAction.SpeedDown ? TexturePressed : TextureNormal,
                currentAction == PlayerAction.SpeedUp ? TexturePressed : TextureNormal
            );
        }
    }
}
