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
	/// updating once each fixed-interval frame.
	/// </summary>
	/// <remarks>
	/// Each fixed-interval frame, this component asks the referenced world to confine its transform
	/// to the world's canonical bounds.  This is appropriate for objects that are frequently moving,
	/// and whose position and orientation need to be correct for each fixed-interval frame, likely
	/// for the purpose of applying physics or game mechanics in a reliable manner.  If you only need
	/// the position and orientation to be correct for each rendered frame, then see the similar
	/// <see cref="DynamicElementWrapper"/> class.  Or if this element is a rigidbody, see the
	/// <see cref="RigidbodyElementWrapper"/> class.
	/// </remarks>
	/// <seealso cref="World"/>
	/// <seealso cref="IWorldConsumer"/>
	/// <seealso cref="DynamicElementWrapper"/>
	/// <seealso cref="RigidbodyElementWrapper"/>
	[AddComponentMenu("MakeIt.Wrap/Dynamic Element Wrapper (Fixed Update)")]
	public class DynamicElementWrapperUsingFixedUpdate : MonoBehaviour, IWorldConsumer
	{
		[Tooltip("The world component with wrap-around behavior to which this dynamic element conforms.")]
		public World world;

		public bool hasWorld { get { return world != null ; } }
		public World GetWorld() { return world; }
		public void SetWorld(World world) { this.world = world; }

		protected void Start()
		{
			if (world == null) world = WorldConsumerUtility.FindWorld(this);
			this.DisableAndThrowOnUnassignedReference(world, "The DynamicElementWrapperUsingFixedUpdate component requires a reference to a World component.");
		}

		protected void FixedUpdate()
		{
			world.Confine(transform);
		}
	}
}
