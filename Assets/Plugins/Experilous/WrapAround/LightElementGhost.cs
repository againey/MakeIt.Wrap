using UnityEngine;

namespace Experilous.WrapAround
{
	public class LightElementGhost : Ghost<LightElement, LightElementGhost>
	{
		protected void LateUpdate()
		{
			region.Transform(original.transform, transform);

			if (region == null || !region.isActive || !original.IsVisible(this))
			{
				Destroy();
			}
		}
	}
}
