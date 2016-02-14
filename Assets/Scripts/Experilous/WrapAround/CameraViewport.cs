﻿using UnityEngine;
using System.Collections.Generic;

namespace Experilous.WrapAround
{
	[RequireComponent(typeof(Camera))]
	public class CameraViewport : Viewport
	{
		private Camera _camera;
		private Plane[] _frustumPlanes;
		private List<GhostRegion> _visibleGhostRegions = new List<GhostRegion>();

		protected new void Start()
		{
			base.Start();
			_camera = GetComponent<Camera>();
		}

		public override IEnumerable<GhostRegion> visibleGhostRegions { get { return _visibleGhostRegions; } }

		public override void RecalculateVisibleGhostRegions()
		{
			if (_visibleGhostRegions != null)
			{
				foreach (var ghostRegion in _visibleGhostRegions)
				{
					ghostRegion.isActive = false;
				}
			}

			world.GetVisibleGhostRegions(_camera, _visibleGhostRegions);

			foreach (var ghostRegion in _visibleGhostRegions)
			{
				ghostRegion.isActive = true;
			}

			// TODO: Find a better design for where to put this.
			_frustumPlanes = GeometryUtility.CalculateFrustumPlanes(_camera);
		}

		public override bool IsVisible(Vector3 position)
		{
			foreach (var plane in _frustumPlanes)
			{
				if (plane.GetDistanceToPoint(position) < 0f) return false;
			}
			return true;
		}

		public override bool IsVisible(Vector3 position, float radius)
		{
			foreach (var plane in _frustumPlanes)
			{
				if (plane.GetDistanceToPoint(position) < -radius) return false;
			}
			return true;
		}

		public override bool IsVisible(Bounds box)
		{
			return GeometryUtility.TestPlanesAABB(_frustumPlanes, box);
		}

		public override bool IsVisible(Vector3 position, Bounds box)
		{
			return IsVisible(new Bounds(box.center + position, box.size));
		}
	}
}
