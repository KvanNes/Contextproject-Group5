using UnityEngine;
using Utilities;

namespace GraphicalUI
{
    public class SignPartThrottler : SignPart
    {
        public override void Initialize()
        {
            Texture2D textureStraight = TextureUtils.LoadTexture("ArrowTextureStraight");
            Texture2D textureUTurnLeft = TextureUtils.LoadTexture("ArrowTextureUTurnLeft");
            Texture2D textureUTurnRight = TextureUtils.LoadTexture("ArrowTextureUTurnRight");
            Texture2D textureLeftCurve = TextureUtils.LoadTexture("ArrowTextureCurveLeft");
            Texture2D textureRightCurve = TextureUtils.LoadTexture("ArrowTextureCurveRight");

            AddArrow(0, 0, textureStraight);
            AddArrow(1, 0, textureStraight);
            AddArrow(2, 0, textureStraight);
            AddArrow(3, 0, textureStraight);
            AddArrow(4, 0, textureStraight);
            AddArrow(5, 0, textureStraight);
            AddArrow(6, 0, textureLeftCurve);
            AddArrow(7, 0, textureLeftCurve);
            AddArrow(7, 1, textureRightCurve);
            AddArrow(8, 1, textureUTurnLeft);
            AddArrow(8, 2, textureUTurnLeft);
            AddArrow(7, 2, textureStraight);
            AddArrow(6, 2, textureUTurnRight);
            AddArrow(5, 2, textureUTurnRight);
            AddArrow(5, 3, textureUTurnRight);
            AddArrow(6, 3, textureLeftCurve);
            AddArrow(6, 4, textureStraight);
            AddArrow(6, 5, textureLeftCurve);
            AddArrow(5, 5, textureStraight);
            AddArrow(4, 5, textureStraight);
            AddArrow(3, 5, textureUTurnLeft);
            AddArrow(2, 5, textureUTurnLeft);
            AddArrow(2, 4, textureUTurnLeft);
            AddArrow(3, 4, textureUTurnRight);
            AddArrow(3, 3, textureUTurnRight);
            AddArrow(2, 3, textureStraight);
            AddArrow(1, 3, textureStraight);
            AddArrow(0, 3, textureStraight);
            AddArrow(-1, 3, textureRightCurve);
            AddArrow(-2, 3, textureRightCurve);
            AddArrow(-2, 4, textureStraight);
            AddArrow(-2, 5, textureStraight);
            AddArrow(-2, 6, textureUTurnRight);
            AddArrow(-2, 7, textureUTurnRight);
            AddArrow(-1, 7, textureUTurnRight);
            AddArrow(-1, 6, textureLeftCurve);
            AddArrow(0, 6, textureStraight);
            AddArrow(1, 6, textureStraight);
            AddArrow(2, 6, textureStraight);
            AddArrow(3, 6, textureStraight);
            AddArrow(4, 6, textureLeftCurve);
            AddArrow(5, 6, textureLeftCurve);
            AddArrow(5, 7, textureStraight);
        }
    }
}