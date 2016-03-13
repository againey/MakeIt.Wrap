/******************************************************************************\
 *  Copyright (C) 2016 Experilous <againey@experilous.com>
 *  
 *  This file is subject to the terms and conditions defined in the file
 *  'Assets/Plugins/Experilous/License.txt', which is a part of this package.
 *
\******************************************************************************/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Experilous.WrapAround
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
				var consumers = new List<Object>();
				foreach (var element in provider.GetComponentsInChildren<IWorldConsumer>())
				{
					if (element.GetWorld() == null)
					{
						consumers.Add((Object)element);
					}
				}

				if (consumers.Count > 0)
				{
					Undo.RecordObjects(consumers.ToArray(), "Apply World To Children");

					foreach (var element in consumers)
					{
						((IWorldConsumer)element).SetWorld(provider.world);
						EditorUtility.SetDirty(element);
					}
				}
			}

			if (GUILayout.Button(new GUIContent("Apply to All Children", "Search through all descendants for any components that implement IWorldConsumer, and assign it this provider's world, overwriting any world it might have already been assigned.")))
			{
				var consumers = new List<Object>();
				foreach (var element in provider.GetComponentsInChildren<IWorldConsumer>())
				{
					if (!ReferenceEquals(element.GetWorld(), world))
					{
						consumers.Add((Object)element);
					}
				}

				if (consumers.Count > 0)
				{
					Undo.RecordObjects(consumers.ToArray(), "Apply World To Children");

					foreach (var element in consumers)
					{
						((IWorldConsumer)element).SetWorld(provider.world);
						EditorUtility.SetDirty(element);
					}
				}
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
