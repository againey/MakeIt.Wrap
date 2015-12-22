using UnityEngine;

namespace Experilous.WrapAround
{
	[RequireComponent(typeof(Collider))]
	public class SphereBoundedColliderElement : ColliderElement
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

		public override bool IsCollidable(ColliderElementGhost ghost)
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
					radius = collider.bounds.extents.magnitude;
				}
			}
		}
	}
}
