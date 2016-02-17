using UnityEngine;
using UnityEditor;

namespace Experilous.WrapAround
{
	[CustomEditor(typeof(RigidbodyElement))]
	public class RigidbodyElementEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			var element = (RigidbodyElement)target;

			element.world = (World)EditorGUILayout.ObjectField("World", element.world, typeof(World), true);
			element.bounds = (AbstractBounds)EditorGUILayout.ObjectField("Bounds", element.bounds, typeof(AbstractBounds), true);
			element.ghostPrefab = (RigidbodyElementGhost)EditorGUILayout.ObjectField("Ghost Prefab", element.ghostPrefab, typeof(RigidbodyElementGhost), false);

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

		protected void CreateGhostPrefab(RigidbodyElement element)
		{
			var path = EditorUtility.SaveFilePanelInProject("Create Ghost Prefab", string.Format("{0} Ghost", element.name), "prefab", "Select where to create the ghost prefab.");

			if (string.IsNullOrEmpty(path)) return;

			var ghostTemplate = CreateGhostTemplate(element);
			var ghostPrefab = PrefabUtility.CreatePrefab(path, ghostTemplate);
			DestroyImmediate(ghostTemplate);

			element.ghostPrefab = ghostPrefab.GetComponent<RigidbodyElementGhost>();
		}

		protected void UpdateGhostPrefab(RigidbodyElement element)
		{
			var ghostTemplate = CreateGhostTemplate(element);
			var ghostPrefab = PrefabUtility.ReplacePrefab(ghostTemplate, element.ghostPrefab);
			DestroyImmediate(ghostTemplate);

			element.ghostPrefab = ghostPrefab.GetComponent<RigidbodyElementGhost>();
		}

		protected GameObject CreateGhostTemplate(RigidbodyElement element)
		{
			var ghostTemplate = Instantiate(element).gameObject;
			ghostTemplate.transform.SetParent(element.transform.parent, false);
			ghostTemplate.name = name + " Ghost";
			AdjustComponents(ghostTemplate.transform, ghostTemplate.transform);
			ghostTemplate.AddComponent<RigidbodyElementGhost>();
			return ghostTemplate;
		}

		private bool AdjustComponents(Transform transform, Transform topLevel)
		{
			var hasChildren = false;
			for (int i = 0; i < transform.childCount; ++i)
			{
				hasChildren = AdjustComponents(transform.GetChild(i), topLevel) || hasChildren;
			}

			var components = transform.GetComponents<Component>();
			var hasRigidbody = false;
			var hasCollider = false;
			foreach (var component in components)
			{
				if (component is Rigidbody)
				{
					hasRigidbody = true;
				}
				else if (component is Collider)
				{
					hasCollider = true;
				}
			}

			if (hasRigidbody && !ReferenceEquals(transform, topLevel))
			{
				// Nested rigidbodies are not included as part of the element ghost prefab.
				DestroyImmediate(transform.gameObject);
				return false;
			}
			else if (!hasCollider)
			{
				if (!hasChildren)
				{
					// Neither this object nor any of its descendants contain colliders, so it isn't neeed as part of the element ghost prefab.
					DestroyImmediate(transform.gameObject);
					return false;
				}
				else
				{
					// This object is just a transform intermediary between the root and descendant colliders; keep the transform and nothing else.
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
				// Remove any component that doesn't contribute to the physical definition of the root rigidbody.
				foreach (var component in components)
				{
					if (!(component is Rigidbody || component is Collider || component is Transform))
					{
						DestroyImmediate(component);
					}
				}
				return true;
			}
		}
	}
}
