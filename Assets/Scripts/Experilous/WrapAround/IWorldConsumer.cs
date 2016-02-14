using UnityEngine;

namespace Experilous.WrapAround
{
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
