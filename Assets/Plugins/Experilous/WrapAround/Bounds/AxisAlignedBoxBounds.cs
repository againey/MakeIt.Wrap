/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	public abstract class AxisAlignedBoxBounds : ElementBounds
	{
		public Bounds box;

		protected static TBounds CreateDerived<TBounds>(GameObject gameObject, Bounds box) where TBounds : AxisAlignedBoxBounds
		{
			var bounds = gameObject.AddComponent<TBounds>();
			bounds.box = box;
			bounds.hideFlags = HideFlags.HideInInspector;
			return bounds;
		}

		public static AxisAlignedBoxBounds Create(GameObject gameObject, Bounds box, bool fixedScale)
		{
			if (fixedScale)
			{
				return FixedAxisAlignedBoxBounds.Create(gameObject, box);
			}
			else
			{
				return ScalableAxisAlignedBoxBounds.Create(gameObject, box);
			}
		}

		#region Transform() & InverseTransform()

		public static Bounds InverseTransform(Transform transform, Bounds box, bool fixedScale)
		{
			if (fixedScale)
			{
				return FixedAxisAlignedBoxBounds.InverseTransform(transform, box);
			}
			else
			{
				return ScalableAxisAlignedBoxBounds.InverseTransform(transform, box);
			}
		}

		public abstract Bounds Transform(Transform transform);
		public abstract Bounds Transform(Transform transform, GhostRegion ghostRegion);

		public Bounds Transform()
		{
			return Transform(transform);
		}

		public Bounds Transform(GhostRegion ghostRegion)
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
