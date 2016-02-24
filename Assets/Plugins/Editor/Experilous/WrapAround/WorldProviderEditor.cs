/******************************************************************************\
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
	[CustomEditor(typeof(WorldProvider))]
	public class WorldProviderEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			var provider = (WorldProvider)target;

			provider.world = (World)EditorGUILayout.ObjectField("World", provider.world, typeof(World), true);

			GUI.enabled = (provider.world != null);
			if (GUILayout.Button(new GUIContent("Apply to Unset Children", "Search through all descendants for any components that implement IWorldConsumer, and assign it this provider's world if the component doesn't already have a world assigned.")))
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
			if (GUILayout.Button(new GUIContent("Apply to All Children", "Search through all descendants for any components that implement IWorldConsumer, and assign it this provider's world, overwriting any world it might have already been assigned.")))
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
