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
			if (GUILayout.Button("Push to Unset Children"))
			{
				foreach (var element in provider.GetComponentsInChildren<RenderableElement>())
				{
					if (element.viewport == null)
					{
						element.viewport = provider.viewport;
						EditorUtility.SetDirty(element);
					}
				}

				foreach (var element in provider.GetComponentsInChildren<LightElement>())
				{
					if (element.viewport == null)
					{
						element.viewport = provider.viewport;
						EditorUtility.SetDirty(element);
					}
				}
			}
			GUI.enabled = true;

			GUI.enabled = (provider.viewport != null);
			if (GUILayout.Button("Push to All Children"))
			{
				foreach (var element in provider.GetComponentsInChildren<RenderableElement>())
				{
					element.viewport = provider.viewport;
					EditorUtility.SetDirty(element);
				}

				foreach (var element in provider.GetComponentsInChildren<LightElement>())
				{
					element.viewport = provider.viewport;
					EditorUtility.SetDirty(element);
				}
			}
			GUI.enabled = true;
		}
	}
}
