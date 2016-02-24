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
	/// An interface for any component which needs to have a viewport reference in order to function.
	/// By implementing this interface, a component will automatically integrate with viewport providers.
	/// </summary>
	/// <seealso cref="Viewport"/>
	/// <seealso cref="ViewportProvider"/>
	public interface IViewportConsumer
	{
		bool hasViewport { get; }
		void SetViewport(Viewport viewport);
	}

	public static class ViewportConsumerUtility
	{
		public static Viewport FindViewport(Component component)
		{
			var provider = component.GetComponentInParent<ViewportProvider>();
			if (provider != null)
			{
				return provider.viewport;
			}
			else
			{
				return null;
			}
		}
	}
}
