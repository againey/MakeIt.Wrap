using System;
using System.Collections.Generic;
using UnityEngine;

namespace Experilous.WrapAround
{
	public abstract class AxisAligned2DViewport : AxisAlignedViewport
	{
		private object _ghostRegions;
		private IEnumerable<GhostRegion> _enumerableGhostRegions;

		protected Vector3 _min;
		protected Vector3 _max;

		public override Vector3 min { get { return _min; } }
		public override Vector3 max { get { return _max; } }

		protected void Start()
		{
			_ghostRegions = world.InstantiateGhostRegions();
			RecalculateVisibleGhostRegions();
		}

		public override IEnumerable<GhostRegion> visibleGhostRegions { get { return _enumerableGhostRegions; } }

		public override bool IsVisible(Vector3 position)
		{
			return
				position.x >= min.x &&
				position.y >= min.y &&
				position.z >= min.z &&
				position.x < max.x &&
				position.y < max.y &&
				position.z < max.z;
		}

		public override bool IsVisible(Vector3 position, float radius)
		{
			return
				position.x + radius >= min.x &&
				position.y + radius >= min.y &&
				position.z + radius >= min.z &&
				position.x - radius < max.x &&
				position.y - radius < max.y &&
				position.z - radius < max.z;
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
