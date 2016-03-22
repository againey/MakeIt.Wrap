/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using UnityEditor;

namespace Experilous.WrapAround
{
	[CustomEditor(typeof(SpriteElement))]
	public class SpriteElementEditor : GhostableElementEditor<SpriteElement, SpriteElementGhost>
	{
		protected override void OnElementGUI(SpriteElement element)
		{
			element.viewport = (Viewport)EditorGUILayout.ObjectField(new GUIContent("Viewport", "The viewport component with wrap-around behavior with which this sprite interacts."), element.viewport, typeof(Viewport), true);

			GUILayout.Space(EditorGUIUtility.singleLineHeight);

			if (ElementBoundsEditorUtility.OnInspectorGUI(element))
			{
				SceneView.RepaintAll();
			}

			GUILayout.Space(EditorGUIUtility.singleLineHeight);
		}

		[DrawGizmo(GizmoType.Active)]
		private static void DrawGizmoSelected(SpriteElement element, GizmoType gizmoType)
		{
			DrawGizmoSelected(element, gizmoType, new Color(0f, 0.5f, 1f, 0.5f), new Color(0.25f, 0.5f, 1f, 0.25f));
		}
	}
}
