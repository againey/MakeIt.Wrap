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
using UnityEditor;

namespace MakeIt.Wrap
{
	[CustomEditor(typeof(ViewportProvider))]
	public class ViewportProviderEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			var provider = (ViewportProvider)target;

			Viewport viewport = null;

			viewport = (Viewport)EditorGUILayout.ObjectField(new GUIContent("Viewport", "The viewport component to be provided to any descendant viewport consumers."), provider.viewport, typeof(Viewport), true);

			GUI.enabled = (viewport != null);
			if (GUILayout.Button(new GUIContent("Apply to Unset Children", "Search through all descendants for any components that implement IViewportConsumer, and assign it this provider's viewport if the component doesn't already have a viewport assigned.")))
			{
				provider.ApplyToUnsetConsumers(provider.gameObject, true);
			}

			if (GUILayout.Button(new GUIContent("Apply to All Children", "Search through all descendants for any components that implement IViewportConsumer, and assign it this provider's viewport, overwriting any viewport it might have already been assigned.")))
			{
				provider.ApplyToAllConsumers(provider.gameObject, true);
			}
			GUI.enabled = true;

			if (!ReferenceEquals(provider.viewport, viewport))
			{
				Undo.RecordObject(provider, "Viewport Provider");
				provider.viewport = viewport;
				EditorUtility.SetDirty(provider);
			}
		}
	}
}
