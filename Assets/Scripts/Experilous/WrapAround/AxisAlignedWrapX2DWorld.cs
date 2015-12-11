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

		public float cameraBufferThickness = 0f;
		public float physicsBufferThickness = 0f;

#pragma warning disable 0649
		[SerializeField]
		private Viewport _cameraViewport;
#pragma warning restore 0649

		private GhostRegionRange _cameraGhostRegionRange;
		private int _cameraGhostRegionRangeFrame = -1;

		private AxisAligned2DViewport _physicsViewport;
		private GhostRegionRange _physicsGhostRegionRange;
		private int _physicsGhostRegionRangeFrame = -1;

		private AxisAlignedWrapX2DGhostRegion[] _ghostRegions = null;
		private int _ghostRegionsXIndexOffset = 0;

		protected void Start()
		{
			_physicsViewport = gameObject.AddComponent<AxisAligned2DViewport>();
			_physicsViewport.world = this;
			_physicsViewport.min = new Vector3(minX - physicsBufferThickness, float.NegativeInfinity, float.NegativeInfinity);
			_physicsViewport.max = new Vector3(maxX + physicsBufferThickness, float.PositiveInfinity, float.PositiveInfinity);

			_ghostRegions = new AxisAlignedWrapX2DGhostRegion[3];

			var worldWidth = width;
			_ghostRegions[0] = new AxisAlignedWrapX2DGhostRegion(-worldWidth);
			_ghostRegions[1] = null;
			_ghostRegions[2] = new AxisAlignedWrapX2DGhostRegion(+worldWidth);

			_ghostRegionsXIndexOffset = -1;
		}

		public float width { get { return maxX - minX; } }

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

			var viewportMinX = viewport.min.x - bufferThickness;
			var viewportMaxX = viewport.max.x + bufferThickness;

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
