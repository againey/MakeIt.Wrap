/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.MakeItWrap
{
	public class LocalOriginBounds : ElementBounds
	{
		public LocalOriginBounds()
		{
		}

		#region private Transform()

		private Vector3 Transform(Transform transform)
		{
			return transform.position;
		}

		private Vector3 Transform(Transform transform, GhostRegion ghostRegion)
		{
			return ghostRegion.Transform(transform.position);
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
