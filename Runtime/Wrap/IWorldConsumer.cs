/******************************************************************************\
* Copyright Andy Gainey                                                        *
*                                                                              *
* Licensed under the Apache License, Version 2.0 (the "License");              *
* you may not use this file except in compliance with the License.             *
* You may obtain a copy of the License at                                      *
*                                                                              *
*     http://www.apache.org/licenses/LICENSE-2.0                               *
*                                                                              *
* Unless required by applicable law or agreed to in writing, software          *
* distributed under the License is distributed on an "AS IS" BASIS,            *
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.     *
* See the License for the specific language governing permissions and          *
* limitations under the License.                                               *
\******************************************************************************/

using UnityEngine;

namespace MakeIt.Wrap
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
		World GetWorld();
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
