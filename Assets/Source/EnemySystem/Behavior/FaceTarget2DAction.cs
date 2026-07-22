using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace GMTK26.EnemySystem.Behavior
{
	[Serializable, GeneratePropertyBag]
	[NodeDescription(
		name: "Face Target 2D",
		description: "Flips the agent sprite on X to face the target (top-down / side 2D).",
		story: "[Agent] faces [Target] in 2D",
		category: "Action/Enemy",
		id: "33feab9b0ba3416ca8145e0321687fc3")]
	public partial class FaceTarget2DAction : Unity.Behavior.Action
	{
		[SerializeReference]
		public BlackboardVariable<GameObject> Agent;
		[SerializeReference]
		public BlackboardVariable<GameObject> Target;
		[SerializeReference]
		public BlackboardVariable<bool> Continuous = new(true);

		protected override Node.Status OnStart()
		{
			if (Agent?.Value == null || Target?.Value == null)
			{
				return Node.Status.Failure;
			}

			Face();
			return Continuous.Value ? Node.Status.Running : Node.Status.Success;
		}

		protected override Node.Status OnUpdate()
		{
			if (Agent?.Value == null || Target?.Value == null)
			{
				return Node.Status.Failure;
			}

			if (!Continuous.Value)
			{
				return Node.Status.Success;
			}

			Face();
			return Node.Status.Running;
		}

		private void Face()
		{
			var deltaX = Target.Value.transform.position.x - Agent.Value.transform.position.x;
			if (Mathf.Abs(deltaX) < 0.01f)
			{
				return;
			}

			var scale = Agent.Value.transform.localScale;
			scale.x = Mathf.Sign(deltaX) * Mathf.Abs(scale.x);
			Agent.Value.transform.localScale = scale;
		}
	}
}
