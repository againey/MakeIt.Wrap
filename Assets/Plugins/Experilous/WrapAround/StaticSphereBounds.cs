using UnityEngine;

namespace Experilous.WrapAround
{
	public class StaticSphereBounds : SphereBounds
	{
		protected float _cachedScaledRadius;

		protected new void Start()
		{
			base.Start();

			_cachedScaledRadius = scaledRadius;
		}
		
		public override bool IsVisible(Viewport viewport)
		{
			return viewport.IsVisible(transform.position, _cachedScaledRadius);
		}

		public override bool IsVisible(Viewport viewport, Vector3 position)
		{
			return viewport.IsVisible(position, _cachedScaledRadius);
		}

		public override bool IsVisible(Viewport viewport, Vector3 position, Quaternion rotation)
		{
			return viewport.IsVisible(position, _cachedScaledRadius);
		}

		public override bool IsVisible(Viewport viewport, Transform transform)
		{
			return viewport.IsVisible(transform.position, _cachedScaledRadius);
		}

		public override bool IsVisible(Viewport viewport, Rigidbody rigidbody)
		{
			return viewport.IsVisible(rigidbody.position, _cachedScaledRadius);
		}

		public override bool IsCollidable(World world)
		{
			return world.IsCollidable(transform.position, _cachedScaledRadius);
		}

		public override bool IsCollidable(World world, Vector3 position)
		{
			return world.IsCollidable(position, _cachedScaledRadius);
		}

		public override bool IsCollidable(World world, Vector3 position, Quaternion rotation)
		{
			return world.IsCollidable(position, _cachedScaledRadius);
		}

		public override bool IsCollidable(World world, Transform transform)
		{
			return world.IsCollidable(transform.position, _cachedScaledRadius);
		}

		public override bool IsCollidable(World world, Rigidbody rigidbody)
		{
			return world.IsCollidable(rigidbody.position, _cachedScaledRadius);
		}
	}
}
