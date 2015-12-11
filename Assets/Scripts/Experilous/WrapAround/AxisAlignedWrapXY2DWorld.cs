using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Experilous.WrapAround
{
	public class AxisAlignedWrapXY2DWorld : World
	{
		public float minX;
		public float maxX;
		public float minY;
		public float maxY;

		public float cameraBufferThickness = 0f;
		public float physicsBufferThickness = 0f;

#pragma warning disable 0649
		[SerializeField]
		private Viewport _cameraViewport = null;
#pragma warning restore 0649

		private GhostRegionRange _cameraGhostRegionRange;
		private int _cameraGhostRegionRangeFrame = -1;

		private AxisAligned2DViewport _physicsViewport;
		private GhostRegionRange _physicsGhostRegionRange;
		private int _physicsGhostRegionRangeFrame = -1;

		private AxisAlignedWrapXY2DGhostRegion[] _ghostRegions = null;
		private int _ghostRegionsWidth = 0;
		private int _ghostRegionsHeight = 0;
		private int _ghostRegionsXIndexOffset = 0;
		private int _ghostRegionsYIndexOffset = 0;

		protected void Start()
		{
			_physicsViewport = gameObject.AddComponent<AxisAligned2DViewport>();
			_physicsViewport.world = this;
			_physicsViewport.min = new Vector3(minX - physicsBufferThickness, minY - physicsBufferThickness, float.NegativeInfinity);
			_physicsViewport.max = new Vector3(maxX + physicsBufferThickness, maxY + physicsBufferThickness, float.PositiveInfinity);

			_ghostRegions = new AxisAlignedWrapXY2DGhostRegion[9];

			var worldWidth = width;
			var worldHeight = height;
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

		public float width { get { return maxX - minX; } }
		public float height { get { return maxY - minY; } }

		public override Viewport cameraViewport { get { return _cameraViewport; } }
		public override Viewport physicsViewport { get { return _physicsViewport; } }

		public override IEnumerable<GhostRegion> GetGhostRegions(AxisAligned2DViewport viewport)
		{
			if (viewport == _cameraViewport) return GetCameraGhostRegions(viewport);
			else if (viewport == _physicsViewport) return GetPhysicsGhostRegions(viewport);
			else return GetGhostRegions(viewport, cameraBufferThickness);
		}

		private GhostRegionRange GetCameraGhostRegions(AxisAligned2DViewport viewport)
		{
			if (Time.frameCount == _cameraGhostRegionRangeFrame) return _cameraGhostRegionRange;
			_cameraGhostRegionRange = GetGhostRegions(viewport, cameraBufferThickness);
			_cameraGhostRegionRangeFrame = Time.frameCount;
			return _cameraGhostRegionRange;
		}

		private GhostRegionRange GetPhysicsGhostRegions(AxisAligned2DViewport viewport)
		{
			if (Time.frameCount == _physicsGhostRegionRangeFrame) return _physicsGhostRegionRange;
			_physicsGhostRegionRange = GetGhostRegions(viewport, physicsBufferThickness);
			_physicsGhostRegionRangeFrame = Time.frameCount;
			return _physicsGhostRegionRange;
		}

		private GhostRegionRange GetGhostRegions(AxisAligned2DViewport viewport, float bufferThickness)
		{
			var worldWidth = width;
			var worldHeight = height;

			var viewportMinX = viewport.min.x - bufferThickness;
			var viewportMaxX = viewport.max.x + bufferThickness;
			var viewportMinY = viewport.min.y - bufferThickness;
			var viewportMaxY = viewport.max.y + bufferThickness;

			int xIndexMin = Mathf.FloorToInt((viewportMinX - minX) / worldWidth);
			int xIndexMax = Mathf.CeilToInt((viewportMaxX - maxX) / worldWidth);
			int yIndexMin = Mathf.FloorToInt((viewportMinY - minY) / worldHeight);
			int yIndexMax = Mathf.CeilToInt((viewportMaxY - maxY) / worldHeight);

			ExpandGhostRegions(xIndexMin, xIndexMax, yIndexMin, yIndexMax);

			int first = (yIndexMin - _ghostRegionsYIndexOffset) * _ghostRegionsWidth + (xIndexMin - _ghostRegionsXIndexOffset);
			int last = (yIndexMax - _ghostRegionsYIndexOffset) * _ghostRegionsWidth + (xIndexMax - _ghostRegionsXIndexOffset) + 1;

			return new GhostRegionRange(this, first, last, xIndexMax - xIndexMin + 1);
		}

		private void ExpandGhostRegions(int xIndex, int yIndex)
		{
			ExpandGhostRegions(xIndex, xIndex, yIndex, yIndex);
		}

		private void ExpandGhostRegions(int xIndexMin, int xIndexMax, int yIndexMin, int yIndexMax)
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

			var worldWidth = width;
			var worldHeight = height;

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

		public override void Confine(Element element)
		{
			var position = element.transform.position;
			var xOffset = position.x - minX;
			var yOffset = position.y - minY;
			var worldWidth = width;
			var worldHeight = height;
			position.x = xOffset - Mathf.Floor(xOffset / worldWidth) * worldWidth + minX;
			position.y = yOffset - Mathf.Floor(yOffset / worldHeight) * worldHeight + minY;
			element.transform.position = position;
		}

		public struct GhostRegionRange : IEnumerable<AxisAlignedWrapXY2DGhostRegion>, IEnumerable<GhostRegion>
		{
			private AxisAlignedWrapXY2DWorld _world;
			private int _first;
			private int _last;
			private int _width;

			public GhostRegionRange(AxisAlignedWrapXY2DWorld world, int first, int last, int width)
			{
				_world = world;
				_first = first;
				_last = last;
				_width = width;
			}

			public IEnumerator<AxisAlignedWrapXY2DGhostRegion> GetEnumerator()
			{
				return new GhostRegionEnumerator(_world, _first, _last, _width);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new GhostRegionEnumerator(_world, _first, _last, _width);
			}

			IEnumerator<GhostRegion> IEnumerable<GhostRegion>.GetEnumerator()
			{
				return new GhostRegionEnumerator(_world, _first, _last, _width);
			}
		}

		public struct GhostRegionEnumerator : IEnumerator<AxisAlignedWrapXY2DGhostRegion>, IEnumerator<GhostRegion>
		{
			private AxisAlignedWrapXY2DWorld _world;
			private int _first;
			private int _last;
			private int _width;
			private int _rowFirst;
			private int _current;

			public GhostRegionEnumerator(AxisAlignedWrapXY2DWorld world, int first, int last, int width)
			{
				_world = world;
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
				if (_current - _rowFirst == _width)
				{
					_current = _rowFirst + _world._ghostRegionsWidth;
					_rowFirst = _current;
				}
				if (_current == -_world._ghostRegionsYIndexOffset * _world._ghostRegionsWidth - _world._ghostRegionsXIndexOffset)
				{
					if (_current < _last) ++_current;
					if (_current == _last) return false;
					if (_current - _rowFirst == _width)
					{
						_current = _rowFirst + _world._ghostRegionsWidth;
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
