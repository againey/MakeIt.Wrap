﻿/******************************************************************************\
 *  Copyright (C) 2016 Experilous <againey@experilous.com>
 *  
 *  This file is subject to the terms and conditions defined in the file
 *  'Assets/Plugins/Experilous/License.txt', which is a part of this package.
 *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	/// <summary>
	/// An interface for any component which needs to have a world reference in order to function.
	/// By implementing this interface, a component will automatically integrate with world providers.
	/// </summary>
	/// <seealso cref="World"/>
	/// <seealso cref="WorldProvider"/>
	public interface IWorldConsumer
	{
		bool hasWorld { get; }
		void SetWorld(World world);
	}

	public static class WorldConsumerUtility
	{
		public static World FindWorld(Component component)
		{
			var provider = component.GetComponentInParent<WorldProvider>();
			if (provider != null)
			{
				return provider.world;
			}
			else
			{
				return null;
			}
		}

		public static TWorld FindWorld<TWorld>(Component component) where TWorld : World
		{
			var provider = component.GetComponentInParent<WorldProvider>();
			while (provider != null)
			{
				if (provider.world is TWorld)
				{
					return (TWorld)provider.world;
				}
				else
				{
					var parent = provider.transform.parent;
					if (parent == null) return null;
					provider = component.GetComponentInParent<WorldProvider>();
				}
			}
			return null;
		}
	}
}
