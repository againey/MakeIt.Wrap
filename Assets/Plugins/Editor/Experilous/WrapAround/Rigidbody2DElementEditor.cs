﻿/******************************************************************************\
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
	[CustomEditor(typeof(Rigidbody2DElement))]
	public class Rigidbody2DElementEditor : GhostableElementEditor<Rigidbody2DElement, Rigidbody2DElementGhost>
	{
		protected override void OnElementGUI(Rigidbody2DElement element)
		{
			element.world = (World)EditorGUILayout.ObjectField("World", element.world, typeof(World), true);

			GUILayout.Space(EditorGUIUtility.singleLineHeight);

			if (ElementBoundsEditorUtility.OnInspectorGUI(element, ref element.boundsSource, ref element.boundsProvider))
			{
				element.RefreshBounds();
			}

			GUILayout.Space(EditorGUIUtility.singleLineHeight);
		}
	}
}
