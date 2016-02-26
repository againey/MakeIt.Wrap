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
	[RequireComponent(typeof(Rigidbody2D))]
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

			if (region == null || !region.isActive || !original.IsCollidable(this))
			{
				Destroy();
			}
		}
	}
}
