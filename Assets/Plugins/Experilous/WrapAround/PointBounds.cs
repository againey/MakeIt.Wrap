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

		public override bool IsVisible(Viewport viewport, Rigidbody2D rigidbody)
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

		public override bool IsCollidable(World world, Rigidbody2D rigidbody)
		{
			return world.IsCollidable(rigidbody.position);
		}

		public override bool Intersects(World world, float buffer = 0f)
		{
			return world.Intersects(transform.position, buffer);
		}

		public override bool Intersects(World world, Vector3 position, float buffer = 0f)
		{
			return world.Intersects(position, buffer);
		}

		public override bool Intersects(World world, Vector3 position, Quaternion rotation, float buffer = 0f)
		{
			return world.Intersects(position, buffer);
		}

		public override bool Intersects(World world, Transform transform, float buffer = 0f)
		{
			return world.Intersects(transform.position, buffer);
		}

		public override bool Intersects(World world, Rigidbody rigidbody, float buffer = 0f)
		{
			return world.Intersects(rigidbody.position, buffer);
		}

		public override bool Intersects(World world, Rigidbody2D rigidbody, float buffer = 0f)
		{
			return world.Intersects(rigidbody.position, buffer);
		}

		public override bool ContainedBy(World world, float buffer = 0f)
		{
			return world.Contains(transform.position, buffer);
		}

		public override bool ContainedBy(World world, Vector3 position, float buffer = 0f)
		{
			return world.Contains(position, buffer);
		}

		public override bool ContainedBy(World world, Vector3 position, Quaternion rotation, float buffer = 0f)
		{
			return world.Contains(position, buffer);
		}

		public override bool ContainedBy(World world, Transform transform, float buffer = 0f)
		{
			return world.Contains(transform.position, buffer);
		}

		public override bool ContainedBy(World world, Rigidbody rigidbody, float buffer = 0f)
		{
			return world.Contains(rigidbody.position, buffer);
		}

		public override bool ContainedBy(World world, Rigidbody2D rigidbody, float buffer = 0f)
		{
			return world.Contains(rigidbody.position, buffer);
		}
	}
}
