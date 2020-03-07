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
	[CustomEditor(typeof(MeshElement))]
	public class MeshElementEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			var element = (MeshElement)target;

			Undo.RecordObject(element, "Mesh Element");

			EditorGUI.BeginChangeCheck();

			element.viewport = (Viewport)EditorGUILayout.ObjectField(new GUIContent("Viewport", "The viewport component with wrap-around behavior with which this mesh interacts."), element.viewport, typeof(Viewport), true);

			GUILayout.Space(EditorGUIUtility.singleLineHeight);

			if (ElementBoundsEditorUtility.OnInspectorGUI(element, ref element.boundsSource, ref element.boundsProvider))
			{
				element.RefreshBounds();
			}

			if (EditorGUI.EndChangeCheck())
			{
				EditorUtility.SetDirty(element);
			}
		}
	}
}
