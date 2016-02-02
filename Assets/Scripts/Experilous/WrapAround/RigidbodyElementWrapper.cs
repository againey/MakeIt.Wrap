using UnityEngine;

namespace Experilous.WrapAround
{
	[RequireComponent(typeof(Rigidbody))]
	public class RigidbodyElementWrapper : MonoBehaviour
	{
		public World world;

		protected Rigidbody _rigidbody;

		protected void Start()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		protected void FixedUpdate()
		{
			world.Confine(_rigidbody);
		}
	}
}
