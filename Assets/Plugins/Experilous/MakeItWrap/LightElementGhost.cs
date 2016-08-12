/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.MakeIt.Wrap
{
	[AddComponentMenu("Wrap-Around Worlds/Elements/Ghosts/Light")]
	public class LightElementGhost : Ghost<LightElement, LightElementGhost>
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
