/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using UnityEditor;

namespace Experilous.WrapAround
{
	[CustomEditor(typeof(WrapAroundElement))]
	public class WrapAroundElementEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			var element = (WrapAroundElement)target;

			Undo.RecordObject(element, typeof(WrapAroundElement).GetPrettyName());

			EditorGUI.BeginChangeCheck();

			element.world = (WrapAroundWorld)EditorGUILayout.ObjectField(new GUIContent("World"), element.world, typeof(WrapAroundWorld), true);

			//TODO

			if (EditorGUI.EndChangeCheck())
			{
				EditorUtility.SetDirty(element);
			}
		}

		[MenuItem("CONTEXT/WrapAroundElement/Remove Component")]
		private static void RemoveComponent(MenuCommand command)
		{
			throw new System.InvalidOperationException();
		}

		[MenuItem("CONTEXT/WrapAroundElement/Remove Component", validate = true)]
		private static bool CanRemoveComponent(MenuCommand command)
		{
			return false;
		}
	}
}
