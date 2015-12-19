using UnityEngine;
using System.Collections.Generic;

namespace Experilous.WrapAround
{
	public abstract class Viewport : MonoBehaviour
	{
		public World world;

		public abstract IEnumerable<GhostRegion> visibleGhostRegions { get; }

		public abstract void RecalculateVisibleGhostRegions();

		public abstract bool IsVisible(Vector3 position);
		public abstract bool IsVisible(Vector3 position, float radius);

		public abstract bool IsVisible(PointElement element);
		public abstract bool IsVisible(SphereElement element);
	}
}
