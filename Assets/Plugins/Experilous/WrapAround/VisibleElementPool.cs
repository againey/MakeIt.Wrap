/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using System.Collections.Generic;

namespace Experilous.WrapAround
{
	public class VisibleElementPool<TBounds, TWorld> : GhostableElementPool<TBounds> where TBounds : ElementBounds where TWorld : World
	{
		protected TWorld _world;
		protected LayerMask _layers;

		protected void LateUpdate()
		{
			for (int elementIndex = 0; elementIndex < _elements.Length; ++elementIndex)
			{
				var element = _elements[elementIndex];
				if (element == null)
				{
					RemoveElement(elementIndex);
				}
				else
				{
					var elementBounds = _elementBounds[elementIndex];
					int ghostIndex = _firstGhostIndices[elementIndex];
					int priorGhostIndex = -1;
					while (ghostIndex != -1)
					{
						var ghostRegion = _ghostRegions[ghostIndex];
						if (ghostRegion == null || !ghostRegion.isActive || !elementBounds.IsVisible(_world, _layers, _ghostTransforms[ghostIndex]))
						{
							ghostIndex = RemoveGhost(ghostIndex, elementIndex, priorGhostIndex);
						}
						else
						{
							priorGhostIndex = ghostIndex;
							ghostIndex = _nextGhostIndices[ghostIndex];
						}
					}
				}
			}

			foreach (var ghostRegion in _world.GetVisibleGhostRegions(_layers))
			{
				for (int elementIndex = 0; elementIndex < _elements.Length; ++elementIndex)
				{
					if (FindGhostIndex(elementIndex, ghostRegion) == -1 && _elementBounds[elementIndex].IsVisible(_world, _layers, _elementTransforms[elementIndex], ghostRegion))
					{
						InstantiateGhost(elementIndex, ghostRegion);
					}
				}
			}
		}
	}
}
