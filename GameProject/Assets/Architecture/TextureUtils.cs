using System;
using UnityEngine;
using System.Collections;

public class TextureUtils {

	public static Texture2D LoadTexture(String path) {
		return (Texture2D) Resources.Load<Texture2D>(path);
	}

}


