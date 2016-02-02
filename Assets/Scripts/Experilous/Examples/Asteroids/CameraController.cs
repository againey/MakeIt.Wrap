using UnityEngine;
using System.Collections.Generic;
using Experilous.WrapAround;

namespace Experilous.Examples.Asteroids
{
	public class CameraController : MonoBehaviour
	{
		public float speed = 1f;

		protected void Update()
		{
			var horizontal = Input.GetAxis("Horizontal");
			var vertical = Input.GetAxis("Vertical");

			transform.position = transform.position + new Vector3(horizontal * speed, vertical * speed, 0f);
		}
	}
}
