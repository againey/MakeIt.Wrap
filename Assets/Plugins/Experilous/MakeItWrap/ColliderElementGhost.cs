/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.MakeIt.Wrap
{
	[AddComponentMenu("Wrap-Around Worlds/Elements/Ghosts/Collider")]
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
