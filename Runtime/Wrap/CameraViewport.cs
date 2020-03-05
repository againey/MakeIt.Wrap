/******************************************************************************\
* Copyright Andy Gainey                                                        *
*                                                                              *
* Licensed under the Apache License, Version 2.0 (the "License");              *
* you may not use this file except in compliance with the License.             *
* You may obtain a copy of the License at                                      *
*                                                                              *
*     http://www.apache.org/licenses/LICENSE-2.0                               *
*                                                                              *
* Unless required by applicable law or agreed to in writing, software          *
* distributed under the License is distributed on an "AS IS" BASIS,            *
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.     *
* See the License for the specific language governing permissions and          *
* limitations under the License.                                               *
\******************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using MakeIt.Numerics;

namespace MakeIt.Wrap
{
	/// <summary>
	/// A viewport whose visible area is derived directly from a camera's view frustum.
	/// </summary>
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("MakeIt.Wrap/Viewports/Camera Viewport")]
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
