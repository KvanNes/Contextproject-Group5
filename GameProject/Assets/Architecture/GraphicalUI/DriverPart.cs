using UnityEngine;
using Cars;
using Utilities;

namespace GraphicalUI
{
    public class DriverPart : ClientPart
    {
        public Texture2D TextureLeftNormal, TextureLeftPressed, TextureRightNormal, TextureRightPressed;

        public override void Initialize()
        {
            TextureLeftNormal = TextureUtils.LoadTexture("stuur-links-normaal");
            TextureLeftPressed = TextureUtils.LoadTexture("stuur-links-ingedrukt");
            TextureRightNormal = TextureUtils.LoadTexture("stuur-rechts-normaal");
            TextureRightPressed = TextureUtils.LoadTexture("stuur-rechts-ingedrukt");
        }
        
        public override void BecomeVisible()
        {
            Camera.main.backgroundColor = Color.black;
        }

        public override void DrawGraphicalUI()
        {
            PlayerAction currentAction = Action.GetPlayerAction(PlayerType.Driver); //MainScript.SelfPlayer.Role.GetPlayerAction();

            DrawControls(
                currentAction == PlayerAction.SteerLeft ? TextureLeftPressed : TextureLeftNormal,
                currentAction == PlayerAction.SteerRight ? TextureRightPressed : TextureRightNormal
            );
        }
    }
}
