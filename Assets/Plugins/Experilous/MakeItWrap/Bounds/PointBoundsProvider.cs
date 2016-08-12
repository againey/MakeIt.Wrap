/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.MakeIt.Wrap
{
	[AddComponentMenu("Wrap-Around Worlds/Bounds/Point Bounds Provider")]
	public class PointBoundsProvider : ElementBoundsProvider
	{
		public Vector3 position;

		public override ElementBounds CreateBounds(bool fixedScale, bool fixedRotation)
		{
			return PointBounds.Create(position, fixedScale, fixedRotation);
		}
	}
}
