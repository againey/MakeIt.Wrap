using UnityEngine;

namespace Experilous.WrapAround
{
	[RequireComponent(typeof(Rigidbody))]
	public class RigidbodyElementGhost : MonoBehaviour
	{
		public RigidbodyElement original;
		public GhostRegion region;

		protected Rigidbody _rigidbody;
		protected Rigidbody _originalRigidbody;

		public new Rigidbody rigidbody { get { return _rigidbody; } }

		protected void Start()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_originalRigidbody = original.GetComponent<Rigidbody>();
		}

		protected void FixedUpdate()
		{
			region.Transform(_originalRigidbody, _rigidbody);
			_rigidbody.angularVelocity = _originalRigidbody.angularVelocity;
			_rigidbody.velocity = _originalRigidbody.velocity;

			if (!region.isActive || !original.IsCollidable(this))
			{
				region.RemoveElement(original.GetInstanceID());
				Destroy(gameObject);
			}
		}
	}
}
