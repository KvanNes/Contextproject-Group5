using UnityEngine;
using System;
using Utilities;

public class SignControllerDriver : SignController
{
    public override bool Enabled()
    {
        return false; // Disable until mud tiles are added to the track.
        // return MainScipt.selfPlayer.Role is Driver;
    }

    public override void Start()
    {
        Texture2D textureSlow = Utils.LoadTexture("TextureSlow");

        AddArrow(0, 0, textureSlow);
    }
}
