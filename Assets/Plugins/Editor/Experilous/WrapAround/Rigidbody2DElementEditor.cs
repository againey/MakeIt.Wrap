﻿/******************************************************************************\
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
	[CustomEditor(typeof(Rigidbody2DElement))]
	public class Rigidbody2DElementEditor : GhostableElementEditor<Rigidbody2DElement, Rigidbody2DElementGhost>
	{
		protected override void OnElementGUI(Rigidbody2DElement element)
		{
			element.world = (World)EditorGUILayout.ObjectField("World", element.world, typeof(World), true);
			element.bounds = (AbstractBounds)EditorGUILayout.ObjectField("Bounds", element.bounds, typeof(AbstractBounds), true);
		}

		protected override bool IsExcluded(Component[] components)
		{
			foreach (var component in components)
			{
				if (component is Rigidbody2D)
				{
					return true;
				}
			}
			return false;
		}

		protected override bool IsNecessary(Component[] components)
		{
			foreach (var component in components)
			{
				if (component is Collider2D)
				{
					return true;
				}
			}
			return false;
		}

		protected override void RemoveUnnecessaryComponents(Component[] components)
		{
			RemoveAll(components, (Component component) => { return !(component is Rigidbody2D || component is Collider2D || component is Transform); });
		}
	}
}