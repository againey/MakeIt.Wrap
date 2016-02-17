using UnityEngine;

namespace Experilous.WrapAround
{
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
