/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	public class FixedSphereBounds : SphereBounds
	{
		public static FixedSphereBounds Create(GameObject gameObject, Sphere sphere)
		{
			return CreateDerived<FixedSphereBounds>(gameObject, sphere);
		}

		public static Sphere InverseTransform(Transform transform, Sphere sphere)
		{
			return new Sphere(sphere.center - transform.position, sphere.radius);
		}

		public override Sphere Transform(Transform transform)
		{
			return new Sphere(transform.position + sphere.center, sphere.radius);
		}

		public override Sphere Transform(Transform transform, GhostRegion ghostRegion)
		{
			return ghostRegion.Transform(new Sphere(transform.position + sphere.center, sphere.radius));
		}
	}
}
