using System;
using System.Collections.Generic;
using UnityEngine;

namespace Experilous.WrapAround
{
	public abstract class AxisAligned2DViewport : AxisAlignedViewport
	{
		protected Vector3 _min;
		protected Vector3 _max;

		public override Vector3 min { get { return _min; } }
		public override Vector3 max { get { return _max; } }

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

		public override bool IsVisible(PointElement element)
		{
			return IsVisible(element.transform.position);
		}

		public override bool IsVisible(SphereElement element)
		{
			return IsVisible(element.transform.position, element.radius);
		}

		protected override IEnumerable<GhostRegion> GetGhostRegions(object ghostRegions)
		{
			return world.GetGhostRegions(this, ghostRegions);
		}
	}
}
