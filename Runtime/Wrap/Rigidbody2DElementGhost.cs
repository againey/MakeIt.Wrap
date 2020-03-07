/******************************************************************************\
* Copyright Andy Gainey                                                        *
*                                                                              *
* Licensed under the Apache License, Version 2.0 (the "License");              *
* you may not use this file except in compliance with the License.             *
* You may obtain a copy of the License at                                      *
*                                                                              *
*     http://www.apache.org/licenses/LICENSE-2.0                               *
*                                                                              *
* Unless required by applicable law or agreed to in writing, software          *
* distributed under the License is distributed on an "AS IS" BASIS,            *
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.     *
* See the License for the specific language governing permissions and          *
* limitations under the License.                                               *
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
