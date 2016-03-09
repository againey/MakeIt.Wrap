/******************************************************************************\
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
	[CustomEditor(typeof(SpriteElement))]
	public class SpriteElementEditor : GhostableElementEditor<SpriteElement, SpriteElementGhost>
	{
		protected override void OnElementGUI(SpriteElement element)
		{
			element.viewport = (Viewport)EditorGUILayout.ObjectField("Viewport", element.viewport, typeof(Viewport), true);

			if (ElementBoundsEditorUtility.OnInspectorGUI(element, ref element.boundsSource, ref element.boundsProvider))
			{
				element.RefreshBounds();
			}
		}

		protected override bool IsExcluded(Component[] components)
		{
			return false;
		}

		protected override bool IsNecessary(Component[] components)
		{
			foreach (var component in components)
			{
				if (component is SpriteRenderer)
				{
					return true;
				}
			}
			return false;
		}

		protected override void RemoveUnnecessaryComponents(Component[] components)
		{
			RemoveAll(components, (Component component) => { return !(component is SpriteRenderer || component is Transform); });
		}
	}
}
