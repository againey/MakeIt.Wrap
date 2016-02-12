using UnityEngine;

namespace Experilous.WrapAround
{
	public class ColliderElementGhost : Ghost<ColliderElement, ColliderElementGhost>
	{
		protected void FixedUpdate()
		{
			if (region == null || !region.isActive || !original.IsCollidable(this))
			{
				Destroy();
			}
		}
	}
}
