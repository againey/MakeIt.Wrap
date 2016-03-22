/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	public class LocalOriginBounds : ElementBounds
	{
		public static LocalOriginBounds Create(GameObject gameObject)
		{
			var bounds = gameObject.AddComponent<LocalOriginBounds>();
			bounds.hideFlags = HideFlags.HideInInspector;
			return bounds;
		}

		#region Transform()

		public Vector3 Transform(Transform transform)
		{
			return transform.position;
		}

		public Vector3 Transform(Transform transform, GhostRegion ghostRegion)
		{
			return ghostRegion.Transform(transform.position);
		}

		public Vector3 Transform()
		{
			return transform.position;
		}

		public Vector3 Transform(GhostRegion ghostRegion)
		{
			return ghostRegion.Transform(transform.position);
		}

		#endregion

		#region IsVisible()

		public override bool IsVisible(Viewport viewport, Transform transform)
		{
			return viewport.IsVisible(Transform(transform));
		}

		public override bool IsVisible(Viewport viewport, Transform transform, GhostRegion ghostRegion)
		{
			return viewport.IsVisible(Transform(transform, ghostRegion));
		}

		#endregion

		#region IsCollidable()

		public override bool IsCollidable(World world, Transform transform)
		{
			return world.IsCollidable(Transform(transform));
		}

		public override bool IsCollidable(World world, Transform transform, GhostRegion ghostRegion)
		{
			return world.IsCollidable(Transform(transform, ghostRegion));
		}

		#endregion

		#region Intersects()

		public override bool Intersects(World world, Transform transform, float buffer = 0f)
		{
			return world.Intersects(Transform(transform), buffer);
		}

		public override bool Intersects(World world, Transform transform, GhostRegion ghostRegion, float buffer = 0f)
		{
			return world.Intersects(Transform(transform, ghostRegion), buffer);
		}

		#endregion

		#region ContainedBy()

		public override bool ContainedBy(World world, Transform transform, float buffer = 0f)
		{
			return world.Contains(Transform(transform), buffer);
		}

		public override bool ContainedBy(World world, Transform transform, GhostRegion ghostRegion, float buffer = 0f)
		{
			return world.Contains(Transform(transform, ghostRegion), buffer);
		}

		#endregion
	}
}
