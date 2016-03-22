/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	public class FixedPointBounds : PointBounds
	{
		public static FixedPointBounds Create(GameObject gameObject, Vector3 position)
		{
			return CreateDerived<FixedPointBounds>(gameObject, position);
		}

		public static Vector3 InverseTransform(Transform transform, Vector3 position)
		{
			return position - transform.position;
		}

		public override Vector3 Transform(Transform transform)
		{
			return transform.position + position;
		}

		public override Vector3 Transform(Transform transform, GhostRegion ghostRegion)
		{
			return ghostRegion.Transform(transform.position + position);
		}
	}
}
