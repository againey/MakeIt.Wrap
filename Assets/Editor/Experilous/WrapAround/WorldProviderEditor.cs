using UnityEngine;
using UnityEditor;

namespace Experilous.WrapAround
{
	[CustomEditor(typeof(WorldProvider))]
	public class WorldProviderEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			var provider = (WorldProvider)target;

			provider.world = (World)EditorGUILayout.ObjectField("World", provider.world, typeof(World), true);

			GUI.enabled = (provider.world != null);
			if (GUILayout.Button("Push to Unset Children"))
			{
				foreach (var element in provider.GetComponentsInChildren<ColliderElement>())
				{
					if (element.world == null)
					{
						element.world = provider.world;
						EditorUtility.SetDirty(element);
					}
				}

				foreach (var element in provider.GetComponentsInChildren<RigidbodyElement>())
				{
					if (element.world == null)
					{
						element.world = provider.world;
						EditorUtility.SetDirty(element);
					}
				}
			}
			GUI.enabled = true;

			GUI.enabled = (provider.world != null);
			if (GUILayout.Button("Push to All Children"))
			{
				foreach (var element in provider.GetComponentsInChildren<ColliderElement>())
				{
					element.world = provider.world;
					EditorUtility.SetDirty(element);
				}

				foreach (var element in provider.GetComponentsInChildren<RigidbodyElement>())
				{
					element.world = provider.world;
					EditorUtility.SetDirty(element);
				}
			}
			GUI.enabled = true;
		}
	}
}
