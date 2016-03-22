/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using UnityEditor;

namespace Experilous.WrapAround
{
	[CustomEditor(typeof(LightElement))]
	public class LightElementEditor : GhostableElementEditor<LightElement, LightElementGhost>
	{
		protected override void OnElementGUI(LightElement element)
		{
			element.viewport = (Viewport)EditorGUILayout.ObjectField(new GUIContent("Viewport", "The viewport component with wrap-around behavior with which this light interacts."), element.viewport, typeof(Viewport), true);

			GUILayout.Space(EditorGUIUtility.singleLineHeight);

			if (ElementBoundsEditorUtility.OnInspectorGUI(element))
			{
				SceneView.RepaintAll();
			}

			GUILayout.Space(EditorGUIUtility.singleLineHeight);
		}

		[DrawGizmo(GizmoType.Active)]
		private static void DrawGizmoSelected(LightElement element, GizmoType gizmoType)
		{
			DrawGizmoSelected(element, gizmoType, new Color(1f, 1f, 1f, 0.5f), new Color(1f, 1f, 1f, 0.25f));
		}
	}
}
