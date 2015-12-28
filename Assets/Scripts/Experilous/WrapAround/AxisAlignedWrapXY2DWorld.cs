using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Experilous.WrapAround
{
	public class AxisAlignedWrapXY2DWorld : World
	{
		public float minX;
		public float minY;
		public float maxX;
		public float maxY;

		public float maxVisibleObjectRadius;
		public float maxPhysicsObjectRadius;

		public float width { get { return maxX - minX; } }
		public float height { get { return maxY - minY; } }
		public Bounds bounds { get { return new Bounds(new Vector3((minX + maxX) * 0.5f, (minY + maxY) * 0.5f, 0f), new Vector3(width, height, 0f)); } }

		private AxisAlignedWrapXY2DGhostRegionContainer _physicsGhostRegions;
		private IEnumerable<GhostRegion> _enumerablePhysicsGhostRegions;

		protected void Start()
		{
			_physicsGhostRegions = new AxisAlignedWrapXY2DGhostRegionContainer(width, height);

			var physicsBuffer = maxPhysicsObjectRadius * 2f;
			_enumerablePhysicsGhostRegions = GetGhostRegions(minX - physicsBuffer, minY - physicsBuffer, maxX + physicsBuffer, maxY + physicsBuffer, _physicsGhostRegions);
		}

		public override IEnumerable<GhostRegion> GetGhostRegions(AxisAlignedViewport viewport, object ghostRegions)
		{
			return GetGhostRegions(viewport, ghostRegions as AxisAlignedWrapXY2DGhostRegionContainer);
		}

		private IEnumerable<GhostRegion> GetGhostRegions(AxisAlignedViewport viewport, AxisAlignedWrapXY2DGhostRegionContainer ghostRegions)
		{
			return GetGhostRegions(
				viewport.min.x - maxVisibleObjectRadius,
				viewport.min.y - maxVisibleObjectRadius,
				viewport.max.x + maxVisibleObjectRadius,
				viewport.max.y + maxVisibleObjectRadius,
				ghostRegions);
		}

		private IEnumerable<GhostRegion> GetGhostRegions(float rangeMinX, float rangeMinY, float rangeMaxX, float rangeMaxY, AxisAlignedWrapXY2DGhostRegionContainer ghostRegions)
		{
			var worldWidth = width;
			var worldHeight = height;

			int xIndexMin = Mathf.FloorToInt((rangeMinX - minX) / worldWidth);
			int yIndexMin = Mathf.FloorToInt((rangeMinY - minY) / worldHeight);
			int xIndexMax = Mathf.CeilToInt((rangeMaxX - maxX) / worldWidth);
			int yIndexMax = Mathf.CeilToInt((rangeMaxY - maxY) / worldHeight);

			ghostRegions.Expand(xIndexMin, yIndexMin, xIndexMax, yIndexMax, worldWidth, worldHeight);
			return ghostRegions.Range(xIndexMin, yIndexMin, xIndexMax, yIndexMax);
		}

		public override IEnumerable<GhostRegion> physicsGhostRegions { get { return _enumerablePhysicsGhostRegions; } }

		public override bool IsCollidable(Vector3 position)
		{
			return
				position.x >= minX - maxPhysicsObjectRadius &&
				position.y >= minY - maxPhysicsObjectRadius &&
				position.x < maxX + maxPhysicsObjectRadius &&
				position.y < maxY + maxPhysicsObjectRadius;
		}

		public override bool IsCollidable(Vector3 position, float radius)
		{
			return
				position.x + radius >= minX - maxPhysicsObjectRadius &&
				position.y + radius >= minY - maxPhysicsObjectRadius &&
				position.x - radius < maxX + maxPhysicsObjectRadius &&
				position.y - radius < maxY + maxPhysicsObjectRadius;
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
			var yOffset = position.y - minY;
			var worldWidth = width;
			var worldHeight = height;
			position.x = xOffset - Mathf.Floor(xOffset / worldWidth) * worldWidth + minX;
			position.y = yOffset - Mathf.Floor(yOffset / worldHeight) * worldHeight + minY;
		}

		public override object InstantiateGhostRegions()
		{
			return new AxisAlignedWrapXY2DGhostRegionContainer(width, height);
		}
	}
}
