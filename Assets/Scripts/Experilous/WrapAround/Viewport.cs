using UnityEngine;
using System.Collections.Generic;

namespace Experilous.WrapAround
{
	public abstract class Viewport : MonoBehaviour
	{
		public World world;

		private object _ghostRegions;
		private IEnumerable<GhostRegion> _enumerableGhostRegions;

		protected void Start()
		{
			_ghostRegions = world.InstantiateGhostRegions(this);
			RecalculateVisibleGhostRegions();
		}

		public IEnumerable<GhostRegion> visibleGhostRegions { get { return _enumerableGhostRegions; } }

		public void RecalculateVisibleGhostRegions() { _enumerableGhostRegions = GetGhostRegions(_ghostRegions); }

		public abstract bool IsVisible(Vector3 position);
		public abstract bool IsVisible(Vector3 position, float radius);

		public abstract bool IsVisible(PointElement element);
		public abstract bool IsVisible(SphereElement element);

		protected abstract IEnumerable<GhostRegion> GetGhostRegions(object ghostRegions);
	}
}
