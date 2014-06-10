using Cars;
using NetworkManager;
using UnityEngine;
using Utilities;

namespace Controllers
{
    public class SignControllerThrottler : SignController
    {
        public override bool Enabled()
        {
            return MainScript.SelfPlayer.Role is Throttler;
        }

        public override void Start()
        {
            Texture2D TextureStraight = Utils.LoadTexture("ArrowTextureStraight");
            Texture2D TextureUTurnLeft = Utils.LoadTexture("ArrowTextureUTurnLeft");
            Texture2D TextureUTurnRight = Utils.LoadTexture("ArrowTextureUTurnRight");
            Texture2D TextureLeftCurve = Utils.LoadTexture("ArrowTextureCurveLeft");
            Texture2D TextureRightCurve = Utils.LoadTexture("ArrowTextureCurveRight");

            AddArrow(0, 0, TextureStraight);
            AddArrow(1, 0, TextureStraight);
            AddArrow(2, 0, TextureStraight);
            AddArrow(3, 0, TextureStraight);
            AddArrow(4, 0, TextureStraight);
            AddArrow(5, 0, TextureStraight);
            AddArrow(6, 0, TextureLeftCurve);
            AddArrow(7, 0, TextureLeftCurve);
            AddArrow(7, 1, TextureRightCurve);
            AddArrow(8, 1, TextureUTurnLeft);
            AddArrow(8, 2, TextureUTurnLeft);
            AddArrow(7, 2, TextureStraight);
            AddArrow(6, 2, TextureUTurnRight);
            AddArrow(5, 2, TextureUTurnRight);
            AddArrow(5, 3, TextureUTurnRight);
            AddArrow(6, 3, TextureLeftCurve);
            AddArrow(6, 4, TextureStraight);
            AddArrow(6, 5, TextureLeftCurve);
            AddArrow(5, 5, TextureStraight);
            AddArrow(4, 5, TextureStraight);
            AddArrow(3, 5, TextureUTurnLeft);
            AddArrow(2, 5, TextureUTurnLeft);
            AddArrow(2, 4, TextureUTurnLeft);
            AddArrow(3, 4, TextureUTurnRight);
            AddArrow(3, 3, TextureUTurnRight);
            AddArrow(2, 3, TextureStraight);
            AddArrow(1, 3, TextureStraight);
            AddArrow(0, 3, TextureStraight);
            AddArrow(-1, 3, TextureRightCurve);
            AddArrow(-2, 3, TextureRightCurve);
            AddArrow(-2, 4, TextureStraight);
            AddArrow(-2, 5, TextureStraight);
            AddArrow(-2, 6, TextureUTurnRight);
            AddArrow(-2, 7, TextureUTurnRight);
            AddArrow(-1, 7, TextureUTurnRight);
            AddArrow(-1, 6, TextureLeftCurve);
            AddArrow(0, 6, TextureStraight);
            AddArrow(1, 6, TextureStraight);
            AddArrow(2, 6, TextureStraight);
            AddArrow(3, 6, TextureStraight);
            AddArrow(4, 6, TextureLeftCurve);
            AddArrow(5, 6, TextureLeftCurve);
            AddArrow(5, 7, TextureStraight);
        }
    }
}