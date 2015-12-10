using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Experilous.WrapAround
{
	public class AxisAlignedWrapX2DWorld : World
	{
		public float minX;
		public float maxX;

		private AxisAlignedWrapX2DGhostRegion[] _ghostRegions = null;
		private int _ghostRegionsXIndexOffset = 0;

		protected void Start()
		{
			_ghostRegions = new AxisAlignedWrapX2DGhostRegion[3];

			var worldWidth = width;
			_ghostRegions[0] = new AxisAlignedWrapX2DGhostRegion(-worldWidth);
			_ghostRegions[1] = null;
			_ghostRegions[2] = new AxisAlignedWrapX2DGhostRegion(+worldWidth);

			_ghostRegionsXIndexOffset = -1;
		}

		public float width { get { return maxX - minX; } }

		public override IEnumerable<GhostRegion> GetVisibleGhostRegions(AxisAligned2DViewport viewport)
		{
			var worldWidth = width;

			var viewportMinX = viewport.min.x;
			var viewportMaxX = viewport.max.x;

			int xIndexMin = Mathf.FloorToInt((viewportMinX - minX) / worldWidth);
			int xIndexMax = Mathf.CeilToInt((viewportMaxX - maxX) / worldWidth);

			ExpandGhostRegions(xIndexMin);
			ExpandGhostRegions(xIndexMax);

			return new GhostRegionRange(this, xIndexMin - _ghostRegionsXIndexOffset, xIndexMax - _ghostRegionsXIndexOffset + 1);
		}

		private void ExpandGhostRegions(int xIndex)
		{
			if (xIndex < _ghostRegionsXIndexOffset)
			{
				var oldLength = _ghostRegions.Length;
				var newLength = oldLength * 3 / 2;
				var delta = newLength - oldLength;
				var newGhostRegions = new AxisAlignedWrapX2DGhostRegion[newLength];
				Array.Copy(_ghostRegions, 0, newGhostRegions, delta, oldLength);
				_ghostRegions = newGhostRegions;
				_ghostRegionsXIndexOffset -= delta;

				var worldWidth = width;
				for (int i = 0; i < delta; ++i)
				{
					_ghostRegions[i] = new AxisAlignedWrapX2DGhostRegion((_ghostRegionsXIndexOffset + i) * worldWidth);
				}
			}
			else if (xIndex >= _ghostRegions.Length + _ghostRegionsXIndexOffset)
			{
				var oldLength = _ghostRegions.Length;
				var newLength = oldLength * 3 / 2;
				var newGhostRegions = new AxisAlignedWrapX2DGhostRegion[newLength];
				Array.Copy(_ghostRegions, 0, newGhostRegions, 0, oldLength);
				_ghostRegions = newGhostRegions;

				var worldWidth = width;
				for (int i = oldLength; i < newLength; ++i)
				{
					_ghostRegions[i] = new AxisAlignedWrapX2DGhostRegion((_ghostRegionsXIndexOffset + i) * worldWidth);
				}
			}
		}

		public override void Confine(Element element)
		{
			var position = element.transform.position;
			var xOffset = position.x - minX;
			var worldWidth = width;
			position.x = xOffset - Mathf.Floor(xOffset / worldWidth) * worldWidth + minX;
			element.transform.position = position;
		}

		public struct GhostRegionRange : IEnumerable<AxisAlignedWrapX2DGhostRegion>, IEnumerable<GhostRegion>
		{
			private AxisAlignedWrapX2DWorld _world;
			private int _first;
			private int _last;

			public GhostRegionRange(AxisAlignedWrapX2DWorld world, int first, int last)
			{
				_world = world;
				_first = first;
				_last = last;
			}

			public IEnumerator<AxisAlignedWrapX2DGhostRegion> GetEnumerator()
			{
				return new GhostRegionEnumerator(_world, _first, _last);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new GhostRegionEnumerator(_world, _first, _last);
			}

			IEnumerator<GhostRegion> IEnumerable<GhostRegion>.GetEnumerator()
			{
				return new GhostRegionEnumerator(_world, _first, _last);
			}
		}

		public struct GhostRegionEnumerator : IEnumerator<AxisAlignedWrapX2DGhostRegion>, IEnumerator<GhostRegion>
		{
			private AxisAlignedWrapX2DWorld _world;
			private int _first;
			private int _last;
			private int _current;

			public GhostRegionEnumerator(AxisAlignedWrapX2DWorld world, int first, int last)
			{
				_world = world;
				_first = first;
				_last = last;
				_current = _first - 1;
			}

			public AxisAlignedWrapX2DGhostRegion Current
			{
				get
				{
					return _world._ghostRegions[_current];
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return _world._ghostRegions[_current];
				}
			}

			GhostRegion IEnumerator<GhostRegion>.Current
			{
				get
				{
					return _world._ghostRegions[_current];
				}
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				if (_current < _last) ++_current;
				if (_current == _last) return false;
				if (_current == -_world._ghostRegionsXIndexOffset) ++_current;
				return _current < _last;
			}

			public void Reset()
			{
				_current = _first - 1;
			}
		}
	}
}
