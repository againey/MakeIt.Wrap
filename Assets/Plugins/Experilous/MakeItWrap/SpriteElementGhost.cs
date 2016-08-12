/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.MakeIt.Wrap
{
	[AddComponentMenu("Wrap-Around Worlds/Elements/Ghosts/Sprite")]
	public class SpriteElementGhost : Ghost<SpriteElement, SpriteElementGhost>
	{
		protected void LateUpdate()
		{
			region.Transform(original.transform, transform);

			if (region == null || !region.isActive || !original.bounds.IsVisible(original.viewport, transform))
			{
				Destroy();
			}
		}
	}
}
