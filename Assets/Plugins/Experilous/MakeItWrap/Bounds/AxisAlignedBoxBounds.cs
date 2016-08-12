/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using Experilous.MakeIt.Utilities;

namespace Experilous.MakeIt.Wrap
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
