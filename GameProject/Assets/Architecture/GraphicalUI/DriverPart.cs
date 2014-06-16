using UnityEngine;
using NetworkManager;
using Cars;
using Utilities;

namespace GraphicalUI
{
    public class DriverPart : ClientPart
    {
        private Texture2D _textureLeftNormal, _textureLeftPressed, _textureRightNormal, _textureRightPressed;

        public override void Initialize()
        {
            _textureLeftNormal = TextureUtils.LoadTexture("stuur-links-normaal");
            _textureLeftPressed = TextureUtils.LoadTexture("stuur-links-ingedrukt");
            _textureRightNormal = TextureUtils.LoadTexture("stuur-rechts-normaal");
            _textureRightPressed = TextureUtils.LoadTexture("stuur-rechts-ingedrukt");
        }

        public override void DrawGraphicalUI()
        {
            PlayerAction currentAction = MainScript.SelfPlayer.Role.GetPlayerAction();

            DrawControls(
                currentAction == PlayerAction.SteerLeft ? _textureLeftPressed : _textureLeftNormal,
                currentAction == PlayerAction.SteerRight ? _textureRightPressed : _textureRightNormal
            );
        }
    }
}
