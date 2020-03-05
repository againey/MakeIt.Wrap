/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace MakeIt.Wrap
{
	[AddComponentMenu("MakeIt.Wrap/Elements/Ghosts/Collider")]
	public class ColliderElementGhost : Ghost<ColliderElement, ColliderElementGhost>
	{
		protected void FixedUpdate()
		{
			if (region == null || !region.isActive || !original.bounds.IsCollidable(original.world, transform))
			{
				Destroy();
			}
		}
	}
}
