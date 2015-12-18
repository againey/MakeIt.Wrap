using System;
using UnityEngine;

namespace Experilous.WrapAround
{
	public class SphereElement : Element
	{
		public float radius;

		protected void Start()
		{
			if (radius == 0f)
			{
				var scaleVector = transform.lossyScale;
				var scaleScalar = Mathf.Max(Mathf.Max(scaleVector.x, scaleVector.y), scaleVector.z);
				var sphereCollider = GetComponent<SphereCollider>();
				if (sphereCollider != null)
				{

					radius = sphereCollider.radius * scaleScalar;
				}
				else
				{
					var collider = GetComponent<Collider>();
					if (collider != null)
					{
						radius = collider.bounds.extents.magnitude * scaleScalar;
					}
				}
			}
		}

		public override bool IsVisible(Viewport viewport, Vector3 position, Quaternion rotation)
		{
			return viewport.IsVisible(position, radius);
		}
	}
}
