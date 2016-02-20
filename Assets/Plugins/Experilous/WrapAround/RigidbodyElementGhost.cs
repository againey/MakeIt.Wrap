using UnityEngine;

namespace Experilous.WrapAround
{
	[RequireComponent(typeof(Rigidbody))]
	public class RigidbodyElementGhost : Ghost<RigidbodyElement, RigidbodyElementGhost>
	{
		protected Rigidbody _rigidbody;
		protected Rigidbody _originalRigidbody;

		public new Rigidbody rigidbody { get { return _rigidbody; } }

		protected new void Start()
		{
			base.Start();

			_rigidbody = GetComponent<Rigidbody>();
			_originalRigidbody = original.GetComponent<Rigidbody>();
		}

		protected void FixedUpdate()
		{
			region.Transform(_originalRigidbody, _rigidbody);
			_rigidbody.angularVelocity = _originalRigidbody.angularVelocity;
			_rigidbody.velocity = _originalRigidbody.velocity;

			if (region == null || !region.isActive || !original.IsCollidable(this))
			{
				Destroy();
			}
		}
	}
}
