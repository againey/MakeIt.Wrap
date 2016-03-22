/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	public class RotatableSphereBounds : SphereBounds
	{
		public static RotatableSphereBounds Create(GameObject gameObject, Sphere sphere)
		{
			return CreateDerived<RotatableSphereBounds>(gameObject, sphere);
		}

		public static Sphere InverseTransform(Transform transform, Sphere sphere)
		{
			return new Sphere(transform.InverseTransformDirection(sphere.center - transform.position), sphere.radius);
		}

		public override Sphere Transform(Transform transform)
		{
			return new Sphere(transform.position + transform.TransformDirection(sphere.center), sphere.radius);
		}

		public override Sphere Transform(Transform transform, GhostRegion ghostRegion)
		{
			return ghostRegion.Transform(new Sphere(transform.position + transform.TransformDirection(sphere.center), sphere.radius));
		}
	}
}
