using UnityEngine;

namespace GMTK26.EnemySystem
{
	public class PeaProjectile : MonoBehaviour
	{
		[SerializeField]
		private float Speed = 8f;
		[SerializeField]
		private float Lifetime = 3f;
		[SerializeField]
		private Color Color = new(0.85f, 1f, 0.35f, 1f);

		private Vector2 _direction = Vector2.right;
		private float _age;

		private void Awake()
		{
			var renderer = GetComponent<SpriteRenderer>();
			if (renderer != null && renderer.sprite == null)
			{
				renderer.sprite = EnemyPlaceholderVisual.CreateCircleSprite(Color, 16);
			}
		}

		public void Launch(Vector2 direction, float speed)
		{
			_direction = direction.sqrMagnitude > 0f ? direction.normalized : Vector2.right;
			Speed = speed;
			_age = 0f;
		}

		private void Update()
		{
			transform.position += (Vector3)(_direction * Speed * Time.deltaTime);
			_age += Time.deltaTime;
			if (_age >= Lifetime)
			{
				Destroy(gameObject);
			}
		}
	}
}
