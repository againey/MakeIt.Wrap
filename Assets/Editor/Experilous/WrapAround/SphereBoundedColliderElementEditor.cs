using UnityEngine;
using UnityEditor;

namespace Experilous.WrapAround
{
	[CustomEditor(typeof(SphereBoundedColliderElement))]
	public class SphereBoundedColliderElementEditor : ColliderElementEditor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var element = (SphereBoundedColliderElement)target;

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Sphere Bounds", EditorStyles.boldLabel);
			element.radius = EditorGUILayout.FloatField("Radius", element.radius);
		}
	}
}
