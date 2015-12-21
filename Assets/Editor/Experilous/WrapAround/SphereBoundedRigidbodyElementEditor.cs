using UnityEngine;
using UnityEditor;

namespace Experilous.WrapAround
{
	[CustomEditor(typeof(SphereBoundedRigidbodyElement))]
	public class SphereBoundedRigidbodyElementEditor : RigidbodyElementEditor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var element = (SphereBoundedRigidbodyElement)target;

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Sphere Bounds", EditorStyles.boldLabel);
			element.radius = EditorGUILayout.FloatField("Radius", element.radius);
		}
	}
}
