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
			if (GUILayout.Button("Apply to Unset Children"))
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
			if (GUILayout.Button("Apply to All Children"))
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
