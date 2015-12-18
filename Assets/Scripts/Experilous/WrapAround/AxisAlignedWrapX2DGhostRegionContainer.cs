using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Experilous.WrapAround
{
	public class AxisAlignedWrapX2DGhostRegionContainer
	{
		private AxisAlignedWrapX2DGhostRegion[] _ghostRegions = null;
		private int _ghostRegionsWidth = 0;
		private int _ghostRegionsXIndexOffset = 0;
		private Viewport _viewport;

		public AxisAlignedWrapX2DGhostRegionContainer(float worldWidth, Viewport viewport)
		{
			_viewport = viewport;

			_ghostRegions = new AxisAlignedWrapX2DGhostRegion[3];

			_ghostRegions[0] = new AxisAlignedWrapX2DGhostRegion(-worldWidth, _viewport);
			_ghostRegions[1] = null;
			_ghostRegions[2] = new AxisAlignedWrapX2DGhostRegion(+worldWidth, _viewport);

			_ghostRegionsWidth = 3;
			_ghostRegionsXIndexOffset = -1;
		}

		public GhostRegionRange Range(int xIndexMin, int xIndexMax)
		{
			int first = xIndexMin - _ghostRegionsXIndexOffset;
			int last = xIndexMax - _ghostRegionsXIndexOffset + 1;

			return new GhostRegionRange(this, first, last);
		}

		public void Expand(int xIndex, float worldWidth)
		{
			Expand(xIndex, xIndex, worldWidth);
		}

		public void Expand(int xIndexMin, int xIndexMax, float worldWidth)
		{
			var leftExpansion = Mathf.Max(_ghostRegionsXIndexOffset - xIndexMin, 0);
			var rightExpansion = Mathf.Max(xIndexMax + 1 - _ghostRegionsWidth - _ghostRegionsXIndexOffset, 0);

			if (leftExpansion == 0 && rightExpansion == 0) return;

			var newWidth = _ghostRegionsWidth + leftExpansion + rightExpansion;
			var newGhostRegions = new AxisAlignedWrapX2DGhostRegion[newWidth];

			Array.Copy(_ghostRegions, _ghostRegionsWidth, newGhostRegions, leftExpansion, _ghostRegionsWidth);

			_ghostRegions = newGhostRegions;
			_ghostRegionsWidth = newWidth;
			_ghostRegionsXIndexOffset -= leftExpansion;

			for (int x = 0; x < _ghostRegionsWidth; ++x)
			{
				_ghostRegions[x] = new AxisAlignedWrapX2DGhostRegion((_ghostRegionsXIndexOffset + x) * worldWidth, _viewport);
			}

			for (int x = _ghostRegionsWidth - rightExpansion; x < _ghostRegionsWidth; ++x)
			{
				_ghostRegions[x] = new AxisAlignedWrapX2DGhostRegion((_ghostRegionsXIndexOffset + x) * worldWidth, _viewport);
			}
		}

		public struct GhostRegionRange : IEnumerable<AxisAlignedWrapX2DGhostRegion>, IEnumerable<GhostRegion>
		{
			private AxisAlignedWrapX2DGhostRegionContainer _container;
			private int _first;
			private int _last;

			public GhostRegionRange(AxisAlignedWrapX2DGhostRegionContainer container, int first, int last)
			{
				_container = container;
				_first = first;
				_last = last;
			}

			public IEnumerator<AxisAlignedWrapX2DGhostRegion> GetEnumerator()
			{
				return new GhostRegionEnumerator(_container, _first, _last);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new GhostRegionEnumerator(_container, _first, _last);
			}

			IEnumerator<GhostRegion> IEnumerable<GhostRegion>.GetEnumerator()
			{
				return new GhostRegionEnumerator(_container, _first, _last);
			}
		}

		public struct GhostRegionEnumerator : IEnumerator<AxisAlignedWrapX2DGhostRegion>, IEnumerator<GhostRegion>
		{
			private AxisAlignedWrapX2DGhostRegionContainer _container;
			private int _first;
			private int _last;
			private int _current;

			public GhostRegionEnumerator(AxisAlignedWrapX2DGhostRegionContainer container, int first, int last)
			{
				_container = container;
				_first = first;
				_last = last;
				_current = _first - 1;
			}

			public AxisAlignedWrapX2DGhostRegion Current
			{
				get
				{
					return _container._ghostRegions[_current];
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return _container._ghostRegions[_current];
				}
			}

			GhostRegion IEnumerator<GhostRegion>.Current
			{
				get
				{
					return _container._ghostRegions[_current];
				}
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				if (_current < _last) ++_current;
				if (_current == _last) return false;
				if (_current == -_container._ghostRegionsXIndexOffset) ++_current;
				return _current < _last;
			}

			public void Reset()
			{
				_current = _first - 1;
			}
		}
	}
}
