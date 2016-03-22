/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	public class RotatablePointBounds : PointBounds
	{
		public static RotatablePointBounds Create(GameObject gameObject, Vector3 position)
		{
			return CreateDerived<RotatablePointBounds>(gameObject, position);
		}

		public static Vector3 InverseTransform(Transform transform, Vector3 position)
		{
			return transform.InverseTransformDirection(position - transform.position);
		}

		public override Vector3 Transform(Transform transform)
		{
			return transform.position + transform.TransformDirection(position);
		}

		public override Vector3 Transform(Transform transform, GhostRegion ghostRegion)
		{
			return ghostRegion.Transform(transform.position + transform.TransformDirection(position));
		}
	}
}
