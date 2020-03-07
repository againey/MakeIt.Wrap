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
	/// <summary>
	/// Forces a recalculation of the attached viewport's visible ghost regions every render frame.
	/// </summary>
	/// <remarks>
	/// This is created as a separate component primarily to allow the <c>LateUpdate()</c> method
	/// to be given a specific script execution order which will guarantee that it executes after
	/// any scripts that might affect the viewport's configuration (such as a camera controller),
	/// but before all other scripts that need to operate relative to the viewport's latest
	/// configuration (such as a mesh element or light element).
	/// </remarks>
	[RequireComponent(typeof(Viewport))]
	[AddComponentMenu("MakeIt.Wrap/Dynamic Viewport")]
	public class DynamicViewport : MonoBehaviour
	{
		private Viewport _viewport;

		protected void Start()
		{
			_viewport = GetComponent<Viewport>();
		}

		protected void LateUpdate()
		{
			_viewport.RecalculateVisibleGhostRegions();
		}
	}
}
