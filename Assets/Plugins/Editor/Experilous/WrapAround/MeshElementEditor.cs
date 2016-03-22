/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using UnityEditor;

namespace Experilous.WrapAround
{
	[CustomEditor(typeof(MeshElement))]
	public class MeshElementEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			var element = (MeshElement)target;

			Undo.RecordObject(element, "Mesh Element");

			EditorGUI.BeginChangeCheck();

			element.viewport = (Viewport)EditorGUILayout.ObjectField(new GUIContent("Viewport", "The viewport component with wrap-around behavior with which this mesh interacts."), element.viewport, typeof(Viewport), true);

			GUILayout.Space(EditorGUIUtility.singleLineHeight);

			if (ElementBoundsEditorUtility.OnInspectorGUI(element))
			{
				SceneView.RepaintAll();
			}

			if (EditorGUI.EndChangeCheck())
			{
				EditorUtility.SetDirty(element);
			}
		}

		[DrawGizmo(GizmoType.Active)]
		private static void DrawGizmoSelected(MeshElement element, GizmoType gizmoType)
		{
			if (element.bounds == null) return;

			var bounds = element.bounds;
			var originalColor = new Color(0f, 0.5f, 1f, 0.5f);
			ElementBoundsEditorUtility.DrawGizmosSelected(bounds, originalColor);

			if (element.viewport == null) return;
			var viewport = element.viewport;
			var ghostColor = new Color(0.25f, 0.5f, 1f, 0.25f);

			foreach (var ghostRegion in viewport.visibleGhostRegions)
			{
				if (bounds.IsVisible(viewport, ghostRegion))
				{
					ElementBoundsEditorUtility.DrawGizmosSelected(bounds, ghostRegion, ghostColor);
				}
			}
		}
	}
}
