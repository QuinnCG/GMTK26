using GMTK26.InputSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GMTK26.PlayerSystem
{
	public class Player : MonoBehaviour
	{
		[SerializeField, Unit(Units.MetersPerSecond)]
		private float Speed = 3f;

		private GlobalInputActions _input;

		private void Awake()
		{
			_input = new GlobalInputActions();
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
			var inputDir = _input.Player.Move.ReadValue<Vector2>();
			transform.Translate(Speed * Time.deltaTime * inputDir);
		}
	}
}
