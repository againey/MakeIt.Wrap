using UnityEngine;

namespace Experilous.WrapAround
{
	public class RigidbodyElementGhost : MechanicalElementGhost
	{
		protected Rigidbody _rigidbody;
		protected Rigidbody _originalRigidbody;

		protected void Start()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_originalRigidbody = original.GetComponent<Rigidbody>();
		}

		public override void UpdateFromOriginal()
		{
			Vector3 position = _originalRigidbody.position;
			Quaternion rotation = _originalRigidbody.rotation;
			region.Transform(ref position, ref rotation);
			_rigidbody.position = position;
			_rigidbody.rotation = rotation;
			_rigidbody.angularVelocity = _originalRigidbody.angularVelocity;
			_rigidbody.velocity = _originalRigidbody.velocity;
		}
	}
}
