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
using System;

namespace MakeIt.Wrap
{
	[AddComponentMenu("MakeIt.Wrap/Bounds/Axis Aligned Box Bounds Provider")]
	public class AxisAlignedBoxBoundsProvider : ElementBoundsProvider
	{
		public Vector3 center;
		public Vector3 size;

		public override ElementBounds CreateBounds(bool fixedScale, bool fixedRotation)
		{
			if (fixedRotation == false) throw new ArgumentException("Axis aligned box bounds cannot be used with dynamic rotation.");
			return AxisAlignedBoxBounds.Create(new Bounds(center, size), fixedScale);
		}
	}
}
