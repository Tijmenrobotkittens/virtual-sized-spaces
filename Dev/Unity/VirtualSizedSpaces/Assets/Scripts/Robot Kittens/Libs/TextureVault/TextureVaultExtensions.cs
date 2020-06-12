using System.IO;
using UnityEngine;
using System.Collections.Generic;
namespace RobotKittens
{

    public static class TextureVaultExtensions
    {
        public static Sprite ToSprite(this Texture2D texture)
        {
            if(texture == null)
            {
                texture = Texture2D.blackTexture;
            }
            return Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)), Vector2.zero);
        }
    }

}
