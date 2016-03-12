﻿/******************************************************************************\
 *  Copyright (C) 2016 Experilous <againey@experilous.com>
 *  
 *  This file is subject to the terms and conditions defined in the file
 *  'Assets/Plugins/Experilous/License.txt', which is a part of this package.
 *
\******************************************************************************/

using UnityEngine;
using UnityEditor;
using System;

namespace Experilous.WrapAround
{
	public static class ElementBoundsEditorUtility
	{
		private static string[] _sourceLabels = new string[]
			{
				"None",
				"Local Origin",
				"Automatic",
				"Automatic (Axis Aligned Box)",
				"Automatic (Sphere)",
				"Manual (Provider Component)",
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

			sourceIndex = EditorGUILayout.Popup("Bounds", sourceIndex, _sourceLabels);

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

			fixedScale = EditorGUILayout.Toggle("Fixed Scale", fixedScale);

			if ((source & ElementBoundsSource.Source) != ElementBoundsSource.AutomaticAxisAlignedBox)
			{
				fixedRotation = EditorGUILayout.Toggle("Fixed Rotation", fixedRotation);
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

				provider = (ElementBoundsProvider)EditorGUILayout.ObjectField("Bounds Provider", provider, typeof(ElementBoundsProvider), true);
			}
			else
			{
				provider = null;
			}

			var changed = EditorGUI.EndChangeCheck();

			return (GUILayout.Button("Refresh Bounds") || changed);
		}
	}
}