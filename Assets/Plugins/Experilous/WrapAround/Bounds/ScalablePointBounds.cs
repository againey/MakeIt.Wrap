/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	public class ScalablePointBounds : PointBounds
	{
		public static ScalablePointBounds Create(GameObject gameObject, Vector3 position)
		{
			return CreateDerived<ScalablePointBounds>(gameObject, position);
		}

		public static Vector3 InverseTransform(Transform transform, Vector3 position)
		{
			return (position - transform.position).DivideComponents(transform.lossyScale);
		}

		public override Vector3 Transform(Transform transform)
		{
			return transform.position + position.MultiplyComponents(transform.lossyScale);
		}

		public override Vector3 Transform(Transform transform, GhostRegion ghostRegion)
		{
			return ghostRegion.Transform(transform.position + position.MultiplyComponents(transform.lossyScale));
		}
	}
}
