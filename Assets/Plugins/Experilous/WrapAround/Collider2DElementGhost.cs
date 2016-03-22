/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	[AddComponentMenu("Wrap-Around Worlds/Elements/Ghosts/Collider 2D")]
	public class Collider2DElementGhost : Ghost<Collider2DElement, Collider2DElementGhost>
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
