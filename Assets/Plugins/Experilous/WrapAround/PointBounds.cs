using UnityEngine;

namespace Experilous.WrapAround
{
	public class PointBounds : AbstractBounds
	{
		public override bool IsVisible(Viewport viewport)
		{
			return viewport.IsVisible(transform.position);
		}

		public override bool IsVisible(Viewport viewport, Vector3 position)
		{
			return viewport.IsVisible(position);
		}

		public override bool IsVisible(Viewport viewport, Vector3 position, Quaternion rotation)
		{
			return viewport.IsVisible(position);
		}

		public override bool IsVisible(Viewport viewport, Transform transform)
		{
			return viewport.IsVisible(transform.position);
		}

		public override bool IsVisible(Viewport viewport, Rigidbody rigidbody)
		{
			return viewport.IsVisible(rigidbody.position);
		}

		public override bool IsCollidable(World world)
		{
			return world.IsCollidable(transform.position);
		}

		public override bool IsCollidable(World world, Vector3 position)
		{
			return world.IsCollidable(position);
		}

		public override bool IsCollidable(World world, Vector3 position, Quaternion rotation)
		{
			return world.IsCollidable(position);
		}

		public override bool IsCollidable(World world, Transform transform)
		{
			return world.IsCollidable(transform.position);
		}

		public override bool IsCollidable(World world, Rigidbody rigidbody)
		{
			return world.IsCollidable(rigidbody.position);
		}
	}
}
