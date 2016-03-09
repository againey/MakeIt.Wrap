/******************************************************************************\
 *  Copyright (C) 2016 Experilous <againey@experilous.com>
 *  
 *  This file is subject to the terms and conditions defined in the file
 *  'Assets/Plugins/Experilous/License.txt', which is a part of this package.
 *
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

			element.viewport = (Viewport)EditorGUILayout.ObjectField("Viewport", element.viewport, typeof(Viewport), true);

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
