using UnityEngine;

namespace Experilous.WrapAround
{
	public abstract class GhostRegion
	{
		public abstract bool isActive { get; set; }

		public abstract void Transform(ref Vector3 position, ref Quaternion rotation);
		public abstract void Transform(Transform sourceTransform, Transform targetTransform);
		public abstract void Transform(Rigidbody sourceRigidbody, Rigidbody targetRigidbody);
		public abstract Matrix4x4 transformation { get; }
	}
}
