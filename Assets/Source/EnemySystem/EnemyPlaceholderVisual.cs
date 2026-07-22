using UnityEngine;

namespace GMTK26.EnemySystem
{
	public static class EnemyPlaceholderVisual
	{
		public static Sprite CreateCircleSprite(Color color, int size = 32)
		{
			var texture = new Texture2D(size, size, TextureFormat.RGBA32, false)
			{
				filterMode = FilterMode.Point,
				wrapMode = TextureWrapMode.Clamp,
				name = "EnemyPlaceholder"
			};

			var center = (size - 1) * 0.5f;
			var radius = size * 0.45f;
			for (var y = 0; y < size; y++)
			{
				for (var x = 0; x < size; x++)
				{
					var dx = x - center;
					var dy = y - center;
					var inside = dx * dx + dy * dy <= radius * radius;
					texture.SetPixel(x, y, inside ? color : Color.clear);
				}
			}

			texture.Apply(false, true);
			return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
		}
	}
}
