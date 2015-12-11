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

		private Vector3 _bufferMin;
		private Vector3 _bufferMax;

		public Vector3 bufferMin { get { return _bufferMin; } }
		public Vector3 bufferMax { get { return _bufferMax; } }

		public override IEnumerable<GhostRegion> visibleGhostRegions
		{
			get
			{
				return World.GetVisibleGhostRegions(this);
			}
		}

		protected void Awake()
		{
			_min = minimumCorner.position;
			_max = maximumCorner.position;
			_bufferMin = _min - new Vector3(BufferThickness, BufferThickness, BufferThickness);
			_bufferMax = _max + new Vector3(BufferThickness, BufferThickness, BufferThickness);
		}

		public override bool IsVisible(Vector3 position)
		{
			return
				position.x >= _min.x &&
				position.y >= _min.y &&
				position.z >= _min.z &&
				position.x < _max.x &&
				position.y < _max.y &&
				position.z < _max.z;
		}

		public override bool IsVisible(Vector3 position, float radius)
		{
			return
				position.x + radius >= _min.x &&
				position.y + radius >= _min.y &&
				position.z + radius >= _min.z &&
				position.x - radius < _max.x &&
				position.y - radius < _max.y &&
				position.z - radius < _max.z;
		}

		public override bool IsVisible(PointElement element)
		{
			return IsVisible(element.transform.position);
		}

		public override bool IsVisible(SphereElement element)
		{
			return IsVisible(element.transform.position, element.radius);
		}
	}
}
