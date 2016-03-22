/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using UnityEditor;

namespace Experilous.WrapAround
{
	[CustomEditor(typeof(RigidbodyElement))]
	public class RigidbodyElementEditor : GhostableElementEditor<RigidbodyElement, RigidbodyElementGhost>
	{
		protected override void OnElementGUI(RigidbodyElement element)
		{
			element.world = (World)EditorGUILayout.ObjectField(new GUIContent("World", "The world component with wrap-around behavior to which this rigidbody conforms."), element.world, typeof(World), true);

			GUILayout.Space(EditorGUIUtility.singleLineHeight);

			if (ElementBoundsEditorUtility.OnInspectorGUI(element))
			{
				SceneView.RepaintAll();
			}

			GUILayout.Space(EditorGUIUtility.singleLineHeight);
		}

		[DrawGizmo(GizmoType.Active)]
		private static void DrawGizmoSelected(RigidbodyElement element, GizmoType gizmoType)
		{
			DrawGizmoSelected(element, gizmoType, new Color(1f, 1f, 1f, 0.5f), new Color(1f, 1f, 1f, 0.25f));
		}
	}
}
