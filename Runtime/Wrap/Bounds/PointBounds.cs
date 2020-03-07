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
	public abstract class PointBounds : ElementBounds
	{
		public Vector3 position;

		public PointBounds(Vector3 position)
		{
			this.position = position;
		}

		public static PointBounds Create(Vector3 position, bool fixedScale, bool fixedRotation)
		{
			if (fixedScale)
			{
				if (fixedRotation)
				{
					return new FixedPointBounds(position);
				}
				else
				{
					return new RotatablePointBounds(position);
				}
			}
			else
			{
				if (fixedRotation)
				{
					return new ScalablePointBounds(position);
				}
				else
				{
					return new DynamicPointBounds(position);
				}
			}
		}

		public static PointBounds Create(Vector3 position, Transform transform, bool fixedScale, bool fixedRotation)
		{
			if (fixedScale)
			{
				if (fixedRotation)
				{
					return new FixedPointBounds(position - transform.position);
				}
				else
				{
					return new RotatablePointBounds(transform.InverseTransformDirection(position - transform.position));
				}
			}
			else
			{
				var scale = transform.lossyScale;
				if (fixedRotation)
				{
					return new ScalablePointBounds((position - transform.position).DivideComponents(scale));
				}
				else
				{
					return new DynamicPointBounds(transform.InverseTransformVector(position - transform.position));
				}
			}
		}
	}
}
