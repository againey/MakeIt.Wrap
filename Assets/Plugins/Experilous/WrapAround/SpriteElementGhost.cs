/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

namespace Experilous.WrapAround
{
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
