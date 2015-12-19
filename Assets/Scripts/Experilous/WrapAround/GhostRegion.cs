using UnityEngine;

namespace Experilous.WrapAround
{
	public abstract class GhostRegion
	{
		public abstract Viewport viewport { get; }
		public abstract bool isActive { get; set; }

		public abstract bool HasGhost(Element element);
		public abstract bool HasGhost(int instanceId);
		public abstract void AddElement(Element element);
		public abstract void AddElement(int instanceId);
		public abstract void RemoveElement(Element element);
		public abstract void RemoveElement(int instanceId);
		public abstract void Transform(ref Vector3 position, ref Quaternion rotation);
		public abstract void Transform(Transform sourceTransform, Transform targetTransform);
		public abstract void Transform(Rigidbody sourceRigidbody, Rigidbody targetRigidbody);
		public abstract Matrix4x4 transformation { get; }
	}
}
