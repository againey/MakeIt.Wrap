using System;
using System.Collections.Generic;
using UnityEngine;

namespace Experilous.WrapAround
{
	public abstract class AxisAligned2DViewport : AxisAlignedViewport
	{
		private object _ghostRegions;
		private IEnumerable<GhostRegion> _enumerableGhostRegions;

		protected Bounds _box;

		public override Vector3 min { get { return _box.min; } }
		public override Vector3 max { get { return _box.max; } }

		protected void Start()
		{
			_ghostRegions = world.InstantiateGhostRegions();
			RecalculateVisibleGhostRegions();
		}

		public override IEnumerable<GhostRegion> visibleGhostRegions { get { return _enumerableGhostRegions; } }

		public override bool IsVisible(Vector3 position)
		{
			return _box.Contains(position);
		}

		public override bool IsVisible(Vector3 position, float radius)
		{
			return _box.SqrDistance(position) <= radius * radius;
		}

		public override bool IsVisible(Bounds box)
		{
			return _box.Intersects(box);
		}

		public override bool IsVisible(Vector3 position, Bounds box)
		{
			return _box.Intersects(new Bounds(box.center + position, box.size));
		}

		public override void RecalculateVisibleGhostRegions()
		{
			if (_enumerableGhostRegions != null)
			{
				foreach (var ghostRegion in _enumerableGhostRegions)
				{
					ghostRegion.isActive = false;
				}
			}

			_enumerableGhostRegions = world.GetGhostRegions(this, _ghostRegions);

			foreach (var ghostRegion in _enumerableGhostRegions)
			{
				ghostRegion.isActive = true;
			}
		}
	}
}
