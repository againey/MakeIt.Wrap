using UnityEngine;
using System.Collections.Generic;
using Experilous.WrapAround;

namespace Experilous.Examples.RPG
{
	public class PlayerController : MonoBehaviour
	{
		public Camera viewportCamera;
		public AxisAligned2DViewport viewport;

		public float walkSpeed = 1f;

		protected void Awake()
		{
		}
	
		protected void Start()
		{
		}
	
		protected void FixedUpdate()
		{
		}
	
		protected void Update()
		{
			var horizontal = Input.GetAxis("Horizontal");
			var vertical = Input.GetAxis("Vertical");

			transform.position += new Vector3(horizontal * walkSpeed, vertical * walkSpeed, 0f);
		}
	
		protected void LateUpdate()
		{
		}
	}
}
