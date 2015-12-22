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

			if (!region.isActive || !original.IsVisible(this))
			{
				region.RemoveElement(original.GetInstanceID());
				Destroy(gameObject);
			}
		}
	}
}
