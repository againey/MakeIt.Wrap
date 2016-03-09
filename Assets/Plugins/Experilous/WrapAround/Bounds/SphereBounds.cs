/******************************************************************************\
 *  Copyright (C) 2016 Experilous <againey@experilous.com>
 *  
 *  This file is subject to the terms and conditions defined in the file
 *  'Assets/Plugins/Experilous/License.txt', which is a part of this package.
 *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	public abstract class SphereBounds : ElementBounds
	{
		public Sphere sphere;

		public SphereBounds(Sphere sphere)
		{
			this.sphere = sphere;
		}

		public static SphereBounds Create(Sphere sphere, bool fixedScale, bool fixedOrientation)
		{
			if (fixedScale)
			{
				if (fixedOrientation)
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
				if (fixedOrientation)
				{
					return new ScalableSphereBounds(sphere);
				}
				else
				{
					return new DynamicSphereBounds(sphere);
				}
			}
		}

		public static SphereBounds Create(Sphere sphere, Transform transform, bool fixedScale, bool fixedOrientation)
		{
			if (fixedScale)
			{
				var radius = sphere.radius;
				if (fixedOrientation)
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
				if (fixedOrientation)
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
