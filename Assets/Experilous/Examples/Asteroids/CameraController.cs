﻿using UnityEngine;

namespace Experilous.Examples.Asteroids
{
	public class CameraController : MonoBehaviour
	{
		public float speed = 1f;
		public float zoomSpeed = 1f;

		public Vector3 surfaceNormal = Vector3.back;

		protected void Start()
		{
			surfaceNormal.Normalize();
		}

		protected void Update()
		{
			var moveHorizontal = Input.GetAxis("Horizontal");
			var moveForward = Input.GetAxis("Vertical");
			var moveUp = -Input.GetAxis("Zoom");

			var right = Vector3.Cross(surfaceNormal, transform.forward);
			var forward = Vector3.Cross(transform.right, surfaceNormal);
			if (right == Vector3.zero) right = Vector3.Cross(surfaceNormal, transform.up);
			if (forward == Vector3.zero) forward = Vector3.Cross(transform.up, surfaceNormal);

			right.Normalize();
			forward.Normalize();

			transform.position += right * moveHorizontal * speed + forward * moveForward * speed + surfaceNormal * moveUp * zoomSpeed;
		}
	}
}