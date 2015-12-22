using UnityEngine;

namespace Experilous.WrapAround
{
	[RequireComponent(typeof(Rigidbody))]
	public class SphereBoundedRigidbodyElement : RigidbodyElement
	{
		public float radius;

		private float scaledRadius
		{
			get
			{
				var scale = transform.lossyScale;
				return radius * Mathf.Max(Mathf.Max(scale.x, scale.y), scale.z);
			}
		}

		public override bool IsCollidable(RigidbodyElementGhost ghost)
		{
			return world.IsCollidable(ghost.transform.position, scaledRadius);
		}

		public override bool IsCollidable(Vector3 position, Quaternion rotation)
		{
			return world.IsCollidable(position, scaledRadius);
		}

		protected void Start()
		{
			if (radius == 0f)
			{
				var sphereCollider = GetComponent<SphereCollider>();
				if (sphereCollider != null)
				{
					radius = sphereCollider.radius;
				}
				else
				{
					var collider = GetComponent<Collider>();
					if (collider != null)
					{
						radius = collider.bounds.extents.magnitude;
					}
				}
			}
		}
	}
}
