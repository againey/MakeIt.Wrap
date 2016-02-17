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
			if (GUILayout.Button("Apply to Unset Children"))
			{
				foreach (var element in provider.GetComponentsInChildren<IWorldConsumer>())
				{
					if (!element.hasWorld)
					{
						element.SetWorld(provider.world);
						EditorUtility.SetDirty((Component)element);
					}
				}
			}
			GUI.enabled = true;

			GUI.enabled = (provider.world != null);
			if (GUILayout.Button("Apply to All Children"))
			{
				foreach (var element in provider.GetComponentsInChildren<IWorldConsumer>())
				{
					element.SetWorld(provider.world);
					EditorUtility.SetDirty((Component)element);
				}
			}
			GUI.enabled = true;
		}
	}
}
