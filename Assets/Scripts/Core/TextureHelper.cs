using UnityEngine;

namespace Assets.Scripts
{
    public static class TextureHelper
    {
        public static Texture2D FromSprite(this Sprite sprite)
        {
            var text = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            var pixels = sprite.texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width, (int)sprite.rect.height);
            text.SetPixels(pixels);
            text.Apply();
            return text;
        }

        public static Texture2D FromSpriteWithFilterMode(this Sprite sprite, FilterMode filterMode)
        {
            var text = sprite.FromSprite();
            text.filterMode = filterMode;
            return text;
        }
    }
}