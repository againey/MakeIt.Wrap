﻿/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using Experilous.MakeIt.Utilities;

namespace Experilous.MakeIt.Wrap
{
	public abstract class SphereBounds : ElementBounds
	{
		public Sphere sphere;

		public SphereBounds(Sphere sphere)
		{
			this.sphere = sphere;
		}

		public static SphereBounds Create(Sphere sphere, bool fixedScale, bool fixedRotation)
		{
			if (fixedScale)
			{
				if (fixedRotation)
				{
					return new FixedSphereBounds(sphere);
				}
				else
				{
					return new RotatableSphereBounds(sphere);
				}
			}
			else
			{
				if (fixedRotation)
				{
					return new ScalableSphereBounds(sphere);
				}
				else
				{
					return new DynamicSphereBounds(sphere);
				}
			}
		}

		public static SphereBounds Create(Sphere sphere, Transform transform, bool fixedScale, bool fixedRotation)
		{
			if (fixedScale)
			{
				var radius = sphere.radius;
				if (fixedRotation)
				{
					var center = sphere.center - transform.position;
					return new FixedSphereBounds(new Sphere(center, radius));
				}
				else
				{
					var center = transform.InverseTransformDirection(sphere.center - transform.position);
					return new RotatableSphereBounds(new Sphere(center, radius));
				}
			}
			else
			{
				var scale = transform.lossyScale;
				var radius = sphere.radius / scale.MaxAbsComponent();
				if (fixedRotation)
				{
					var center = (sphere.center - transform.position).DivideComponents(scale);
					return new ScalableSphereBounds(new Sphere(center, radius));
				}
				else
				{
					var center = transform.InverseTransformVector(sphere.center - transform.position);
					return new DynamicSphereBounds(new Sphere(center, radius));
				}
			}
		}
	}
}