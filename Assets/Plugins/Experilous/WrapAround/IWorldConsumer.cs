/******************************************************************************\
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
	}
}
