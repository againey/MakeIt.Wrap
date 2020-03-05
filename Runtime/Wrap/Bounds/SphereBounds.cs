/******************************************************************************\
* Copyright Andy Gainey                                                        *
*                                                                              *
* Licensed under the Apache License, Version 2.0 (the "License");              *
* you may not use this file except in compliance with the License.             *
* You may obtain a copy of the License at                                      *
*                                                                              *
*     http://www.apache.org/licenses/LICENSE-2.0                               *
*                                                                              *
* Unless required by applicable law or agreed to in writing, software          *
* distributed under the License is distributed on an "AS IS" BASIS,            *
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.     *
* See the License for the specific language governing permissions and          *
* limitations under the License.                                               *
\******************************************************************************/

using UnityEngine;
using MakeIt.Numerics;

namespace MakeIt.Wrap
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
