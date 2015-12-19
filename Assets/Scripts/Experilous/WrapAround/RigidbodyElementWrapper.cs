using UnityEngine;

namespace Experilous.WrapAround
{
	[RequireComponent(typeof(Rigidbody))]
	public class RigidbodyElementWrapper : MechanicalElementWrapper
	{
		protected Rigidbody _rigidbody;

		protected void Start()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		protected new void FixedUpdate()
		{
			world.Confine(_rigidbody);
		}
	}
}
