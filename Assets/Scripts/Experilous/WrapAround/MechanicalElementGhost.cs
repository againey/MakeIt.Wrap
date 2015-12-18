using UnityEngine;

namespace Experilous.WrapAround
{
	public class MechanicalElementGhost : ElementGhost
	{
		protected void FixedUpdate()
		{
			UpdateFromOriginal();

			if (!original.IsVisible(region.viewport, transform.position, transform.rotation))
			{
				region.RemoveElement(original);
				Destroy(gameObject);
			}
		}

		public override void UpdateFromOriginal()
		{
			Vector3 position = original.transform.position;
			Quaternion rotation = original.transform.rotation;
			region.Transform(ref position, ref rotation);
			transform.position = position;
			transform.rotation = rotation;
		}
	}
}
