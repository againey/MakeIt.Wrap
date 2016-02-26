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
	/// <summary>
	/// Abstract representation of a bounding volume that can perform the first phase of
	/// a double-dispatch visibility or collision check with a viewport or world.
	/// </summary>
	/// <seealso cref="Viewport"/>
	/// <seealso cref="World"/>
	public abstract class AbstractBounds : MonoBehaviour
	{
		public abstract bool IsVisible(Viewport viewport);
		public abstract bool IsVisible(Viewport viewport, Vector3 position);
		public abstract bool IsVisible(Viewport viewport, Vector3 position, Quaternion rotation);
		public abstract bool IsVisible(Viewport viewport, Transform tranform);
		public abstract bool IsVisible(Viewport viewport, Rigidbody rigidbody);
		public abstract bool IsVisible(Viewport viewport, Rigidbody2D rigidbody);
		public abstract bool IsCollidable(World world);
		public abstract bool IsCollidable(World world, Vector3 position);
		public abstract bool IsCollidable(World world, Vector3 position, Quaternion rotation);
		public abstract bool IsCollidable(World world, Transform tranform);
		public abstract bool IsCollidable(World world, Rigidbody rigidbody);
		public abstract bool IsCollidable(World world, Rigidbody2D rigidbody);
		public abstract bool Intersects(World world, float buffer = 0f);
		public abstract bool Intersects(World world, Vector3 position, float buffer = 0f);
		public abstract bool Intersects(World world, Vector3 position, Quaternion rotation, float buffer = 0f);
		public abstract bool Intersects(World world, Transform tranform, float buffer = 0f);
		public abstract bool Intersects(World world, Rigidbody rigidbody, float buffer = 0f);
		public abstract bool Intersects(World world, Rigidbody2D rigidbody, float buffer = 0f);
		public abstract bool ContainedBy(World world, float buffer = 0f);
		public abstract bool ContainedBy(World world, Vector3 position, float buffer = 0f);
		public abstract bool ContainedBy(World world, Vector3 position, Quaternion rotation, float buffer = 0f);
		public abstract bool ContainedBy(World world, Transform tranform, float buffer = 0f);
		public abstract bool ContainedBy(World world, Rigidbody rigidbody, float buffer = 0f);
		public abstract bool ContainedBy(World world, Rigidbody2D rigidbody, float buffer = 0f);
	}
}
