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
	[CustomEditor(typeof(ViewportProvider))]
	public class ViewportProviderEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			var provider = (ViewportProvider)target;

			Viewport viewport = null;

			viewport = (Viewport)EditorGUILayout.ObjectField("Viewport", provider.viewport, typeof(Viewport), true);

			GUI.enabled = (viewport != null);
			if (GUILayout.Button(new GUIContent("Apply to Unset Children", "Search through all descendants for any components that implement IViewportConsumer, and assign it this provider's viewport if the component doesn't already have a viewport assigned.")))
			{
				var consumers = new List<Object>();
				foreach (var element in provider.GetComponentsInChildren<IViewportConsumer>())
				{
					if (element.GetViewport() == null)
					{
						consumers.Add((Object)element);
					}
				}

				if (consumers.Count > 0)
				{
					Undo.RecordObjects(consumers.ToArray(), "Apply Viewport To Children");

					foreach (var element in consumers)
					{
						((IViewportConsumer)element).SetViewport(provider.viewport);
						EditorUtility.SetDirty(element);
					}
				}
			}

			if (GUILayout.Button(new GUIContent("Apply to All Children", "Search through all descendants for any components that implement IViewportConsumer, and assign it this provider's viewport, overwriting any viewport it might have already been assigned.")))
			{
				var consumers = new List<Object>();
				foreach (var element in provider.GetComponentsInChildren<IViewportConsumer>())
				{
					if (!ReferenceEquals(element.GetViewport(), viewport))
					{
						consumers.Add((Object)element);
					}
				}

				if (consumers.Count > 0)
				{
					Undo.RecordObjects(consumers.ToArray(), "Apply Viewport To Children");

					foreach (var element in consumers)
					{
						((IViewportConsumer)element).SetViewport(provider.viewport);
						EditorUtility.SetDirty(element);
					}
				}
			}
			GUI.enabled = true;

			if (!ReferenceEquals(provider.viewport, viewport))
			{
				Undo.RecordObject(provider, "Viewport Provider");
				provider.viewport = viewport;
				EditorUtility.SetDirty(provider);
			}
		}
	}
}
