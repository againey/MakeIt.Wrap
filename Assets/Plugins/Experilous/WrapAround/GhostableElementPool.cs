/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using System.Collections.Generic;

namespace Experilous.WrapAround
{
	public abstract class GhostableElementPool<TBounds> : MonoBehaviour where TBounds : ElementBounds
	{
		protected GhostableElement2[] _elements;
		protected Transform[] _elementTransforms;
		protected TBounds[] _elementBounds;
		protected int[] _firstGhostIndices;
		protected int[] _elementGhostPrefabInstanceIDs;
		protected int _nextFreeElementIndex;
		protected int _elementPoolSize;

		protected Ghost2[] _ghosts;
		protected Transform[] _ghostTransforms;
		protected int[] _nextGhostIndices;
		protected GhostRegion[] _ghostRegions;
		protected int _nextFreeGhostIndex;
		protected int _ghostPoolSize;

		protected Dictionary<int, List<Ghost2>> _ghostPools;

		public void AddElement(GhostableElement2 element, TBounds bounds)
		{
			int elementIndex;
			if (_nextFreeElementIndex < _elementPoolSize)
			{
				elementIndex = _nextFreeElementIndex;
				_nextFreeElementIndex = _firstGhostIndices[_nextFreeElementIndex];
			}
			else
			{
				var oldLength = _elements.Length;
				if (_nextFreeElementIndex >= oldLength)
				{
					var newLength = (oldLength * 3) / 2 + 1; //oldLength * 1.5 + 1
					var newElements = new GhostableElement2[newLength];
					var newElementTransforms = new Transform[newLength];
					var newElementBounds = new TBounds[newLength];
					var newFirstGhostIndices = new int[newLength];
					var newElementGhostPrefabInstanceIDs = new int[newLength];

					if (oldLength > 0)
					{
						System.Array.Copy(_elements, newElements, oldLength);
						System.Array.Copy(_elementTransforms, newElementTransforms, oldLength);
						System.Array.Copy(_elementBounds, newElementBounds, oldLength);
						System.Array.Copy(_firstGhostIndices, newFirstGhostIndices, oldLength);
						System.Array.Copy(_elementGhostPrefabInstanceIDs, newElementGhostPrefabInstanceIDs, oldLength);
					}
				}

				elementIndex = _nextFreeElementIndex;
				++_nextFreeElementIndex;
				_elementPoolSize = _nextFreeElementIndex;
			}

			_elements[elementIndex] = element;
			_elementTransforms[elementIndex] = element.transform;
			_elementBounds[elementIndex] = bounds;
			_firstGhostIndices[elementIndex] = -1;
			_elementGhostPrefabInstanceIDs[elementIndex] = element.ghostPrefab.GetInstanceID();
		}

		protected void RemoveElement(int elementIndex)
		{
			_firstGhostIndices[elementIndex] = _nextFreeElementIndex;
			_nextFreeElementIndex = elementIndex;
		}

		protected int FindGhostIndex(int elementIndex, GhostRegion ghostRegion)
		{
			int ghostIndex = _firstGhostIndices[elementIndex];
			while (ghostIndex != -1)
			{
				if (ReferenceEquals(ghostRegion, _ghostRegions[ghostIndex]))
				{
					return ghostIndex;
				}
				ghostIndex = _nextGhostIndices[ghostIndex];
			}
			return -1;
		}

		protected void InstantiateGhost(int elementIndex, GhostRegion ghostRegion)
		{
			var element = _elements[elementIndex];
			var elementTransform = _elementTransforms[elementIndex];

			int ghostPrefabInstanceID = _elementGhostPrefabInstanceIDs[elementIndex];
			List<Ghost2> ghostPool;
			Ghost2 ghost = null;
			if (_ghostPools.TryGetValue(ghostPrefabInstanceID, out ghostPool) && ghostPool.Count > 0)
			{
				ghost = ghostPool[ghostPool.Count - 1];
				ghostPool.RemoveAt(ghostPool.Count - 1);
			}
			else
			{
				ghost = Instantiate(element.ghostPrefab);
			}

			var ghostIndex = AddGhost(elementIndex);
			var ghostTransform = ghost.transform;

			ghostTransform.SetParent(elementTransform.parent, false);
			ghost.name = element.name + " (Ghost)";
			_ghosts[ghostIndex] = ghost;
			_ghostTransforms[ghostIndex] = ghost.transform;
			_ghostRegions[ghostIndex] = ghostRegion;

			ghostTransform.localScale = elementTransform.localScale;
			ghostRegion.Transform(elementTransform, ghostTransform);
		}

		protected int AddGhost(int elementIndex)
		{
			int ghostIndex;
			if (_nextFreeGhostIndex < _ghostPoolSize)
			{
				ghostIndex = _nextFreeGhostIndex;
				_nextFreeGhostIndex = _nextGhostIndices[_nextFreeGhostIndex];
			}
			else
			{
				var oldLength = _ghosts.Length;
				if (_nextFreeGhostIndex >= oldLength)
				{
					var newLength = (oldLength * 3) / 2 + 1; //oldLength * 1.5 + 1
					var newGhosts = new Ghost2[newLength];
					var newGhostTransforms = new Transform[newLength];
					var newNextGhostIndices = new int[newLength];
					var newGhostRegions = new GhostRegion[newLength];

					if (oldLength > 0)
					{
						System.Array.Copy(_ghosts, newGhosts, oldLength);
						System.Array.Copy(_ghostTransforms, newGhostTransforms, oldLength);
						System.Array.Copy(_nextGhostIndices, newNextGhostIndices, oldLength);
						System.Array.Copy(_ghostRegions, newGhostRegions, oldLength);
					}
				}

				ghostIndex = _nextFreeGhostIndex;
				++_nextFreeGhostIndex;
				_ghostPoolSize = _nextFreeGhostIndex;
			}

			var nextGhostIndex = _firstGhostIndices[elementIndex];
			_firstGhostIndices[elementIndex] = ghostIndex;
			_nextGhostIndices[ghostIndex] = nextGhostIndex;

			return ghostIndex;
		}

		protected int RemoveGhost(int ghostIndex, int elementIndex, int priorGhostIndex)
		{
			int ghostPrefabInstanceID = _elementGhostPrefabInstanceIDs[elementIndex];
			List<Ghost2> ghostPool;
			if (!_ghostPools.TryGetValue(ghostPrefabInstanceID, out ghostPool))
			{
				ghostPool = new List<Ghost2>();
				_ghostPools.Add(ghostPrefabInstanceID, ghostPool);
			}

			ghostPool.Add(_ghosts[ghostIndex]);

			var nextGhostIndex = _nextGhostIndices[ghostIndex];
			_nextGhostIndices[ghostIndex] = _nextFreeGhostIndex;
			_nextFreeGhostIndex = ghostIndex;

			if (priorGhostIndex != -1)
			{
				_nextGhostIndices[priorGhostIndex] = ghostIndex;
			}
			else
			{
				_firstGhostIndices[elementIndex] = ghostIndex;
			}

			return nextGhostIndex;
		}
	}
}
