/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace MakeIt.Wrap
{
	[AddComponentMenu("MakeIt.Wrap/Bounds/Point Bounds Provider")]
	public class PointBoundsProvider : ElementBoundsProvider
	{
		public Vector3 position;

		public override ElementBounds CreateBounds(bool fixedScale, bool fixedRotation)
		{
			return PointBounds.Create(position, fixedScale, fixedRotation);
		}
	}
}
