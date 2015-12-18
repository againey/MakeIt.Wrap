using UnityEngine;

namespace Experilous.WrapAround
{
	[RequireComponent(typeof(Element))]
	[RequireComponent(typeof(Rigidbody))]
	public class RigidbodyElementWrapper : MechanicalElementWrapper
	{
		protected Rigidbody _rigidbody;

		protected new void Start()
		{
			base.Start();
			_rigidbody = GetComponent<Rigidbody>();
		}

		protected new void FixedUpdate()
		{
			_element.world.Confine(_rigidbody);
		}
	}
}
