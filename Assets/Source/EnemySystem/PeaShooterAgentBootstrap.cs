using System.Collections.Generic;
using GMTK26.PlayerSystem;
using Unity.Behavior;
using UnityEngine;

namespace GMTK26.EnemySystem
{
	[RequireComponent(typeof(BehaviorGraphAgent))]
	public class PeaShooterAgentBootstrap : MonoBehaviour
	{
		[SerializeField]
		private BehaviorGraphAgent Agent;
		[SerializeField]
		private Transform[] PatrolPoints;
		[SerializeField]
		private string AgentVariableName = "Agent";
		[SerializeField]
		private string PlayerVariableName = "Player";
		[SerializeField]
		private string PlayerTransformVariableName = "PlayerTransform";
		[SerializeField]
		private string SelfTransformVariableName = "SelfTransform";
		[SerializeField]
		private string WaypointsVariableName = "Waypoints";

		private void Awake()
		{
			if (Agent == null)
			{
				Agent = GetComponent<BehaviorGraphAgent>();
			}
		}

		private void Start()
		{
			if (Agent == null || Agent.Graph == null)
			{
				Debug.LogWarning("PeaShooterAgentBootstrap needs a BehaviorGraphAgent with a graph assigned.", this);
				return;
			}

			Agent.Init();

			var player = FindFirstObjectByType<Player>();
			Agent.SetVariableValue(AgentVariableName, gameObject);
			Agent.SetVariableValue(SelfTransformVariableName, transform);
			Agent.SetVariableValue("DetectRange", 6f);

			if (player != null)
			{
				Agent.SetVariableValue(PlayerVariableName, player.gameObject);
				Agent.SetVariableValue(PlayerTransformVariableName, player.transform);
			}
			else
			{
				Debug.LogWarning("PeaShooterAgentBootstrap could not find a Player in the scene.", this);
			}

			if (PatrolPoints != null && PatrolPoints.Length > 0)
			{
				var waypoints = new List<GameObject>(PatrolPoints.Length);
				foreach (var point in PatrolPoints)
				{
					if (point == null)
					{
						continue;
					}

					// Keep patrol targets fixed in world space even if authored as children.
					point.SetParent(null, true);
					waypoints.Add(point.gameObject);
				}

				Agent.SetVariableValue(WaypointsVariableName, waypoints);
			}
		}
	}
}
