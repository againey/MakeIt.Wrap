﻿using UnityEngine;
using Experilous.WrapAround;

namespace Experilous.Examples.RPG
{
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerController : MonoBehaviour
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