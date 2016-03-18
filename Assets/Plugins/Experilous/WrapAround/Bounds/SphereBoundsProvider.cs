/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	public class SphereBoundsProvider : ElementBoundsProvider
	{
		public Vector3 center;
		public float radius;

		public override ElementBounds CreateBounds(bool fixedScale, bool fixedRotation)
		{
			return SphereBounds.Create(new Sphere(center, radius), fixedScale, fixedRotation);
		}
	}
}
