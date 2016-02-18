﻿using UnityEngine;

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
		public abstract bool IsCollidable(World world);
		public abstract bool IsCollidable(World world, Vector3 position);
		public abstract bool IsCollidable(World world, Vector3 position, Quaternion rotation);
		public abstract bool IsCollidable(World world, Transform tranform);
		public abstract bool IsCollidable(World world, Rigidbody rigidbody);
	}
}