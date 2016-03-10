/******************************************************************************\
 *  Copyright (C) 2016 Experilous <againey@experilous.com>
 *  
 *  This file is subject to the terms and conditions defined in the file
 *  'Assets/Plugins/Experilous/License.txt', which is a part of this package.
 *
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
			return AxisAlignedBoxBounds.Create(new Bounds(center, size), fixedScale);
		}
	}
}
