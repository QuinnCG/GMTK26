using UnityEngine;

namespace GMTK26.MissileSystem
{
	public class MissileManager : MonoBehaviour
	{
		private static MissileManager _instance;

		private void Awake()
		{
			_instance = this;
		}

		public static Missile Spawn(Vector2 origin, Vector2 direction, GameObject prefab)
		{
			var instance = Object.Instantiate(prefab, origin, Quaternion.identity, _instance.transform);
			var missile = instance.GetComponent<Missile>();
			missile.Init(direction);

			return missile;
		}
	}
}
