﻿/******************************************************************************\
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
using MakeIt.Core;

namespace MakeIt.Wrap
{
	/// <summary>
	/// Forces the attached dynamic object to always remain confined to the canonical world bounds,
	/// updating the rigidbody once each fixed-interval frame.
	/// </summary>
	/// <remarks>
	/// Each fixed-interval frame, this component asks the referenced world to confine its rigidbody
	/// to the world's canonical bounds.  This is appropriate for dynamic rigidbody objects.  If you
	/// only need the position and orientation to be correct for each rendered frame, then see the
	/// similar <see cref="DynamicElementWrapper"/> class.  Or if this element needs to be correct
	/// for each fixed-interval frame but does not have a rigidbody component, then see the
	/// <see cref="DynamicElementWrapperUsingFixedUpdate"/> class.
	/// </remarks>
	/// <seealso cref="World"/>
	/// <seealso cref="IWorldConsumer"/>
	/// <seealso cref="DynamicElementWrapper"/>
	/// <seealso cref="DynamicElementWrapperUsingFixedUpdate"/>
	[RequireComponent(typeof(Rigidbody))]
	[AddComponentMenu("MakeIt.Wrap/Rigidbody Element Wrapper")]
	public class RigidbodyElementWrapper : MonoBehaviour, IWorldConsumer
	{
		[Tooltip("The world component with wrap-around behavior to which this rigidbody conforms.")]
		public World world;

		protected Rigidbody _rigidbody;

		public bool hasWorld { get { return world != null ; } }
		public World GetWorld() { return world; }
		public void SetWorld(World world) { this.world = world; }

		protected void Start()
		{
			if (world == null) world = WorldConsumerUtility.FindWorld(this);
			this.DisableAndThrowOnUnassignedReference(world, "The RigidbodyElementWrapper component requires a reference to a World component.");

			_rigidbody = GetComponent<Rigidbody>();
		}

		protected void FixedUpdate()
		{
			world.Confine(_rigidbody);
		}
	}
}
