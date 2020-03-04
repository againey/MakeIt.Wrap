/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using Experilous.Numerics;

namespace Experilous.MakeItWrap
{
	public class ScalableAxisAlignedBoxBounds : AxisAlignedBoxBounds
	{
		public ScalableAxisAlignedBoxBounds(Bounds box) : base(box)
		{
		}

		#region private Transform()

		private Bounds Transform(Transform transform)
		{
			var scale = transform.lossyScale;
			return new Bounds(transform.position + box.center.MultiplyComponents(scale), box.size.MultiplyComponents(scale));
		}

		private Bounds Transform(Transform transform, GhostRegion ghostRegion)
		{
			var scale = transform.lossyScale;
			return ghostRegion.Transform(new Bounds(transform.position + box.center.MultiplyComponents(scale), box.size.MultiplyComponents(scale)));
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
			var transformedBox = Transform(transform);
			Gizmos.color = color;
			Gizmos.DrawWireCube(transformedBox.center, transformedBox.size);
		}

		public override void DrawGizmosSelected(Transform transform, GhostRegion ghostRegion, Color color)
		{
			var transformedBox = Transform(transform, ghostRegion);
			Gizmos.color = color;
			Gizmos.DrawWireCube(transformedBox.center, transformedBox.size);
		}
#endif

		#endregion
	}
}
