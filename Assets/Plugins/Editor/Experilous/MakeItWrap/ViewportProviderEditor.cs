/******************************************************************************\
* Copyright Andy Gainey                                                        *
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

			Viewport viewport = null;

			viewport = (Viewport)EditorGUILayout.ObjectField(new GUIContent("Viewport", "The viewport component to be provided to any descendant viewport consumers."), provider.viewport, typeof(Viewport), true);

			GUI.enabled = (viewport != null);
			if (GUILayout.Button(new GUIContent("Apply to Unset Children", "Search through all descendants for any components that implement IViewportConsumer, and assign it this provider's viewport if the component doesn't already have a viewport assigned.")))
			{
				provider.ApplyToUnsetConsumers(provider.gameObject, true);
			}

			if (GUILayout.Button(new GUIContent("Apply to All Children", "Search through all descendants for any components that implement IViewportConsumer, and assign it this provider's viewport, overwriting any viewport it might have already been assigned.")))
			{
				provider.ApplyToAllConsumers(provider.gameObject, true);
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
