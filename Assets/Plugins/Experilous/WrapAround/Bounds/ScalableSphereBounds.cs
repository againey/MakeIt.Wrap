/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	public class ScalableSphereBounds : SphereBounds
	{
		public static ScalableSphereBounds Create(GameObject gameObject, Sphere sphere)
		{
			return CreateDerived<ScalableSphereBounds>(gameObject, sphere);
		}

		public static Sphere InverseTransform(Transform transform, Sphere sphere)
		{
			var scale = transform.lossyScale;
			var center = (sphere.center - transform.position).DivideComponents(scale);
			var radius = sphere.radius / scale.MaxAbsComponent();
			return new Sphere(center, radius);
		}

		public override Sphere Transform(Transform transform)
		{
			var scale = transform.lossyScale;
			return new Sphere(transform.position + sphere.center.MultiplyComponents(scale), sphere.radius * scale.MaxAbsComponent());
		}

		public override Sphere Transform(Transform transform, GhostRegion ghostRegion)
		{
			var scale = transform.lossyScale;
			return ghostRegion.Transform(new Sphere(transform.position + sphere.center.MultiplyComponents(scale), sphere.radius * scale.MaxAbsComponent()));
		}
	}
}
