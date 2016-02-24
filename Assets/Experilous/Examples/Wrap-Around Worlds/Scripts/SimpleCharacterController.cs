﻿/******************************************************************************\
 *  Copyright (C) 2016 Experilous <againey@experilous.com>
 *  
 *  This file is subject to the terms and conditions defined in the file
 *  'Assets/Plugins/Experilous/License.txt', which is a part of this package.
 *
\******************************************************************************/

using UnityEngine;

namespace Experilous.Examples.WrapAround
{
	[RequireComponent(typeof(Rigidbody))]
	public class SimpleCharacterController : MonoBehaviour
	{
		public float walkSpeed = 1f;

		protected Rigidbody _rigidbody;

		protected void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		protected void Update()
		{
			var horizontal = Input.GetAxis("Horizontal");
			var vertical = Input.GetAxis("Vertical");

			_rigidbody.velocity = new Vector3(horizontal * walkSpeed, 0f, vertical * walkSpeed);
		}
	}
}