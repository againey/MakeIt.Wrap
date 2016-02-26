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
	public class RigidbodyElementWrapper : MonoBehaviour, IWorldConsumer
	{
		public World world;

		protected Rigidbody _rigidbody;

		public bool hasWorld { get { return world != null ; } }
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
