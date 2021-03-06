﻿/******************************************************************************\
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
using MakeIt.Core;

namespace MakeIt.Wrap
{
	public abstract class GhostableElementEditor<TElement, TGhost> : Editor
		where TElement : GhostableElement<TElement, TGhost>
		where TGhost : Ghost<TElement, TGhost>
	{
		public override void OnInspectorGUI()
		{
			var element = (TElement)target;

			Undo.RecordObject(element, typeof(TElement).GetPrettyName());

			EditorGUI.BeginChangeCheck();

			OnElementGUI(element);
			OnGhostPrefabGUI(element);

			if (EditorGUI.EndChangeCheck())
			{
				EditorUtility.SetDirty(element);
			}
		}

		protected virtual void OnElementGUI(TElement element)
		{
		}

		protected void OnGhostPrefabGUI(TElement element)
		{
			element.ghostPrefab = (TGhost)EditorGUILayout.ObjectField(new GUIContent("Ghost Prefab", "The prefab that will be used to construct ghost instances as they are needed."), element.ghostPrefab, typeof(TGhost), false);

			if (GUILayout.Button(new GUIContent("Create Ghost Prefab", "Automatically generate a prefab that mimics this game object, but strips out all unnecessary children and components.")))
			{
				CreateGhostPrefab(element);
			}

			if (element.ghostPrefab != null)
			{
				if (GUILayout.Button(new GUIContent("Update Ghost Prefab", "Automatically generate a prefab that mimics this game object, but strips out all unnecessary children and components, and replace the existing prefab with the newly generated one.")))
				{
					UpdateGhostPrefab(element);
				}
			}

			if (element.ghostPrefab == null)
			{
				EditorGUILayout.HelpBox("This ghostable element does not yet have a ghost prefab, which will result in various startup and runtime costs.", MessageType.Warning);
			}
		}

		protected void CreateGhostPrefab(TElement element)
		{
			var path = EditorUtility.SaveFilePanelInProject("Create Ghost Prefab", string.Format("{0} Ghost", element.name), "prefab", "Select where to create the ghost prefab.", AssetUtility.selectedFolderOrDefault);

			if (string.IsNullOrEmpty(path)) return;

			var ghostTemplate = CreateGhostTemplate(element);
			var ghostPrefab = PrefabUtility.CreatePrefab(path, ghostTemplate);
			DestroyImmediate(ghostTemplate);

			element.ghostPrefab = ghostPrefab.GetComponent<TGhost>();
		}

		protected void UpdateGhostPrefab(TElement element)
		{
			var ghostTemplate = CreateGhostTemplate(element);
			var ghostPrefab = PrefabUtility.ReplacePrefab(ghostTemplate, element.ghostPrefab);
			DestroyImmediate(ghostTemplate);

			element.ghostPrefab = ghostPrefab.GetComponent<TGhost>();
		}

		protected GameObject CreateGhostTemplate(TElement element)
		{
			var ghostTemplate = Instantiate(element).gameObject;
			ghostTemplate.transform.SetParent(element.transform.parent, false);
			ghostTemplate.name = name + " Ghost";
			element.AdjustGhostComponents(ghostTemplate.transform, ghostTemplate.transform);
			ghostTemplate.AddComponent<TGhost>();
			return ghostTemplate;
		}
	}
}
