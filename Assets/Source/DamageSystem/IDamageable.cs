using UnityEngine;

namespace GMTK26.DamageSystem
{
	public interface IDamageable
	{
		public Team Team { get; }
		/// <summary>
		/// Attempt to apply damage to this object.
		/// </summary>
		/// <param name="damage">The actual damage to apply.</param>
		/// <param name="sourceTeam">The team that this source of damage is on. Friendly-fire should be ignored.</param>
		/// <param name="knockback">Direction * Speed of the knockback to apply, if any at all.</param>
		/// <returns>True, if damage was applied. False, if it was ignored.</returns>
		public bool TryTakeDamage(float damage, Team sourceTeam, Vector2 knockback = default);
	}
}
