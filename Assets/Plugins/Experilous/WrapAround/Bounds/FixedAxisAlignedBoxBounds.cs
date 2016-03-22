/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	public class FixedAxisAlignedBoxBounds : AxisAlignedBoxBounds
	{
		public static FixedAxisAlignedBoxBounds Create(GameObject gameObject, Bounds box)
		{
			return CreateDerived<FixedAxisAlignedBoxBounds>(gameObject, box);
		}

		public static Bounds InverseTransform(Transform transform, Bounds box)
		{
			return new Bounds(box.center - transform.position, box.size);
		}

		public override Bounds Transform(Transform transform)
		{
			return new Bounds(transform.position + box.center, box.size);
		}

		public override Bounds Transform(Transform transform, GhostRegion ghostRegion)
		{
			return ghostRegion.Transform(new Bounds(transform.position + box.center, box.size));
		}
	}
}
