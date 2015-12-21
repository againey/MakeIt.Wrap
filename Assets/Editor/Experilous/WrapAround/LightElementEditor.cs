using UnityEngine;
using UnityEditor;

namespace Experilous.WrapAround
{
	[CustomEditor(typeof(LightElement))]
	public class LightElementEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			var element = (LightElement)target;

			element.viewport = (Viewport)EditorGUILayout.ObjectField("Viewport", element.viewport, typeof(Viewport), true);
			element.ghostPrefab = (LightElementGhost)EditorGUILayout.ObjectField("Ghost Prefab", element.ghostPrefab, typeof(LightElementGhost), false);

			if (GUILayout.Button("Create Ghost Prefab"))
			{
				CreateGhostPrefab(element);
			}

			if (element.ghostPrefab != null)
			{
				if (GUILayout.Button("Update Ghost Prefab"))
				{
					UpdateGhostPrefab(element);
				}
			}
		}

		protected void CreateGhostPrefab(LightElement element)
		{
			var path = EditorUtility.SaveFilePanelInProject("Create Ghost Prefab", string.Format("{0} Ghost", element.name), "prefab", "Select where to create the ghost prefab.");

			if (AssetDatabase.LoadAssetAtPath<Object>(path) != null)
			{
				if (EditorUtility.DisplayDialog("Create Ghost Prefab", "An asset already exists at this path.  Do you want to overwrite it?", "Yes", "No") == false)
				{
					return;
				}
			}

			var ghostTemplate = CreateGhostTemplate(element);
			var ghostPrefab = PrefabUtility.CreatePrefab(path, ghostTemplate);
			DestroyImmediate(ghostTemplate);

			element.ghostPrefab = ghostPrefab.GetComponent<LightElementGhost>();
		}

		protected void UpdateGhostPrefab(LightElement element)
		{
			var ghostTemplate = CreateGhostTemplate(element);
			var ghostPrefab = PrefabUtility.ReplacePrefab(ghostTemplate, element.ghostPrefab);
			DestroyImmediate(ghostTemplate);

			element.ghostPrefab = ghostPrefab.GetComponent<LightElementGhost>();
		}

		protected GameObject CreateGhostTemplate(LightElement element)
		{
			var ghostTemplate = Instantiate(element).gameObject;
			ghostTemplate.transform.SetParent(element.transform.parent, false);
			ghostTemplate.name = name + " Ghost";
			AdjustComponents(ghostTemplate.transform);
			ghostTemplate.AddComponent<LightElementGhost>();
			return ghostTemplate;
		}

		private bool AdjustComponents(Transform transform)
		{
			var hasChildren = false;
			for (int i = 0; i < transform.childCount; ++i)
			{
				hasChildren = AdjustComponents(transform.GetChild(i)) || hasChildren;
			}

			var components = transform.GetComponents<Component>();
			var hasLight = false;
			foreach (var component in components)
			{
				if (component is Light)
				{
					hasLight = true;
				}
			}

			if (!hasLight)
			{
				if (!hasChildren)
				{
					DestroyImmediate(transform.gameObject);
					return false;
				}
				else
				{
					foreach (var component in components)
					{
						if (!(component is Transform))
						{
							DestroyImmediate(component);
						}
					}
					return true;
				}
			}
			else
			{
				foreach (var component in components)
				{
					if (!(component is Light || component is Transform))
					{
						DestroyImmediate(component);
					}
				}
				return true;
			}
		}
	}
}
