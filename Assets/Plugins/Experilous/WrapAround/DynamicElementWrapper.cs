using UnityEngine;

namespace Experilous.WrapAround
{
	public class DynamicElementWrapper : MonoBehaviour, IWorldConsumer
	{
		public World world;

		public bool hasWorld { get { return world != null ; } }
		public void SetWorld(World world) { this.world = world; }

		protected void Start()
		{
			if (world == null) world = WorldConsumerUtility.FindWorld(this);
		}

		protected void LateUpdate()
		{
			world.Confine(transform);
		}
	}
}
