/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using System;

namespace Experilous.WrapAround
{
	public class AxisAlignedBoxBoundsProvider : ElementBoundsProvider
	{
		public Vector3 center;
		public Vector3 size;

		public override ElementBounds CreateBounds(bool fixedScale, bool fixedRotation)
		{
			if (fixedRotation == false) throw new ArgumentException("Axis aligned box bounds cannot be used with dynamic rotation.");
			return null;//AxisAlignedBoxBounds.Create(new Bounds(center, size), fixedScale);
		}
	}
}
