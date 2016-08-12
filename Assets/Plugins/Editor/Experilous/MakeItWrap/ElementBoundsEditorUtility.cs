/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using UnityEditor;
using System;

namespace Experilous.MakeIt.Wrap
{
	public static class ElementBoundsEditorUtility
	{
		private static GUIContent[] _sourceLabels = new GUIContent[]
			{
				new GUIContent("None", "No bounding volume will be created for this wrap-around component."),
				new GUIContent("Local Origin"),
				new GUIContent("Automatic"),
				new GUIContent("Automatic (Axis Aligned Box)"),
				new GUIContent("Automatic (Sphere)"),
				new GUIContent("Manual (Provider Component)"),
			};

		public static bool OnInspectorGUI(BoundedElement element, ref ElementBoundsSource source, ref ElementBoundsProvider provider)
		{
			EditorGUI.BeginChangeCheck();

			int sourceIndex;
			switch (source & ElementBoundsSource.Source)
			{
				case ElementBoundsSource.None: sourceIndex = 0; break;
				case ElementBoundsSource.LocalOrigin: sourceIndex = 1; break;
				case ElementBoundsSource.Automatic: sourceIndex = 2; break;
				case ElementBoundsSource.AutomaticAxisAlignedBox: sourceIndex = 3; break;
				case ElementBoundsSource.AutomaticSphere: sourceIndex = 4; break;
				case ElementBoundsSource.Manual: sourceIndex = 5; break;
				default: throw new NotImplementedException();
			}

			sourceIndex = EditorGUILayout.Popup(new GUIContent("Bounds", "The method by which a bounding volume is obtained for this wrap-around element, which will affect when the appropriate warp-around behavior is applied."), sourceIndex, _sourceLabels);

			bool fixedScale = (source & ElementBoundsSource.FixedScale) != 0;
			bool fixedRotation = (source & ElementBoundsSource.FixedRotation) != 0;

			switch (sourceIndex)
			{
				case 0: source = ElementBoundsSource.None; break;
				case 1: source = ElementBoundsSource.LocalOrigin; break;
				case 2: source = ElementBoundsSource.Automatic; break;
				case 3: source = ElementBoundsSource.AutomaticAxisAlignedBox; break;
				case 4: source = ElementBoundsSource.AutomaticSphere; break;
				case 5: source = ElementBoundsSource.Manual; break;
				default: throw new NotImplementedException();
			}

			fixedScale = EditorGUILayout.Toggle(new GUIContent("Fixed Scale", "This element's transform scale is never expected to change anywhere in its ancestry."), fixedScale);

			if ((source & ElementBoundsSource.Source) != ElementBoundsSource.AutomaticAxisAlignedBox)
			{
				fixedRotation = EditorGUILayout.Toggle(new GUIContent("Fixed Rotation", "This element's transform rotation is never expected to change anywhere in its ancestry."), fixedRotation);
			}
			else
			{
				var priorEnabledState = GUI.enabled;
				GUI.enabled = false;
				EditorGUILayout.Toggle(new GUIContent("Fixed Rotation", "This element's transform rotation is never expected to change anywhere in its ancestry."), true);
				GUI.enabled = priorEnabledState;
			}

			if (fixedScale) source |= ElementBoundsSource.FixedScale;
			if (fixedRotation) source |= ElementBoundsSource.FixedRotation;

			if ((source & ElementBoundsSource.Source) == ElementBoundsSource.Manual)
			{
				if (provider == null)
				{
					var providers = element.GetComponents<ElementBoundsProvider>();
					if (providers != null && providers.Length == 1)
					{
						provider = providers[0];
					}
				}

				provider = (ElementBoundsProvider)EditorGUILayout.ObjectField(new GUIContent("Bounds Provider", "The manually specified source for setting this wrap-around element's bounding volume."), provider, typeof(ElementBoundsProvider), true);
			}
			else
			{
				provider = null;
			}

			var changed = EditorGUI.EndChangeCheck();

			return (GUILayout.Button(new GUIContent("Refresh Bounds", "Recalculate this element's bounding volume based on the above settings and the configuration of all relevant game objects and components that may have changed.")) || changed);
		}
	}
}
