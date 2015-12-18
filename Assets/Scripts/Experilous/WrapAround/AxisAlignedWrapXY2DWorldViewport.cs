using System;
using System.Collections.Generic;
using UnityEngine;

namespace Experilous.WrapAround
{
	[RequireComponent(typeof(AxisAlignedWrapXY2DWorld))]
	public class AxisAlignedWrapXY2DWorldViewport : AxisAligned2DViewport
	{
		public float bufferThickness;

		protected new void Start()
		{
			RecalculateBounds();
			base.Start();
		}

		protected void LateUpdate()
		{
			RecalculateBounds();
		}

		protected void RecalculateBounds()
		{
			var axisAlignedWorld = (AxisAlignedWrapXY2DWorld)world;

			_min = new Vector3(axisAlignedWorld.minX - bufferThickness, axisAlignedWorld.minY - bufferThickness, float.NegativeInfinity);
			_max = new Vector3(axisAlignedWorld.maxX + bufferThickness, axisAlignedWorld.maxY + bufferThickness, float.PositiveInfinity);
		}
	}
}
