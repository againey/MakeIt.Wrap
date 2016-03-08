﻿/******************************************************************************\
 *  Copyright (C) 2016 Experilous <againey@experilous.com>
 *  
 *  This file is subject to the terms and conditions defined in the file
 *  'Assets/Plugins/Experilous/License.txt', which is a part of this package.
 *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
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
	public class DynamicElementWrapperUsingFixedUpdate : MonoBehaviour, IWorldConsumer
	{
		public World world;

		public bool hasWorld { get { return world != null ; } }
		public void SetWorld(World world) { this.world = world; }

		protected void Start()
		{
			if (world == null) world = WorldConsumerUtility.FindWorld(this);
			this.DisableAndThrowOnUnassignedReference(world, "The DynamicElementFixedUpdateWrapper component requires a reference to a World component.");
		}

		protected void FixedUpdate()
		{
			world.Confine(transform);
		}
	}
}