/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using UnityEditor;

namespace MakeIt.Wrap
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

			if (ElementBoundsEditorUtility.OnInspectorGUI(element, ref element.boundsSource, ref element.boundsProvider))
			{
				element.RefreshBounds();
			}

			if (EditorGUI.EndChangeCheck())
			{
				EditorUtility.SetDirty(element);
			}
		}
	}
}
