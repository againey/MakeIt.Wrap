/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	public class ScalableAxisAlignedBoxBounds : AxisAlignedBoxBounds
	{
		public static ScalableAxisAlignedBoxBounds Create(GameObject gameObject, Bounds box)
		{
			return CreateDerived<ScalableAxisAlignedBoxBounds>(gameObject, box);
		}

		public static Bounds InverseTransform(Transform transform, Bounds box)
		{
			var scale = transform.lossyScale;
			var center = (box.center - transform.position).DivideComponents(scale);
			var size = box.size.DivideComponents(scale);
			return new Bounds(center, size);
		}

		public override Bounds Transform(Transform transform)
		{
			var scale = transform.lossyScale;
			return new Bounds(transform.position + box.center.MultiplyComponents(scale), box.size * scale.MaxAbsComponent());
		}

		public override Bounds Transform(Transform transform, GhostRegion ghostRegion)
		{
			var scale = transform.lossyScale;
			return ghostRegion.Transform(new Bounds(transform.position + box.center.MultiplyComponents(scale), box.size * scale.MaxAbsComponent()));
		}
	}
}
