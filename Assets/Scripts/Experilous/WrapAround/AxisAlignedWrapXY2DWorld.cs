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

		public float width { get { return maxX - minX; } }
		public float height { get { return maxY - minY; } }

		public override IEnumerable<GhostRegion> GetGhostRegions(AxisAlignedViewport viewport, object ghostRegions)
		{
			return GetGhostRegions(viewport, ghostRegions as AxisAlignedWrapXY2DGhostRegionContainer);
		}

		private IEnumerable<GhostRegion> GetGhostRegions(AxisAlignedViewport viewport, AxisAlignedWrapXY2DGhostRegionContainer ghostRegions)
		{
			var worldWidth = width;
			var worldHeight = height;

			var viewportMinX = viewport.min.x;
			var viewportMinY = viewport.min.y;
			var viewportMaxX = viewport.max.x;
			var viewportMaxY = viewport.max.y;

			int xIndexMin = Mathf.FloorToInt((viewportMinX - minX) / worldWidth);
			int yIndexMin = Mathf.FloorToInt((viewportMinY - minY) / worldHeight);
			int xIndexMax = Mathf.CeilToInt((viewportMaxX - maxX) / worldWidth);
			int yIndexMax = Mathf.CeilToInt((viewportMaxY - maxY) / worldHeight);

			ghostRegions.Expand(xIndexMin, yIndexMin, xIndexMax, yIndexMax, worldWidth, worldHeight);
			return ghostRegions.Range(xIndexMin, yIndexMin, xIndexMax, yIndexMax);
		}

		public override void Confine(Element element)
		{
			Confine(element.transform);
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

		public override object InstantiateGhostRegions(Viewport viewport)
		{
			return new AxisAlignedWrapXY2DGhostRegionContainer(width, height, viewport);
		}
	}
}
