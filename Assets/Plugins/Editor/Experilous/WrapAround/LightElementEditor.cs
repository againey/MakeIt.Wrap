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
	[CustomEditor(typeof(LightElement))]
	public class LightElementEditor : GhostableElementEditor<LightElement, LightElementGhost>
	{
		protected override void OnElementGUI(LightElement element)
		{
			element.viewport = (Viewport)EditorGUILayout.ObjectField("Viewport", element.viewport, typeof(Viewport), true);
			element.bounds = (AbstractBounds)EditorGUILayout.ObjectField("Bounds", element.bounds, typeof(AbstractBounds), true);
		}

		protected override bool IsExcluded(Component[] components)
		{
			return false;
		}

		protected override bool IsNecessary(Component[] components)
		{
			foreach (var component in components)
			{
				if (component is Light)
				{
					return true;
				}
			}
			return false;
		}

		protected override void RemoveUnnecessaryComponents(Component[] components)
		{
			RemoveAll(components, (Component component) => { return !(component is Light || component is Transform); });
		}
	}
}
