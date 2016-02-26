/******************************************************************************\
 *  Copyright (C) 2016 Experilous <againey@experilous.com>
 *  
 *  This file is subject to the terms and conditions defined in the file
 *  'Assets/Plugins/Experilous/License.txt', which is a part of this package.
 *
\******************************************************************************/

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

		public override bool IsVisible(Viewport viewport, Rigidbody2D rigidbody)
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

		public override bool IsCollidable(World world, Rigidbody2D rigidbody)
		{
			return world.IsCollidable(rigidbody.position, _cachedScaledRadius);
		}

		public override bool Intersects(World world, float buffer = 0f)
		{
			return world.Intersects(transform.position, _cachedScaledRadius + buffer);
		}

		public override bool Intersects(World world, Vector3 position, float buffer = 0f)
		{
			return world.Intersects(position, _cachedScaledRadius + buffer);
		}

		public override bool Intersects(World world, Vector3 position, Quaternion rotation, float buffer = 0f)
		{
			return world.Intersects(position, _cachedScaledRadius + buffer);
		}

		public override bool Intersects(World world, Transform transform, float buffer = 0f)
		{
			return world.Intersects(transform.position, _cachedScaledRadius + buffer);
		}

		public override bool Intersects(World world, Rigidbody rigidbody, float buffer = 0f)
		{
			return world.Intersects(rigidbody.position, _cachedScaledRadius + buffer);
		}

		public override bool Intersects(World world, Rigidbody2D rigidbody, float buffer = 0f)
		{
			return world.Intersects(rigidbody.position, _cachedScaledRadius + buffer);
		}

		public override bool ContainedBy(World world, float buffer = 0f)
		{
			return world.Contains(transform.position, _cachedScaledRadius + buffer);
		}

		public override bool ContainedBy(World world, Vector3 position, float buffer = 0f)
		{
			return world.Contains(position, _cachedScaledRadius + buffer);
		}

		public override bool ContainedBy(World world, Vector3 position, Quaternion rotation, float buffer = 0f)
		{
			return world.Contains(position, _cachedScaledRadius + buffer);
		}

		public override bool ContainedBy(World world, Transform transform, float buffer = 0f)
		{
			return world.Contains(transform.position, _cachedScaledRadius + buffer);
		}

		public override bool ContainedBy(World world, Rigidbody rigidbody, float buffer = 0f)
		{
			return world.Contains(rigidbody.position, _cachedScaledRadius + buffer);
		}

		public override bool ContainedBy(World world, Rigidbody2D rigidbody, float buffer = 0f)
		{
			return world.Contains(rigidbody.position, _cachedScaledRadius + buffer);
		}

#if UNITY_EDITOR
		protected new void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireSphere(transform.position, scaledRadius);
		}
#endif
	}
}
