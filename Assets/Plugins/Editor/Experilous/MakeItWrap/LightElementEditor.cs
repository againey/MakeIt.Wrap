/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using UnityEditor;

namespace Experilous.MakeItWrap
{
	[CustomEditor(typeof(LightElement))]
	public class LightElementEditor : GhostableElementEditor<LightElement, LightElementGhost>
	{
		protected override void OnElementGUI(LightElement element)
		{
			element.viewport = (Viewport)EditorGUILayout.ObjectField(new GUIContent("Viewport", "The viewport component with wrap-around behavior with which this light interacts."), element.viewport, typeof(Viewport), true);

			GUILayout.Space(EditorGUIUtility.singleLineHeight);

			if (ElementBoundsEditorUtility.OnInspectorGUI(element, ref element.boundsSource, ref element.boundsProvider))
			{
				element.RefreshBounds();
			}

			GUILayout.Space(EditorGUIUtility.singleLineHeight);
		}
	}
}
