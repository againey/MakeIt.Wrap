using UnityEngine;

namespace Experilous.WrapAround
{
	public class RigidbodyTopLevelElementGhost : MonoBehaviour
	{
		public RigidbodyElement original;
		public GhostRegion region;

		protected void FixedUpdate()
		{
			if (!region.isActive || !original.IsVisible(this))
			{
				region.RemoveElement(original.GetInstanceID());
				Destroy(gameObject);
			}
		}
	}
}
