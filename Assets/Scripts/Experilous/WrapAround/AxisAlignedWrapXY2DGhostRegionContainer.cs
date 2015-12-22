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

		public AxisAlignedWrapXY2DGhostRegionContainer(float worldWidth, float worldHeight)
		{
			_ghostRegions = new AxisAlignedWrapXY2DGhostRegion[9];

			_ghostRegions[0] = new AxisAlignedWrapXY2DGhostRegion(-worldWidth, -worldHeight);
			_ghostRegions[1] = new AxisAlignedWrapXY2DGhostRegion(0f, -worldHeight);
			_ghostRegions[2] = new AxisAlignedWrapXY2DGhostRegion(+worldWidth, -worldHeight);
			_ghostRegions[3] = new AxisAlignedWrapXY2DGhostRegion(-worldWidth, 0f);
			_ghostRegions[4] = null;
			_ghostRegions[5] = new AxisAlignedWrapXY2DGhostRegion(+worldWidth, 0f);
			_ghostRegions[6] = new AxisAlignedWrapXY2DGhostRegion(-worldWidth, +worldHeight);
			_ghostRegions[7] = new AxisAlignedWrapXY2DGhostRegion(0f, +worldHeight);
			_ghostRegions[8] = new AxisAlignedWrapXY2DGhostRegion(+worldWidth, +worldHeight);

			_ghostRegionsWidth = 3;
			_ghostRegionsHeight = 3;
			_ghostRegionsXIndexOffset = -1;
			_ghostRegionsYIndexOffset = -1;
		}

		public GhostRegionRange Range(int xIndexMin, int yIndexMin, int xIndexMax, int yIndexMax)
		{
			return new GhostRegionRange(this, xIndexMin, yIndexMin, xIndexMax, yIndexMax);
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
							(_ghostRegionsYIndexOffset + y) * worldHeight);
				}
			}

			for (int y = _ghostRegionsHeight - topExpansion; y < _ghostRegionsHeight; ++y)
			{
				for (int x = 0; x < _ghostRegionsWidth; ++x)
				{
					_ghostRegions[y * _ghostRegionsWidth + x] =
						new AxisAlignedWrapXY2DGhostRegion(
							(_ghostRegionsXIndexOffset + x) * worldWidth,
							(_ghostRegionsYIndexOffset + y) * worldHeight);
				}
			}

			for (int x = 0; x < leftExpansion; ++x)
			{
				for (int y = bottomExpansion; y < _ghostRegionsHeight - topExpansion; ++y)
				{
					_ghostRegions[y * _ghostRegionsWidth + x] =
						new AxisAlignedWrapXY2DGhostRegion(
							(_ghostRegionsXIndexOffset + x) * worldWidth,
							(_ghostRegionsYIndexOffset + y) * worldHeight);
				}
			}

			for (int x = _ghostRegionsWidth - rightExpansion; x < _ghostRegionsWidth; ++x)
			{
				for (int y = bottomExpansion; y < _ghostRegionsHeight - topExpansion; ++y)
				{
					_ghostRegions[y * _ghostRegionsWidth + x] =
						new AxisAlignedWrapXY2DGhostRegion(
							(_ghostRegionsXIndexOffset + x) * worldWidth,
							(_ghostRegionsYIndexOffset + y) * worldHeight);
				}
			}
		}

		public struct GhostRegionRange : IEnumerable<AxisAlignedWrapXY2DGhostRegion>, IEnumerable<GhostRegion>
		{
			private AxisAlignedWrapXY2DGhostRegionContainer _container;
			private int[] _indices;
			private int _count;

			public GhostRegionRange(AxisAlignedWrapXY2DGhostRegionContainer container)
			{
				_container = container;
				_indices = null;
				_count = 0;
			}

			public GhostRegionRange(AxisAlignedWrapXY2DGhostRegionContainer container, int xIndex, int yIndex)
				: this(container)
			{
				UpdateRange(xIndex, yIndex);
			}

			public GhostRegionRange(AxisAlignedWrapXY2DGhostRegionContainer container, int xIndexMin, int yIndexMin, int xIndexMax, int yIndexMax)
				: this(container)
			{
				UpdateRange(xIndexMin, yIndexMin, xIndexMax, yIndexMax);
			}

			public void UpdateRange(int xIndex, int yIndex)
			{
				if (xIndex != 0 || yIndex != 0)
				{
					if (_indices == null || _indices.Length < 1)
					{
						_indices = new int[1];
					}

					_indices[0] = (yIndex - _container._ghostRegionsYIndexOffset) * _container._ghostRegionsWidth + (xIndex - _container._ghostRegionsXIndexOffset);
					_count = 1;
				}
				else
				{
					_count = 0;
				}
			}

			public void UpdateRange(int xIndexMin, int yIndexMin, int xIndexMax, int yIndexMax)
			{
				var length = (xIndexMax - xIndexMin + 1) * (yIndexMax - yIndexMin + 1);
				if (_indices == null || _indices.Length < length)
				{
					_indices = new int[length];
				}

				int index = 0;
				for (int y = yIndexMin - _container._ghostRegionsYIndexOffset; y <= yIndexMax - _container._ghostRegionsYIndexOffset; ++y)
				{
					for (int x = xIndexMin - _container._ghostRegionsXIndexOffset; x <= xIndexMax - _container._ghostRegionsXIndexOffset; ++x)
					{
						if (x != -_container._ghostRegionsXIndexOffset || y != -_container._ghostRegionsYIndexOffset)
						{
							_indices[index++] = y * _container._ghostRegionsWidth + x;
						}
					}
				}

				_count = index;
			}

			public IEnumerator<AxisAlignedWrapXY2DGhostRegion> GetEnumerator()
			{
				return new GhostRegionEnumerator(_container, _indices, _count);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new GhostRegionEnumerator(_container, _indices, _count);
			}

			IEnumerator<GhostRegion> IEnumerable<GhostRegion>.GetEnumerator()
			{
				return new GhostRegionEnumerator(_container, _indices, _count);
			}
		}

		public struct GhostRegionEnumerator : IEnumerator<AxisAlignedWrapXY2DGhostRegion>, IEnumerator<GhostRegion>
		{
			private AxisAlignedWrapXY2DGhostRegionContainer _container;
			private int[] _indices;
			private int _count;
			private int _current;

			public GhostRegionEnumerator(AxisAlignedWrapXY2DGhostRegionContainer container, int[] indices, int count)
			{
				_container = container;
				_indices = indices;
				_count = count;
				_current = -1;
			}

			public AxisAlignedWrapXY2DGhostRegion Current
			{
				get
				{
					return _container._ghostRegions[_indices[_current]];
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return _container._ghostRegions[_indices[_current]];
				}
			}

			GhostRegion IEnumerator<GhostRegion>.Current
			{
				get
				{
					return _container._ghostRegions[_indices[_current]];
				}
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				return ++_current < _count;
			}

			public void Reset()
			{
				_current = -1;
			}
		}
	}
}
