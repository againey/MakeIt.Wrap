using UnityEngine;

namespace Experilous.WrapAround
{
	public class RhomboidGhostRegion : GhostRegion
	{
		private Vector3 _offset;
		private bool _isActive = true;

		public RhomboidGhostRegion(Vector3 offset)
		{
			_offset = offset;
		}

		public override bool isActive { get { return _isActive; } set { _isActive = value; } }

		public override void Transform(ref Vector3 position, ref Quaternion rotation)
		{
			position += _offset;
		}

		public override void Transform(Transform sourceTransform, Transform targetTransform)
		{
			targetTransform.position = sourceTransform.position + _offset;
			targetTransform.rotation = sourceTransform.rotation;
		}

		public override void Transform(Rigidbody sourceRigidbody, Rigidbody targetRigidbody)
		{
			targetRigidbody.position = sourceRigidbody.position + _offset;
			targetRigidbody.rotation = sourceRigidbody.rotation;
		}

		public override Matrix4x4 transformation { get { return Matrix4x4.TRS(_offset, Quaternion.identity, Vector3.one); } }
	}
}
