/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using Experilous.Numerics;

namespace Experilous.MakeItWrap
{
	public class DynamicSphereBounds : SphereBounds
	{
		public DynamicSphereBounds(Sphere sphere) : base(sphere)
		{
		}

		#region private Transform()

		private Sphere Transform(Transform transform)
		{
			var scale = transform.lossyScale;
			return new Sphere(transform.TransformPoint(sphere.center), sphere.radius * scale.MaxAbsComponent());
		}

		private Sphere Transform(Transform transform, GhostRegion ghostRegion)
		{
			var scale = transform.lossyScale;
			return ghostRegion.Transform(new Sphere(transform.TransformPoint(sphere.center), sphere.radius * scale.MaxAbsComponent()));
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
			var transformedSphere = Transform(transform);
			Gizmos.color = color;
			Gizmos.DrawWireSphere(transformedSphere.center, transformedSphere.radius);
		}

		public override void DrawGizmosSelected(Transform transform, GhostRegion ghostRegion, Color color)
		{
			var transformedSphere = Transform(transform, ghostRegion);
			Gizmos.color = color;
			Gizmos.DrawWireSphere(transformedSphere.center, transformedSphere.radius);
		}
#endif

		#endregion
	}
}
