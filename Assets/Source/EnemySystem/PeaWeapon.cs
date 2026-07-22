using UnityEngine;

namespace GMTK26.EnemySystem
{
	public class PeaWeapon : MonoBehaviour
	{
		[SerializeField]
		private PeaProjectile ProjectilePrefab;
		[SerializeField]
		private Transform FirePoint;
		[SerializeField]
		private float ProjectileSpeed = 8f;
		[SerializeField]
		private float Cooldown = 0.6f;

		private float _cooldownRemaining;

		public bool CanFire => _cooldownRemaining <= 0f && ProjectilePrefab != null;

		private void Update()
		{
			if (_cooldownRemaining > 0f)
			{
				_cooldownRemaining -= Time.deltaTime;
			}
		}

		public bool TryFire(Vector2 direction)
		{
			if (!CanFire)
			{
				return false;
			}

			var origin = FirePoint != null ? FirePoint.position : transform.position;
			var projectile = Instantiate(ProjectilePrefab, origin, Quaternion.identity);
			projectile.Launch(direction, ProjectileSpeed);
			_cooldownRemaining = Cooldown;
			return true;
		}
	}
}
