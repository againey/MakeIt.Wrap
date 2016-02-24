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
	[CustomEditor(typeof(ViewportProvider))]
	public class ViewportProviderEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			var provider = (ViewportProvider)target;

			provider.viewport = (Viewport)EditorGUILayout.ObjectField("Viewport", provider.viewport, typeof(Viewport), true);

			GUI.enabled = (provider.viewport != null);
			if (GUILayout.Button(new GUIContent("Apply to Unset Children", "Search through all descendants for any components that implement IViewportConsumer, and assign it this provider's viewport if the component doesn't already have a viewport assigned.")))
			{
				foreach (var element in provider.GetComponentsInChildren<IViewportConsumer>())
				{
					if (!element.hasViewport)
					{
						element.SetViewport(provider.viewport);
						EditorUtility.SetDirty((Component)element);
					}
				}
			}
			GUI.enabled = true;

			GUI.enabled = (provider.viewport != null);
			if (GUILayout.Button(new GUIContent("Apply to All Children", "Search through all descendants for any components that implement IViewportConsumer, and assign it this provider's viewport, overwriting any viewport it might have already been assigned.")))
			{
				foreach (var element in provider.GetComponentsInChildren<IViewportConsumer>())
				{
					element.SetViewport(provider.viewport);
					EditorUtility.SetDirty((Component)element);
				}
			}
			GUI.enabled = true;
		}
	}
}
