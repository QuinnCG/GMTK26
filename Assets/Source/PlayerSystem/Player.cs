using FMODUnity;
using GMTK26.InputSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GMTK26.PlayerSystem
{
[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(Rigidbody2D))]
	public class Player : MonoBehaviour
	{
		[SerializeField, Unit(Units.MetersPerSecond)]
		private float Speed = 3f;
		[SerializeField]
		private EventReference FootstepSound;

		private GlobalInputActions _input;
		private Animator _animator;
		private Rigidbody2D _rb;

		private void Awake()
		{
			_input = new GlobalInputActions();
			_animator = GetComponent<Animator>();
			_rb = GetComponent<Rigidbody2D>();
		}

		private void OnEnable()
		{
			_input.Enable();
		}

		private void OnDisable()
		{
			_input.Disable();
		}

		private void Update()
		{
			var inputDir = _input.Player.Move.ReadValue<Vector2>().normalized;
			_rb.linearVelocity = inputDir * Speed;

			bool isMoving = inputDir.sqrMagnitude > 0f;
			_animator.Play(isMoving ? "Moving" : "Idling");

			if (inputDir.x != 0f)
			{
				transform.localScale = new Vector3(Mathf.Sign(inputDir.x), 1f, 1f);
			}
		}

		private void FootstepEvent()
		{
			RuntimeManager.PlayOneShot(FootstepSound, transform.position);
		}
	}
}
