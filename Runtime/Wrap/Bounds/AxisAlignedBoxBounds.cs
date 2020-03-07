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
	public abstract class AxisAlignedBoxBounds : ElementBounds
	{
		public Bounds box;

		public AxisAlignedBoxBounds(Bounds box)
		{
			this.box = box;
		}

		public static AxisAlignedBoxBounds Create(Bounds box, bool fixedScale)
		{
			if (fixedScale)
			{
				return new FixedAxisAlignedBoxBounds(box);
			}
			else
			{
				return new ScalableAxisAlignedBoxBounds(box);
			}
		}

		public static AxisAlignedBoxBounds Create(Bounds box, Transform transform, bool fixedScale)
		{
			if (fixedScale)
			{
				return new FixedAxisAlignedBoxBounds(new Bounds(box.center - transform.position, box.size));
			}
			else
			{
				var scale = transform.lossyScale;
				var center = (box.center - transform.position).DivideComponents(scale);
				var size = box.size.DivideComponents(scale);
				return new ScalableAxisAlignedBoxBounds(new Bounds(center, size));
			}
		}
	}
}
