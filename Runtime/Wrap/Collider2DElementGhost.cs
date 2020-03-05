﻿/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace MakeIt.Wrap
{
	[AddComponentMenu("MakeIt.Wrap/Elements/Ghosts/Collider 2D")]
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