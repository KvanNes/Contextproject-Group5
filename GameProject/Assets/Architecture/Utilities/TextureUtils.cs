using System;
using UnityEngine;

namespace Utilities
{
	public class TextureUtils 
	{
		public static Texture2D LoadTexture(String path)
		{
			return Resources.Load<Texture2D>(path);
		}
	}
}

