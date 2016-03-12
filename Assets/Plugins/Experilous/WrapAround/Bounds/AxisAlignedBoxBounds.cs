﻿/******************************************************************************\
 *  Copyright (C) 2016 Experilous <againey@experilous.com>
 *  
 *  This file is subject to the terms and conditions defined in the file
 *  'Assets/Plugins/Experilous/License.txt', which is a part of this package.
 *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	public abstract class AxisAlignedBoxBounds : ElementBounds
	{
		public Bounds box;

		public AxisAlignedBoxBounds(Bounds box)
		{
			this.box = box;
		}

		public static AxisAlignedBoxBounds Create(Bounds box, bool fixedScale)
		{
			if (fixedScale)
			{
				return new FixedAxisAlignedBoxBounds(box);
			}
			else
			{
				return new ScalableAxisAlignedBoxBounds(box);
			}
		}

		public static AxisAlignedBoxBounds Create(Bounds box, Transform transform, bool fixedScale)
		{
			if (fixedScale)
			{
				return new FixedAxisAlignedBoxBounds(new Bounds(box.center - transform.position, box.size));
			}
			else
			{
				var scale = transform.lossyScale;
				var center = (box.center - transform.position).DivideComponents(scale);
				var size = box.size.DivideComponents(scale);
				return new ScalableAxisAlignedBoxBounds(new Bounds(center, size));
			}
		}
	}
}