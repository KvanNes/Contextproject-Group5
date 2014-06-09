using System;
using UnityEngine;

public class TextureUtils {

	public static Texture2D LoadTexture(String path) {
		return (Texture2D) Resources.Load<Texture2D>(path);
	}
}
