/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	public abstract class PointBounds : ElementBounds
	{
		public Vector3 position;

		protected static TBounds CreateDerived<TBounds>(GameObject gameObject, Vector3 position) where TBounds : PointBounds
		{
			var bounds = gameObject.AddComponent<TBounds>();
			bounds.position = position;
			bounds.hideFlags = HideFlags.HideInInspector;
			return bounds;
		}

		public static PointBounds Create(GameObject gameObject, Vector3 position, bool fixedScale, bool fixedRotation)
		{
			if (fixedScale)
			{
				if (fixedRotation)
				{
					return FixedPointBounds.Create(gameObject, position);
				}
				else
				{
					return RotatablePointBounds.Create(gameObject, position);
				}
			}
			else
			{
				if (fixedRotation)
				{
					return ScalablePointBounds.Create(gameObject, position);
				}
				else
				{
					return DynamicPointBounds.Create(gameObject, position);
				}
			}
		}

		#region Transform() & InverseTransform()

		public static Vector3 InverseTransform(Transform transform, Vector3 position, bool fixedScale, bool fixedRotation)
		{
			if (fixedScale)
			{
				if (fixedRotation)
				{
					return FixedPointBounds.InverseTransform(transform, position);
				}
				else
				{
					return RotatablePointBounds.InverseTransform(transform, position);
				}
			}
			else
			{
				if (fixedRotation)
				{
					return ScalablePointBounds.InverseTransform(transform, position);
				}
				else
				{
					return DynamicPointBounds.InverseTransform(transform, position);
				}
			}
		}

		public abstract Vector3 Transform(Transform transform);
		public abstract Vector3 Transform(Transform transform, GhostRegion ghostRegion);

		public Vector3 Transform()
		{
			return Transform(transform);
		}

		public Vector3 Transform(GhostRegion ghostRegion)
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
