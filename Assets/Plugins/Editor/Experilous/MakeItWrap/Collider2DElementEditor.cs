/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using UnityEditor;

namespace Experilous.MakeItWrap
{
	[CustomEditor(typeof(Collider2DElement))]
	public class Collider2DElementEditor : GhostableElementEditor<Collider2DElement, Collider2DElementGhost>
	{
		protected override void OnElementGUI(Collider2DElement element)
		{
			element.world = (World)EditorGUILayout.ObjectField(new GUIContent("World", "The world component with wrap-around behavior to which this collider conforms."), element.world, typeof(World), true);

			GUILayout.Space(EditorGUIUtility.singleLineHeight);

			if (ElementBoundsEditorUtility.OnInspectorGUI(element, ref element.boundsSource, ref element.boundsProvider))
			{
				element.RefreshBounds();
			}

			GUILayout.Space(EditorGUIUtility.singleLineHeight);
		}
	}
}
