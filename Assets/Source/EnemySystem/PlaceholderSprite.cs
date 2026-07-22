using UnityEngine;

namespace GMTK26.EnemySystem
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class PlaceholderSprite : MonoBehaviour
	{
		[SerializeField]
		private Color Color = new(0.35f, 0.75f, 0.3f, 1f);

		private void Awake()
		{
			var renderer = GetComponent<SpriteRenderer>();
			if (renderer.sprite == null)
			{
				renderer.sprite = EnemyPlaceholderVisual.CreateCircleSprite(Color);
			}
		}
	}
}
