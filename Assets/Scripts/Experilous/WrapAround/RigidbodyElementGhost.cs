using UnityEngine;

namespace Experilous.WrapAround
{
	[RequireComponent(typeof(Rigidbody))]
	public class RigidbodyElementGhost : MonoBehaviour
	{
		public Rigidbody original;
		public GhostRegion region;

		protected Rigidbody _rigidbody;

		protected void Start()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		protected void FixedUpdate()
		{
			region.Transform(original, _rigidbody);
			_rigidbody.angularVelocity = original.angularVelocity;
			_rigidbody.velocity = original.velocity;
		}
	}
}
