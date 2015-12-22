using UnityEngine;

namespace Experilous.WrapAround
{
	public class ColliderElementGhost : MonoBehaviour
	{
		public ColliderElement original;
		public GhostRegion region;

		protected void FixedUpdate()
		{
			if (!region.isActive || !original.IsCollidable(this))
			{
				region.RemoveElement(original.GetInstanceID());
				Destroy(gameObject);
			}
		}
	}
}
