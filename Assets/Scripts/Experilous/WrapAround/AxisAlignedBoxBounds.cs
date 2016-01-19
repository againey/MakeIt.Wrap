using UnityEngine;

namespace Experilous.WrapAround
{
	public class AxisAlignedBoxBounds : AbstractBounds
	{
		public Bounds box;

		public override bool IsVisible(Viewport viewport)
		{
			return viewport.IsVisible(transform.position, box);
		}

		public override bool IsVisible(Viewport viewport, Vector3 position)
		{
			return viewport.IsVisible(position, box);
		}

		public override bool IsVisible(Viewport viewport, Vector3 position, Quaternion rotation)
		{
			return viewport.IsVisible(position, box);
		}

		public override bool IsVisible(Viewport viewport, Transform transform)
		{
			return viewport.IsVisible(transform.position, box);
		}

		public override bool IsVisible(Viewport viewport, Rigidbody rigidbody)
		{
			return viewport.IsVisible(rigidbody.position, box);
		}

		public override bool IsCollidable(World world)
		{
			return world.IsCollidable(transform.position, box);
		}

		public override bool IsCollidable(World world, Vector3 position)
		{
			return world.IsCollidable(position, box);
		}

		public override bool IsCollidable(World world, Vector3 position, Quaternion rotation)
		{
			return world.IsCollidable(position, box);
		}

		public override bool IsCollidable(World world, Transform transform)
		{
			return world.IsCollidable(transform.position, box);
		}

		public override bool IsCollidable(World world, Rigidbody rigidbody)
		{
			return world.IsCollidable(rigidbody.position, box);
		}

#if UNITY_EDITOR
		protected void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireCube(transform.position + box.center, box.size);
		}
#endif
	}
}
