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

		public float maxVisibleObjectRadius;
		public float maxPhysicsObjectRadius;

		public float width { get { return maxX - minX; } }
		public Bounds bounds { get { return new Bounds(new Vector3((minX + maxX) * 0.5f, 0f, 0f), new Vector3(width, 0f, 0f)); } }

		private AxisAlignedWrapX2DGhostRegionContainer _physicsGhostRegions;
		private IEnumerable<GhostRegion> _enumerablePhysicsGhostRegions;

		protected void Start()
		{
			_physicsGhostRegions = new AxisAlignedWrapX2DGhostRegionContainer(width);

			var physicsBuffer = maxPhysicsObjectRadius * 2f;
			_enumerablePhysicsGhostRegions = GetGhostRegions(minX - physicsBuffer, maxX + physicsBuffer, _physicsGhostRegions);
		}

		public override IEnumerable<GhostRegion> GetGhostRegions(AxisAlignedViewport viewport, object ghostRegions)
		{
			return GetGhostRegions(viewport, ghostRegions as AxisAlignedWrapX2DGhostRegionContainer);
		}

		private IEnumerable<GhostRegion> GetGhostRegions(AxisAlignedViewport viewport, AxisAlignedWrapX2DGhostRegionContainer ghostRegions)
		{
			return GetGhostRegions(
				viewport.min.x - maxVisibleObjectRadius,
				viewport.max.x + maxVisibleObjectRadius,
				ghostRegions);
		}

		private IEnumerable<GhostRegion> GetGhostRegions(float rangeMinX, float rangeMaxX, AxisAlignedWrapX2DGhostRegionContainer ghostRegions)
		{
			var worldWidth = width;

			int xIndexMin = Mathf.FloorToInt((rangeMinX - minX) / worldWidth);
			int xIndexMax = Mathf.CeilToInt((rangeMaxX - maxX) / worldWidth);

			ghostRegions.Expand(xIndexMin, xIndexMax, worldWidth);
			return ghostRegions.Range(xIndexMin, xIndexMax);
		}

		public override IEnumerable<GhostRegion> physicsGhostRegions { get { return _enumerablePhysicsGhostRegions; } }

		public override bool IsCollidable(Vector3 position)
		{
			return
				position.x >= minX - maxPhysicsObjectRadius &&
				position.x < maxX + maxPhysicsObjectRadius;
		}

		public override bool IsCollidable(Vector3 position, float radius)
		{
			return
				position.x + radius >= minX - maxPhysicsObjectRadius &&
				position.x - radius < maxX + maxPhysicsObjectRadius;
		}

		public override bool IsCollidable(Bounds box)
		{
			return bounds.Intersects(box);
		}

		public override bool IsCollidable(Vector3 position, Bounds box)
		{
			return bounds.Intersects(new Bounds(box.center + position, box.size));
		}

		public override void Confine(Transform transform)
		{
			var position = transform.position;
			Confine(ref position);
			transform.position = position;
		}

		public override void Confine(Rigidbody rigidbody)
		{
			var position = rigidbody.position;
			Confine(ref position);
			rigidbody.position = position;
		}

		public void Confine(ref Vector3 position)
		{
			var xOffset = position.x - minX;
			var worldWidth = width;
			position.x = xOffset - Mathf.Floor(xOffset / worldWidth) * worldWidth + minX;
		}

		public override object InstantiateGhostRegions()
		{
			return new AxisAlignedWrapX2DGhostRegionContainer(width);
		}
	}
}
