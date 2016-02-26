﻿/******************************************************************************\
 *  Copyright (C) 2016 Experilous <againey@experilous.com>
 *  
 *  This file is subject to the terms and conditions defined in the file
 *  'Assets/Plugins/Experilous/License.txt', which is a part of this package.
 *
\******************************************************************************/

using UnityEngine;
using UnityEditor;

namespace Experilous.WrapAround
{
	public abstract class GhostableElementEditor<TElement, TGhost> : Editor
		where TElement : GhostableElement<TElement, TGhost>
		where TGhost : Ghost<TElement, TGhost>
	{
		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();

			var element = (TElement)target;
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

			element.ghostPrefab = (TGhost)EditorGUILayout.ObjectField("Ghost Prefab", element.ghostPrefab, typeof(TGhost), false);

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
			AdjustComponents(ghostTemplate.transform, ghostTemplate.transform);
			ghostTemplate.AddComponent<TGhost>();
			return ghostTemplate;
		}

		protected abstract bool IsExcluded(Component[] components);
		protected abstract bool IsNecessary(Component[] components);
		protected abstract void RemoveUnnecessaryComponents(Component[] components);

		protected bool AdjustComponents(Transform transform, Transform topLevel)
		{
			var hasChildren = false;
			for (int i = 0; i < transform.childCount; ++i)
			{
				hasChildren = AdjustComponents(transform.GetChild(i), topLevel) || hasChildren;
			}

			var components = transform.GetComponents<Component>();

			bool isExcludedDescendant = IsExcluded(components) && !ReferenceEquals(transform, topLevel);
			bool isNecessary = IsNecessary(components) || ReferenceEquals(transform, topLevel);

			if (isExcludedDescendant)
			{
				DestroyImmediate(transform.gameObject);
				return false;
			}
			else if (!isNecessary)
			{
				if (!hasChildren)
				{
					DestroyImmediate(transform.gameObject);
					return false;
				}
				else
				{
					RemoveAll(components, (Component component) => { return !(component is Transform); });
					return true;
				}
			}
			else
			{
				RemoveUnnecessaryComponents(components);
				return true;
			}
		}

		protected void RemoveAll(Component[] components, System.Predicate<Component> predicate)
		{
			bool noneRemoved = false;
			bool allRemoved = false;
			while (!noneRemoved && !allRemoved)
			{
				noneRemoved = true;
				allRemoved = true;
				foreach (var component in components)
				{
					if (component != null)
					{
						if (predicate(component))
						{
							if (CanDestroy(component, components))
							{
								DestroyImmediate(component);
								if (component == null)
								{
									noneRemoved = false;
								}
								else
								{
									allRemoved = false;
								}
							}
							else
							{
								allRemoved = false;
							}
						}
					}
				}
			}
		}

		private bool CanDestroy(Component component, Component[] components)
		{
			foreach (var otherComponent in components)
			{
				if (otherComponent != null)
				{
					var requirements = Utility.GetAttributes<RequireComponent>(otherComponent.GetType());
					foreach (var requirement in requirements)
					{
						if (requirement.m_Type0 != null && requirement.m_Type0.IsInstanceOfType(component)) return false;
						if (requirement.m_Type1 != null && requirement.m_Type1.IsInstanceOfType(component)) return false;
						if (requirement.m_Type2 != null && requirement.m_Type2.IsInstanceOfType(component)) return false;
					}
				}
			}

			return true;
		}
	}
}
