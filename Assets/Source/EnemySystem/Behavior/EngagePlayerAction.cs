using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace GMTK26.EnemySystem.Behavior
{
	[Serializable, GeneratePropertyBag]
	[NodeDescription(
		name: "Engage Player",
		description: "While the player is within DetectRange, face them and fire peas. Fails when out of range so patrol can run.",
		story: "[Agent] engages [Target] within [DetectRange]",
		category: "Action/Enemy",
		id: "3a5431ac0c4d40d1911866e79aae343c")]
	public partial class EngagePlayerAction : Unity.Behavior.Action
	{
		[SerializeReference]
		public BlackboardVariable<GameObject> Agent;
		[SerializeReference]
		public BlackboardVariable<GameObject> Target;
		[SerializeReference]
		public BlackboardVariable<float> DetectRange = new(6f);

		private PeaWeapon _weapon;

		protected override Node.Status OnStart()
		{
			if (!Validate())
			{
				return Node.Status.Failure;
			}

			_weapon = Agent.Value.GetComponentInChildren<PeaWeapon>();
			if (_weapon == null)
			{
				LogFailure("Agent is missing PeaWeapon.");
				return Node.Status.Failure;
			}

			if (!IsInRange())
			{
				return Node.Status.Failure;
			}

			EngageTick();
			return Node.Status.Running;
		}

		protected override Node.Status OnUpdate()
		{
			if (!Validate() || _weapon == null)
			{
				return Node.Status.Failure;
			}

			if (!IsInRange())
			{
				return Node.Status.Failure;
			}

			EngageTick();
			return Node.Status.Running;
		}

		private bool Validate()
		{
			return Agent?.Value != null && Target?.Value != null;
		}

		private bool IsInRange()
		{
			var distance = Vector2.Distance(Agent.Value.transform.position, Target.Value.transform.position);
			return distance <= DetectRange.Value;
		}

		private void EngageTick()
		{
			var agentTransform = Agent.Value.transform;
			var targetTransform = Target.Value.transform;
			var delta = targetTransform.position - agentTransform.position;
			var deltaX = delta.x;
			if (Mathf.Abs(deltaX) >= 0.01f)
			{
				var scale = agentTransform.localScale;
				scale.x = Mathf.Sign(deltaX) * Mathf.Abs(scale.x);
				agentTransform.localScale = scale;
			}

			_weapon.TryFire((Vector2)delta);
		}
	}
}
