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

		public override bool IsVisible(ColliderElementGhost ghost)
		{
			return viewport.IsVisible(ghost.transform.position, scaledRadius);
		}

		public override bool IsVisible(Vector3 position, Quaternion rotation)
		{
			return viewport.IsVisible(position, scaledRadius);
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
