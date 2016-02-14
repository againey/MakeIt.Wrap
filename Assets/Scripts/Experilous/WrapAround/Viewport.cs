using UnityEngine;
using System.Collections.Generic;

namespace Experilous.WrapAround
{
	public abstract class Viewport : MonoBehaviour, IWorldConsumer
	{
		public World world;

		public abstract IEnumerable<GhostRegion> visibleGhostRegions { get; }

		public abstract void RecalculateVisibleGhostRegions();

		public abstract bool IsVisible(Vector3 position);
		public abstract bool IsVisible(Vector3 position, float radius);
		public abstract bool IsVisible(Bounds box);
		public abstract bool IsVisible(Vector3 position, Bounds box);

		public bool hasWorld { get { return world != null ; } }
		public void SetWorld(World world) { this.world = world; }

		protected void Start()
		{
			if (world == null) world = WorldConsumerUtility.FindWorld(this);
		}
	}
}
