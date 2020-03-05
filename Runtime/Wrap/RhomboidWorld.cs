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

namespace MakeIt.Wrap
{
	[AddComponentMenu("MakeIt.Wrap/Worlds/Rhomboid World")]
	public class RhomboidWorld : RhomboidWorldBase
	{
		public Vector3 origin;
		public Vector3 axis0;
		public Vector3 axis1;
		public Vector3 axis2;
		public bool wrapAxis0;
		public bool wrapAxis1;
		public bool wrapAxis2;

		public override Vector3 untransformedOrigin { get { return origin; } }
		public override Vector3 untransformedAxis0Vector { get { return axis0; } }
		public override Vector3 untransformedAxis1Vector { get { return axis1; } }
		public override Vector3 untransformedAxis2Vector { get { return axis2; } }

		public override bool axis0IsWrapped { get { return wrapAxis0; } }
		public override bool axis1IsWrapped { get { return wrapAxis1; } }
		public override bool axis2IsWrapped { get { return wrapAxis2; } }
	}
}
