/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using UnityEditor;

namespace Experilous.WrapAround
{
	[CustomEditor(typeof(Collider2DElement))]
	public class Collider2DElementEditor : GhostableElementEditor<Collider2DElement, Collider2DElementGhost>
	{
		protected override void OnElementGUI(Collider2DElement element)
		{
			element.world = (World)EditorGUILayout.ObjectField(new GUIContent("World", "The world component with wrap-around behavior to which this collider conforms."), element.world, typeof(World), true);

			GUILayout.Space(EditorGUIUtility.singleLineHeight);

			if (ElementBoundsEditorUtility.OnInspectorGUI(element))
			{
				SceneView.RepaintAll();
			}

			GUILayout.Space(EditorGUIUtility.singleLineHeight);
		}

		[DrawGizmo(GizmoType.Active)]
		private static void DrawGizmoSelected(Collider2DElement element, GizmoType gizmoType)
		{
			DrawGizmoSelected(element, gizmoType, new Color(1f, 1f, 1f, 0.5f), new Color(1f, 1f, 1f, 0.25f));
		}
	}
}
