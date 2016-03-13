/******************************************************************************\
 *  Copyright (C) 2016 Experilous <againey@experilous.com>
 *  
 *  This file is subject to the terms and conditions defined in the file
 *  'Assets/Plugins/Experilous/License.txt', which is a part of this package.
 *
\******************************************************************************/

using UnityEngine;
using System.Collections.Generic;

namespace Experilous.WrapAround
{
	/// <summary>
	/// A viewport whose visible area is derived directly from a camera's view frustum.
	/// </summary>
	[RequireComponent(typeof(Camera))]
	public class CameraViewport : Viewport
	{
		private Camera _camera;
		private Plane[] _frustumPlanes;
		private List<GhostRegion> _visibleGhostRegions = new List<GhostRegion>();

		protected new void Start()
		{
			_camera = GetComponent<Camera>();
			base.Start();
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
			_frustumPlanes = UnityEngine.GeometryUtility.CalculateFrustumPlanes(_camera);
		}

		public override bool IsVisible(Vector3 position)
		{
			foreach (var plane in _frustumPlanes)
			{
				if (plane.GetDistanceToPoint(position) < 0f) return false;
			}
			return true;
		}

		public override bool IsVisible(Sphere sphere)
		{
			foreach (var plane in _frustumPlanes)
			{
				if (plane.GetDistanceToPoint(sphere.center) < -sphere.radius) return false;
			}
			return true;
		}

		public override bool IsVisible(Bounds box)
		{
			return UnityEngine.GeometryUtility.TestPlanesAABB(_frustumPlanes, box);
		}
	}
}
