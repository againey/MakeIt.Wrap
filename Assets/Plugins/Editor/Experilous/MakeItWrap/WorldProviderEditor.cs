/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using UnityEditor;

namespace Experilous.MakeItWrap
{
	[CustomEditor(typeof(WorldProvider))]
	public class WorldProviderEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			var provider = (WorldProvider)target;

			World world = null;

			world = (World)EditorGUILayout.ObjectField(new GUIContent("World", "The world component to be provided to any descendant world consumers."), provider.world, typeof(World), true);

			GUI.enabled = (world != null);
			if (GUILayout.Button(new GUIContent("Apply to Unset Children", "Search through all descendants for any components that implement IWorldConsumer, and assign it this provider's world if the component doesn't already have a world assigned.")))
			{
				provider.ApplyToUnsetConsumers(provider.gameObject, true);
			}

			if (GUILayout.Button(new GUIContent("Apply to All Children", "Search through all descendants for any components that implement IWorldConsumer, and assign it this provider's world, overwriting any world it might have already been assigned.")))
			{
				provider.ApplyToAllConsumers(provider.gameObject, true);
			}
			GUI.enabled = true;

			if (!ReferenceEquals(provider.world, world))
			{
				Undo.RecordObject(provider, "World Provider");
				provider.world = world;
				EditorUtility.SetDirty(provider);
			}
		}
	}
}
