/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.MakeItWrap
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
		Viewport GetViewport();
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

		public static TViewport FindWorld<TViewport>(Component component) where TViewport : Viewport
		{
			var provider = component.GetComponentInParent<ViewportProvider>();
			while (provider != null)
			{
				if (provider.viewport is TViewport)
				{
					return (TViewport)provider.viewport;
				}
				else
				{
					var parent = provider.transform.parent;
					if (parent == null) return null;
					provider = component.GetComponentInParent<ViewportProvider>();
				}
			}
			return null;
		}
	}
}
