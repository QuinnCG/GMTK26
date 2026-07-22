using Sirenix.OdinInspector;
using UnityEngine;

namespace GMTK26.DamageSystem
{
	public class Health : MonoBehaviour, IDamageable
	{
		[SerializeField, Tooltip("The default health this entity starts with.")]
		private float Default = 100f;
		[field: SerializeField]
		public Team Team { get; private set; } = Team.Hostile;

		[ShowInInspector, ReadOnly, HideInEditorMode]
		public float Current { get; private set; }
		[ShowInInspector, ReadOnly, HideInEditorMode]
		public float Max { get; private set; }
		/// <summary>
		/// A 0-1 value calculated as current HP divided by max HP, where 0 is dead and 1 is 100%.
		/// </summary>
		public float Normalized => Current / Max;

		public bool IsDead { get; private set; }

		private void Awake()
		{
			Current = Max = Default;
		}

		public bool TryTakeDamage(float damage, Team sourceTeam, Vector2 knockback = default)
		{
			Debug.Log("Health system not implemented yet!");
			return true;
		}
	}
}
