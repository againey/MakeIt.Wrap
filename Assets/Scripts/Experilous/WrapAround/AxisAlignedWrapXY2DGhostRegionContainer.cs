using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Experilous.WrapAround
{
	public class AxisAlignedWrapXY2DGhostRegionContainer
	{
		private AxisAlignedWrapXY2DGhostRegion[] _ghostRegions = null;
		private int _ghostRegionsWidth = 0;
		private int _ghostRegionsHeight = 0;
		private int _ghostRegionsXIndexOffset = 0;
		private int _ghostRegionsYIndexOffset = 0;
		private Viewport _viewport;

		public AxisAlignedWrapXY2DGhostRegionContainer(float worldWidth, float worldHeight, Viewport viewport)
		{
			_viewport = viewport;

			_ghostRegions = new AxisAlignedWrapXY2DGhostRegion[9];

			_ghostRegions[0] = new AxisAlignedWrapXY2DGhostRegion(-worldWidth, -worldHeight, _viewport);
			_ghostRegions[1] = new AxisAlignedWrapXY2DGhostRegion(0f, -worldHeight, _viewport);
			_ghostRegions[2] = new AxisAlignedWrapXY2DGhostRegion(+worldWidth, -worldHeight, _viewport);
			_ghostRegions[3] = new AxisAlignedWrapXY2DGhostRegion(-worldWidth, 0f, _viewport);
			_ghostRegions[4] = null;
			_ghostRegions[5] = new AxisAlignedWrapXY2DGhostRegion(+worldWidth, 0f, _viewport);
			_ghostRegions[6] = new AxisAlignedWrapXY2DGhostRegion(-worldWidth, +worldHeight, _viewport);
			_ghostRegions[7] = new AxisAlignedWrapXY2DGhostRegion(0f, +worldHeight, _viewport);
			_ghostRegions[8] = new AxisAlignedWrapXY2DGhostRegion(+worldWidth, +worldHeight, _viewport);

			_ghostRegionsWidth = 3;
			_ghostRegionsHeight = 3;
			_ghostRegionsXIndexOffset = -1;
			_ghostRegionsYIndexOffset = -1;
		}

		public GhostRegionRange Range(int xIndexMin, int yIndexMin, int xIndexMax, int yIndexMax)
		{
			int first = (yIndexMin - _ghostRegionsYIndexOffset) * _ghostRegionsWidth + (xIndexMin - _ghostRegionsXIndexOffset);
			int last = (yIndexMax - _ghostRegionsYIndexOffset) * _ghostRegionsWidth + (xIndexMax - _ghostRegionsXIndexOffset) + 1;

			return new GhostRegionRange(this, first, last, xIndexMax - xIndexMin + 1);
		}

		public void Expand(int xIndex, int yIndex, float worldWidth, float worldHeight)
		{
			Expand(xIndex, yIndex, xIndex, yIndex, worldWidth, worldHeight);
		}

		public void Expand(int xIndexMin, int yIndexMin, int xIndexMax, int yIndexMax, float worldWidth, float worldHeight)
		{
			var leftExpansion = Mathf.Max(_ghostRegionsXIndexOffset - xIndexMin, 0);
			var rightExpansion = Mathf.Max(xIndexMax + 1 - _ghostRegionsWidth - _ghostRegionsXIndexOffset, 0);
			var bottomExpansion = Mathf.Max(_ghostRegionsYIndexOffset - yIndexMin, 0);
			var topExpansion = Mathf.Max(yIndexMax + 1 - _ghostRegionsHeight - _ghostRegionsYIndexOffset, 0);

			if (leftExpansion == 0 && rightExpansion == 0 && bottomExpansion == 0 && topExpansion == 0) return;

			if (leftExpansion == 0 && rightExpansion == 0)
			{
				var newHeight = _ghostRegionsHeight + bottomExpansion + topExpansion;
				var newGhostRegions = new AxisAlignedWrapXY2DGhostRegion[_ghostRegionsWidth * newHeight];

				Array.Copy(_ghostRegions, 0, newGhostRegions, _ghostRegionsWidth * bottomExpansion, _ghostRegions.Length);

				_ghostRegions = newGhostRegions;
				_ghostRegionsHeight = newHeight;
				_ghostRegionsYIndexOffset -= bottomExpansion;

			}
			else
			{
				var newWidth = _ghostRegionsWidth + leftExpansion + rightExpansion;
				var newHeight = _ghostRegionsHeight + bottomExpansion + topExpansion;
				var newGhostRegions = new AxisAlignedWrapXY2DGhostRegion[newWidth * newHeight];

				for (int y = 0; y < _ghostRegionsHeight; ++y)
				{
					Debug.LogFormat("{0}, {1}, {2}, {3}, {4}", bottomExpansion, y, newWidth, leftExpansion, (bottomExpansion + y) * newWidth + leftExpansion);
					Array.Copy(_ghostRegions, y * _ghostRegionsWidth, newGhostRegions, (bottomExpansion + y) * newWidth + leftExpansion, _ghostRegionsWidth);
				}

				_ghostRegions = newGhostRegions;
				_ghostRegionsWidth = newWidth;
				_ghostRegionsHeight = newHeight;
				_ghostRegionsXIndexOffset -= leftExpansion;
				_ghostRegionsYIndexOffset -= bottomExpansion;
			}

			for (int y = 0; y < bottomExpansion; ++y)
			{
				for (int x = 0; x < _ghostRegionsWidth; ++x)
				{
					_ghostRegions[y * _ghostRegionsWidth + x] =
						new AxisAlignedWrapXY2DGhostRegion(
							(_ghostRegionsXIndexOffset + x) * worldWidth,
							(_ghostRegionsYIndexOffset + y) * worldHeight,
							_viewport);
				}
			}

			for (int y = _ghostRegionsHeight - topExpansion; y < _ghostRegionsHeight; ++y)
			{
				for (int x = 0; x < _ghostRegionsWidth; ++x)
				{
					_ghostRegions[y * _ghostRegionsWidth + x] =
						new AxisAlignedWrapXY2DGhostRegion(
							(_ghostRegionsXIndexOffset + x) * worldWidth,
							(_ghostRegionsYIndexOffset + y) * worldHeight,
							_viewport);
				}
			}

			for (int x = 0; x < leftExpansion; ++x)
			{
				for (int y = bottomExpansion; y < _ghostRegionsHeight - topExpansion; ++y)
				{
					_ghostRegions[y * _ghostRegionsWidth + x] =
						new AxisAlignedWrapXY2DGhostRegion(
							(_ghostRegionsXIndexOffset + x) * worldWidth,
							(_ghostRegionsYIndexOffset + y) * worldHeight,
							_viewport);
				}
			}

			for (int x = _ghostRegionsWidth - rightExpansion; x < _ghostRegionsWidth; ++x)
			{
				for (int y = bottomExpansion; y < _ghostRegionsHeight - topExpansion; ++y)
				{
					_ghostRegions[y * _ghostRegionsWidth + x] =
						new AxisAlignedWrapXY2DGhostRegion(
							(_ghostRegionsXIndexOffset + x) * worldWidth,
							(_ghostRegionsYIndexOffset + y) * worldHeight,
							_viewport);
				}
			}
		}

		public struct GhostRegionRange : IEnumerable<AxisAlignedWrapXY2DGhostRegion>, IEnumerable<GhostRegion>
		{
			private AxisAlignedWrapXY2DGhostRegionContainer _container;
			private int _first;
			private int _last;
			private int _width;

			public GhostRegionRange(AxisAlignedWrapXY2DGhostRegionContainer container, int first, int last, int width)
			{
				_container = container;
				_first = first;
				_last = last;
				_width = width;
			}

			public IEnumerator<AxisAlignedWrapXY2DGhostRegion> GetEnumerator()
			{
				return new GhostRegionEnumerator(_container, _first, _last, _width);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new GhostRegionEnumerator(_container, _first, _last, _width);
			}

			IEnumerator<GhostRegion> IEnumerable<GhostRegion>.GetEnumerator()
			{
				return new GhostRegionEnumerator(_container, _first, _last, _width);
			}
		}

		public struct GhostRegionEnumerator : IEnumerator<AxisAlignedWrapXY2DGhostRegion>, IEnumerator<GhostRegion>
		{
			private AxisAlignedWrapXY2DGhostRegionContainer _container;
			private int _first;
			private int _last;
			private int _width;
			private int _rowFirst;
			private int _current;

			public GhostRegionEnumerator(AxisAlignedWrapXY2DGhostRegionContainer container, int first, int last, int width)
			{
				_container = container;
				_first = first;
				_last = last;
				_width = width;
				_rowFirst = first;
				_current = _first - 1;
			}

			public AxisAlignedWrapXY2DGhostRegion Current
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
				if (_current - _rowFirst == _width)
				{
					_current = _rowFirst + _container._ghostRegionsWidth;
					_rowFirst = _current;
				}
				if (_current == -_container._ghostRegionsYIndexOffset * _container._ghostRegionsWidth - _container._ghostRegionsXIndexOffset)
				{
					if (_current < _last) ++_current;
					if (_current == _last) return false;
					if (_current - _rowFirst == _width)
					{
						_current = _rowFirst + _container._ghostRegionsWidth;
						_rowFirst = _current;
					}
				}
				return _current < _last;
			}

			public void Reset()
			{
				_current = _first - 1;
				_rowFirst = _first;
			}
		}
	}
}
