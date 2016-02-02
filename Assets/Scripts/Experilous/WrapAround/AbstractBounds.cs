using UnityEngine;

namespace Experilous.WrapAround
{
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
