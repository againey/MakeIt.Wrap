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

		public float width { get { return maxX - minX; } }

		public override IEnumerable<GhostRegion> GetGhostRegions(AxisAlignedViewport viewport, object ghostRegions)
		{
			return GetGhostRegions(viewport, ghostRegions as AxisAlignedWrapX2DGhostRegionContainer);
		}

		private IEnumerable<GhostRegion> GetGhostRegions(AxisAlignedViewport viewport, AxisAlignedWrapX2DGhostRegionContainer ghostRegions)
		{
			var worldWidth = width;

			var viewportMinX = viewport.bufferedMin.x;
			var viewportMaxX = viewport.bufferedMax.x;

			int xIndexMin = Mathf.FloorToInt((viewportMinX - minX) / worldWidth);
			int xIndexMax = Mathf.CeilToInt((viewportMaxX - maxX) / worldWidth);

			ghostRegions.Expand(xIndexMin, xIndexMax, worldWidth);
			return ghostRegions.Range(xIndexMin, xIndexMax);
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

		public override object InstantiateGhostRegions(Viewport viewport)
		{
			return new AxisAlignedWrapX2DGhostRegionContainer(width, viewport);
		}
	}
}
