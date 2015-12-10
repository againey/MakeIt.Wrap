using UnityEngine;

namespace Experilous.WrapAround
{
	public class RigidBodyElementGhost : ElementGhost
	{
		protected Rigidbody _rigidBody;
		protected Rigidbody _originalRigidBody;

		protected void Start()
		{
			_rigidBody = GetComponent<Rigidbody>();
			_originalRigidBody = Original.GetComponent<Rigidbody>();
		}

		protected override void UpdateFromOriginal()
		{
			Vector3 position = _originalRigidBody.position;
			Quaternion rotation = _originalRigidBody.rotation;
			Region.Transform(ref position, ref rotation);
			_rigidBody.position = position;
			_rigidBody.rotation = rotation;
			_rigidBody.angularVelocity = _originalRigidBody.angularVelocity;
			_rigidBody.velocity = _originalRigidBody.velocity;
		}
	}
}
