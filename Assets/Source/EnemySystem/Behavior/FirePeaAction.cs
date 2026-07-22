using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace GMTK26.EnemySystem.Behavior
{
	[Serializable, GeneratePropertyBag]
	[NodeDescription(
		name: "Fire Pea",
		description: "Fires a pea toward the target using PeaWeapon on the agent. Stays Running and fires on cooldown.",
		story: "[Agent] fires peas at [Target]",
		category: "Action/Enemy",
		id: "c3dac94b43384cdc91289d26ff4ee087")]
	public partial class FirePeaAction : Unity.Behavior.Action
	{
		[SerializeReference]
		public BlackboardVariable<GameObject> Agent;
		[SerializeReference]
		public BlackboardVariable<GameObject> Target;

		private PeaWeapon _weapon;

		protected override Node.Status OnStart()
		{
			if (Agent?.Value == null || Target?.Value == null)
			{
				return Node.Status.Failure;
			}

			_weapon = Agent.Value.GetComponentInChildren<PeaWeapon>();
			if (_weapon == null)
			{
				LogFailure("Agent is missing PeaWeapon.");
				return Node.Status.Failure;
			}

			TryFire();
			return Node.Status.Running;
		}

		protected override Node.Status OnUpdate()
		{
			if (Agent?.Value == null || Target?.Value == null || _weapon == null)
			{
				return Node.Status.Failure;
			}

			TryFire();
			return Node.Status.Running;
		}

		private void TryFire()
		{
			var direction = (Vector2)(Target.Value.transform.position - Agent.Value.transform.position);
			_weapon.TryFire(direction);
		}
	}
}
