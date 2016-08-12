/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using Experilous.Numerics;

namespace Experilous.MakeIt.Wrap
{
	[AddComponentMenu("Wrap-Around Worlds/Bounds/Sphere Bounds Provider")]
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
