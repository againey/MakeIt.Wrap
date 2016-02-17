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
			element.bounds = (AbstractBounds)EditorGUILayout.ObjectField("Bounds", element.bounds, typeof(AbstractBounds), true);
			element.ghostPrefab = (LightElementGhost)EditorGUILayout.ObjectField("Ghost Prefab", element.ghostPrefab, typeof(LightElementGhost), false);

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

		protected void CreateGhostPrefab(LightElement element)
		{
			var path = EditorUtility.SaveFilePanelInProject("Create Ghost Prefab", string.Format("{0} Ghost", element.name), "prefab", "Select where to create the ghost prefab.");

			if (string.IsNullOrEmpty(path)) return;

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
