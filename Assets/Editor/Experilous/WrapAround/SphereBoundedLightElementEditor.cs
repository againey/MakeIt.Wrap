using UnityEngine;
using UnityEditor;

namespace Experilous.WrapAround
{
	[CustomEditor(typeof(SphereBoundedLightElement))]
	public class SphereBoundedLightElementEditor : LightElementEditor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var element = (SphereBoundedLightElement)target;

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Sphere Bounds", EditorStyles.boldLabel);
			element.radius = EditorGUILayout.FloatField("Radius", element.radius);
		}
	}
}
