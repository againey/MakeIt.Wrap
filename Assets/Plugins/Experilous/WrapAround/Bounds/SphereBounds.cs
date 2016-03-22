/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	public abstract class SphereBounds : ElementBounds
	{
		public Sphere sphere;

		protected static TBounds CreateDerived<TBounds>(GameObject gameObject, Sphere sphere) where TBounds : SphereBounds
		{
			var bounds = gameObject.AddComponent<TBounds>();
			bounds.sphere = sphere;
			bounds.hideFlags = HideFlags.HideInInspector;
			return bounds;
		}

		public static SphereBounds Create(GameObject gameObject, Sphere sphere, bool fixedScale, bool fixedRotation)
		{
			if (fixedScale)
			{
				if (fixedRotation)
				{
					return FixedSphereBounds.Create(gameObject, sphere);
				}
				else
				{
					return RotatableSphereBounds.Create(gameObject, sphere);
				}
			}
			else
			{
				if (fixedRotation)
				{
					return ScalableSphereBounds.Create(gameObject, sphere);
				}
				else
				{
					return DynamicSphereBounds.Create(gameObject, sphere);
				}
			}
		}

		#region Transform() & InverseTransform()

		public static Sphere InverseTransform(Transform transform, Sphere sphere, bool fixedScale, bool fixedRotation)
		{
			if (fixedScale)
			{
				if (fixedRotation)
				{
					return FixedSphereBounds.InverseTransform(transform, sphere);
				}
				else
				{
					return RotatableSphereBounds.InverseTransform(transform, sphere);
				}
			}
			else
			{
				if (fixedRotation)
				{
					return ScalableSphereBounds.InverseTransform(transform, sphere);
				}
				else
				{
					return DynamicSphereBounds.InverseTransform(transform, sphere);
				}
			}
		}

		public abstract Sphere Transform(Transform transform);
		public abstract Sphere Transform(Transform transform, GhostRegion ghostRegion);

		public Sphere Transform()
		{
			return Transform(transform);
		}

		public Sphere Transform(GhostRegion ghostRegion)
		{
			return Transform(transform, ghostRegion);
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
