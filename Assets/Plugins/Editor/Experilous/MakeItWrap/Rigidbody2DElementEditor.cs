/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using UnityEditor;

namespace Experilous.MakeItWrap
{
	[CustomEditor(typeof(Rigidbody2DElement))]
	public class Rigidbody2DElementEditor : GhostableElementEditor<Rigidbody2DElement, Rigidbody2DElementGhost>
	{
		protected override void OnElementGUI(Rigidbody2DElement element)
		{
			element.world = (World)EditorGUILayout.ObjectField(new GUIContent("World", "The world component with wrap-around behavior to which this rigidbody conforms."), element.world, typeof(World), true);

			GUILayout.Space(EditorGUIUtility.singleLineHeight);

			if (ElementBoundsEditorUtility.OnInspectorGUI(element, ref element.boundsSource, ref element.boundsProvider))
			{
				element.RefreshBounds();
			}

			GUILayout.Space(EditorGUIUtility.singleLineHeight);
		}
	}
}
