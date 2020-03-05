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
	public interface IBoundedElement
	{
		ElementBounds bounds { get; }
		void RefreshBounds();
	}

	public abstract class BoundedElement : MonoBehaviour, IBoundedElement
	{
		public abstract ElementBounds bounds { get; }
		public virtual void RefreshBounds() { }

#if UNITY_EDITOR
		protected virtual void OnDrawGizmosSelected()
		{
			if (bounds == null) RefreshBounds();
			if (bounds != null) bounds.DrawGizmosSelected(transform, Color.white);
		}
#endif
	}
}
