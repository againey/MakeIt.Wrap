/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	public class DynamicSphereBounds : SphereBounds
	{
		public static DynamicSphereBounds Create(GameObject gameObject, Sphere sphere)
		{
			return CreateDerived<DynamicSphereBounds>(gameObject, sphere);
		}

		public static Sphere InverseTransform(Transform transform, Sphere sphere)
		{
			var scale = transform.lossyScale;
			var center = transform.InverseTransformVector(sphere.center - transform.position);
			var radius = sphere.radius / scale.MaxAbsComponent();
			return new Sphere(center, radius);
		}

		public override Sphere Transform(Transform transform)
		{
			var scale = transform.lossyScale;
			return new Sphere(transform.TransformPoint(sphere.center), sphere.radius * scale.MaxAbsComponent());
		}

		public override Sphere Transform(Transform transform, GhostRegion ghostRegion)
		{
			var scale = transform.lossyScale;
			return ghostRegion.Transform(new Sphere(transform.TransformPoint(sphere.center), sphere.radius * scale.MaxAbsComponent()));
		}
	}
}
