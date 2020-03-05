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

namespace MakeIt.Wrap
{
	public class FixedPointBounds : PointBounds
	{
		public FixedPointBounds(Vector3 position) : base(position)
		{
		}

		#region private Transform()

		private Vector3 Transform(Transform transform)
		{
			return transform.position + position;
		}

		private Vector3 Transform(Transform transform, GhostRegion ghostRegion)
		{
			return ghostRegion.Transform(transform.position + position);
		}

		#endregion

		#region public IsVisible()

		public override bool IsVisible(Viewport viewport, Transform transform)
		{
			return viewport.IsVisible(Transform(transform));
		}

		public override bool IsVisible(Viewport viewport, Transform transform, GhostRegion ghostRegion)
		{
			return viewport.IsVisible(Transform(transform, ghostRegion));
		}

		#endregion

		#region public IsCollidable()

		public override bool IsCollidable(World world, Transform transform)
		{
			return world.IsCollidable(Transform(transform));
		}

		public override bool IsCollidable(World world, Transform transform, GhostRegion ghostRegion)
		{
			return world.IsCollidable(Transform(transform, ghostRegion));
		}

		#endregion

		#region public Intersects()

		public override bool Intersects(World world, Transform transform, float buffer = 0f)
		{
			return world.Intersects(Transform(transform), buffer);
		}

		public override bool Intersects(World world, Transform transform, GhostRegion ghostRegion, float buffer = 0f)
		{
			return world.Intersects(Transform(transform, ghostRegion), buffer);
		}

		#endregion

		#region public ContainedBy()

		public override bool ContainedBy(World world, Transform transform, float buffer = 0f)
		{
			return world.Contains(Transform(transform), buffer);
		}

		public override bool ContainedBy(World world, Transform transform, GhostRegion ghostRegion, float buffer = 0f)
		{
			return world.Contains(Transform(transform, ghostRegion), buffer);
		}

		#endregion

		#region [Editor] DrawGizmosSelected()

#if UNITY_EDITOR
		public override void DrawGizmosSelected(Transform transform, Color color)
		{
			var transformedPosition = Transform(transform);
			Gizmos.color = color;
			Gizmos.DrawLine(transformedPosition - new Vector3(0.25f, 0f, 0f), transformedPosition + new Vector3(0.25f, 0f, 0f));
			Gizmos.DrawLine(transformedPosition - new Vector3(0f, 0.25f, 0f), transformedPosition + new Vector3(0f, 0.25f, 0f));
			Gizmos.DrawLine(transformedPosition - new Vector3(0f, 0f, 0.25f), transformedPosition + new Vector3(0f, 0f, 0.25f));
		}

		public override void DrawGizmosSelected(Transform transform, GhostRegion ghostRegion, Color color)
		{
			var transformedPosition = Transform(transform, ghostRegion);
			Gizmos.color = color;
			Gizmos.DrawLine(transformedPosition - new Vector3(0.25f, 0f, 0f), transformedPosition + new Vector3(0.25f, 0f, 0f));
			Gizmos.DrawLine(transformedPosition - new Vector3(0f, 0.25f, 0f), transformedPosition + new Vector3(0f, 0.25f, 0f));
			Gizmos.DrawLine(transformedPosition - new Vector3(0f, 0f, 0.25f), transformedPosition + new Vector3(0f, 0f, 0.25f));
		}
#endif

		#endregion
	}
}
