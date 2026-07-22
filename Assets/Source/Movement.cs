using Sirenix.OdinInspector;
using UnityEngine;

namespace GMTK26
{
	/// <summary>
	/// Generic character movement script that supports basic movement, dashing, and knockback.
	/// </summary>
	[RequireComponent(typeof(Rigidbody2D))]
	public class Movement : MonoBehaviour
	{
		[field: Tooltip("The rate at which knockback velocity is decayed over time.")]
		[field: SerializeField, Unit(Units.MetersPerSecondSquared)]
		public float KnockbackDecayRate { get; set; } = 32f;

		private Rigidbody2D _rb;
		// Accumulates for 1 frame, gets applied, then resets.
		private Vector2 _cumulativeVel;
		private Vector2 _overrideVel;
		private Vector2 _knockbackVel;

		private void Awake()
		{
			_rb = GetComponent<Rigidbody2D>();
		}

		public void AddVelocity(Vector2 velocity)
		{
			_cumulativeVel += velocity;
		}

		/// <summary>
		/// Overrides additive velocity with this 1 frame.<br/>
		/// Use for things like dashing.<br/>
		/// Override knockback too.
		/// </summary>
		public void SetVelocity(Vector2 velocity)
		{
			_overrideVel = velocity;
		}

		/// <summary>
		/// Calling this will apply a knockback that will decay over time based on <see cref="KnockbackDecayRate"/>.<br/>
		/// This knockback will override existing knockback.
		/// </summary>
		public void Knockback(Vector2 velocity)
		{
			_knockbackVel = velocity;
		}

		private void LateUpdate()
		{
			if (_knockbackVel.sqrMagnitude > 0f)
			{
				_cumulativeVel += _knockbackVel;

				float magnitude = _knockbackVel.magnitude;
				Vector2 dir = _knockbackVel.normalized;

				magnitude = Mathf.Max(0f, magnitude - (KnockbackDecayRate * Time.deltaTime));
				_knockbackVel = dir * magnitude;
			}

			// Override velocity is used if it has a magnitude greater than 0.
			_rb.linearVelocity = (_overrideVel.sqrMagnitude > 0f) ? _overrideVel : _cumulativeVel;

			_cumulativeVel = Vector2.zero;
			_overrideVel = Vector2.zero;
		}
	}
}
