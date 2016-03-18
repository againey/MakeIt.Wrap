/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using UnityEditor;

namespace Experilous.WrapAround
{
	[CustomEditor(typeof(ColliderElement))]
	public class ColliderElementEditor : GhostableElementEditor<ColliderElement, ColliderElementGhost>
	{
		protected override void OnElementGUI(ColliderElement element)
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
