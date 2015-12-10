using System;
using System.Collections.Generic;
using UnityEngine;

namespace Experilous.WrapAround
{
	public class AxisAligned2DViewport : Viewport
	{
		public Transform minimumCorner;
		public Transform maximumCorner;
		public float BufferThickness;

		private Vector3 _min;
		private Vector3 _max;

		public Vector3 min { get { return _min; } }
		public Vector3 max { get { return _max; } }

		public override IEnumerable<GhostRegion> visibleGhostRegions
		{
			get
			{
				return World.GetVisibleGhostRegions(this);
			}
		}

		protected void Awake()
		{
			_min = minimumCorner.position - new Vector3(BufferThickness, BufferThickness, BufferThickness);
			_max = maximumCorner.position + new Vector3(BufferThickness, BufferThickness, BufferThickness);
		}

		public override bool IsVisible(PointElement element)
		{
			return IsVisible(element.transform.position);
		}

		public override bool IsVisible(Vector3 position)
		{
			return
				position.x >= _min.x &&
				position.y >= _min.y &&
				position.z >= _min.z &&
				position.x <= _max.x &&
				position.y <= _max.y &&
				position.z <= _max.z;
		}
	}
}
