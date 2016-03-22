/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	public class DynamicPointBounds : PointBounds
	{
		public static DynamicPointBounds Create(GameObject gameObject, Vector3 position)
		{
			return CreateDerived<DynamicPointBounds>(gameObject, position);
		}

		public static Vector3 InverseTransform(Transform transform, Vector3 position)
		{
			return transform.InverseTransformVector(position - transform.position);
		}

		public override Vector3 Transform(Transform transform)
		{
			return transform.TransformPoint(position);
		}

		public override Vector3 Transform(Transform transform, GhostRegion ghostRegion)
		{
			return ghostRegion.Transform(transform.TransformPoint(position));
		}
	}
}
