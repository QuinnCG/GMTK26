using System;
using System.Collections.Generic;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace GMTK26.EnemySystem.Behavior
{
	[Serializable, GeneratePropertyBag]
	[NodeDescription(
		name: "Patrol 2D",
		description: "Moves the agent between waypoints on XY only. Does not rotate the transform (safe for sprites). Succeeds after each waypoint so higher-priority actions can run again.",
		story: "[Agent] patrols 2D along [Waypoints]",
		category: "Action/Enemy",
		id: "e8efed945e6e442993e8cec6600db598")]
	public partial class Patrol2DAction : Unity.Behavior.Action
	{
		[SerializeReference]
		public BlackboardVariable<GameObject> Agent;
		[SerializeReference]
		public BlackboardVariable<List<GameObject>> Waypoints;
		[SerializeReference]
		public BlackboardVariable<float> Speed = new(3f);
		[SerializeReference]
		public BlackboardVariable<float> ArriveDistance = new(0.15f);
		[SerializeReference]
		public BlackboardVariable<float> WaitAtWaypoint = new(0.25f);

		private int _index;
		private float _waitTimer;
		private bool _waiting;

		protected override Node.Status OnStart()
		{
			if (Agent?.Value == null || Waypoints?.Value == null || Waypoints.Value.Count == 0)
			{
				return Node.Status.Failure;
			}

			_waiting = false;
			_waitTimer = 0f;
			return Node.Status.Running;
		}

		protected override Node.Status OnUpdate()
		{
			if (Agent?.Value == null || Waypoints?.Value == null || Waypoints.Value.Count == 0)
			{
				return Node.Status.Failure;
			}

			var agentTransform = Agent.Value.transform;
			var waypoint = Waypoints.Value[_index];
			if (waypoint == null)
			{
				return Node.Status.Failure;
			}

			if (_waiting)
			{
				_waitTimer -= Time.deltaTime;
				if (_waitTimer > 0f)
				{
					return Node.Status.Running;
				}

				_waiting = false;
				_index = (_index + 1) % Waypoints.Value.Count;
				return Node.Status.Success;
			}

			var current = (Vector2)agentTransform.position;
			var target = (Vector2)waypoint.transform.position;
			var toTarget = target - current;
			var distance = toTarget.magnitude;

			if (distance <= ArriveDistance.Value)
			{
				_waiting = true;
				_waitTimer = WaitAtWaypoint.Value;
				return Node.Status.Running;
			}

			var step = Mathf.Min(Speed.Value * Time.deltaTime, distance);
			var next = current + toTarget.normalized * step;
			agentTransform.position = new Vector3(next.x, next.y, agentTransform.position.z);

			if (Mathf.Abs(toTarget.x) >= 0.01f)
			{
				var scale = agentTransform.localScale;
				scale.x = Mathf.Sign(toTarget.x) * Mathf.Abs(scale.x);
				agentTransform.localScale = scale;
			}

			return Node.Status.Running;
		}
	}
}
