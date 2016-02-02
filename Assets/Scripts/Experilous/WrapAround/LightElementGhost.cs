using UnityEngine;

namespace Experilous.WrapAround
{
	public class LightElementGhost : MonoBehaviour
	{
		public LightElement original;
		public GhostRegion region;

		protected void LateUpdate()
		{
			region.Transform(original.transform, transform);

			if (!region.isActive || !original.IsVisible(this))
			{
				region.RemoveElement(original.GetInstanceID());
				Destroy(gameObject);
			}
		}
	}
}
