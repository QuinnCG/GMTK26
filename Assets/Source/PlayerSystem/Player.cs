using FMODUnity;
using GMTK26.InputSystem;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

namespace GMTK26.PlayerSystem
{
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(Movement))]
	public class Player : MonoBehaviour
	{
		private const string IdlingAnimHash = "Idling";
		private const string MovingAnimHash = "Moving";
		private const string DashingAnimHash = "Dashing";
		private const string DeathAnimHash = "Death";

		[SerializeField, Unit(Units.MetersPerSecond)]
		private float MoveSpeed = 3f;

		[Space]

		[SerializeField, Unit(Units.MetersPerSecond)]
		private float DashSpeed = 12f;
		[SerializeField, Unit(Units.Meter)]
		private float DashDistance = 3f;
		[SerializeField, Unit(Units.Second)]
		private float DashCooldown = 0.5f;

		[Title("VFX")]

		// TODO: Need better system for swapping dash vfx based on character transformation.
		[SerializeField, Required]
		private VisualEffect DashTrail;

		[Title("SFX")]

		[SerializeField]
		private EventReference DashSound;
		[SerializeField]
		private EventReference FootstepSound;

		private GlobalInputActions _input;
		private Animator _animator;
		private Movement _movement;

		public bool IsDashing => Time.time < _dashEndTime;

		private Vector2 _lastMoveDir = Vector2.right;
		private float _dashEndTime;
		private float _nextAllowedDashTime;

		private void Awake()
		{
			_input = new GlobalInputActions();
			_animator = GetComponent<Animator>();
			_movement = GetComponent<Movement>();

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;

			SetUpInputBindings();
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
			Vector2 inputDir = UpdateSimpleMove();
			UpdateDashing();
			UpdateAnimations(inputDir);
			UpdateFacingDirection(inputDir);
		}

		private void SetUpInputBindings()
		{
			_input.Player.Dash.performed += OnDash;
		}

		private void OnDash(InputAction.CallbackContext context)
		{
			if (Time.time > _nextAllowedDashTime)
			{
				float duration = DashDistance / DashSpeed;
				_dashEndTime = Time.time + duration;

				// Next allowed dash time = now + duration of dash + cooldown afterwards.
				_nextAllowedDashTime = Time.time + duration + DashCooldown;

				RuntimeManager.PlayOneShot(DashSound, transform.position);
				DashTrail.Play();
			}
		}

		private Vector2 UpdateSimpleMove()
		{
			if (IsDashing)
			{
				return Vector2.zero;
			}

			var inputDir = _input.Player.Move.ReadValue<Vector2>();
			_movement.AddVelocity(inputDir.normalized * MoveSpeed);

			if (inputDir.sqrMagnitude > 0f)
			{
				_lastMoveDir = inputDir;
			}

			return inputDir.normalized;
		}

		private void UpdateDashing()
		{
			if (IsDashing)
			{
				_movement.SetVelocity(_lastMoveDir * DashSpeed);
			}
			else
			{
				DashTrail.Stop();
			}
		}

		private void UpdateAnimations(Vector2 inputDir)
		{
			if (IsDashing)
			{
				_animator.Play(DashingAnimHash);
			}
			// Simple moving (walking).
			else
			{
				bool isMoving = inputDir.sqrMagnitude > 0f;
				_animator.Play(isMoving ? MovingAnimHash : IdlingAnimHash);
			}
		}

		private void UpdateFacingDirection(Vector2 inputDir)
		{
			if (inputDir.x != 0f)
			{
				transform.localScale = new Vector3(Mathf.Sign(inputDir.x), 1f, 1f);
			}
		}

		// Called automatically via Aseprite animation events.
		private void FootstepEvent()
		{
			RuntimeManager.PlayOneShot(FootstepSound, transform.position);
		}
	}
}
