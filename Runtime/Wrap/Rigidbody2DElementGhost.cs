/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace MakeIt.Wrap
{
	[RequireComponent(typeof(Rigidbody2D))]
	[AddComponentMenu("MakeIt.Wrap/Elements/Ghosts/Rigidbody 2D")]
	public class Rigidbody2DElementGhost : Ghost<Rigidbody2DElement, Rigidbody2DElementGhost>
	{
		protected Rigidbody2D _rigidbody;
		protected Rigidbody2D _originalRigidbody;

		public new Rigidbody2D rigidbody { get { return _rigidbody; } }

		protected new void Start()
		{
			base.Start();

			_rigidbody = GetComponent<Rigidbody2D>();
			_originalRigidbody = original.GetComponent<Rigidbody2D>();
		}

		protected void FixedUpdate()
		{
			region.Transform(_originalRigidbody, _rigidbody);
			_rigidbody.angularVelocity = _originalRigidbody.angularVelocity;
			_rigidbody.velocity = _originalRigidbody.velocity;

			if (region == null || !region.isActive || !original.bounds.IsCollidable(original.world, transform))
			{
				Destroy();
			}
		}
	}
}
