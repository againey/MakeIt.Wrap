using UnityEngine;

namespace Experilous.WrapAround
{
	public class ElementGhost : MonoBehaviour
	{
		public Element original;
		public GhostRegion region;

		protected void LateUpdate()
		{
			UpdateFromOriginal();

			if (isStale)
			{
				region.DestroyGhost(this);
			}
		}

		protected virtual bool isStale
		{
			get
			{
				return
					!original.IsVisible(original.world.cameraViewport, transform.position, transform.rotation) &&
					(!original.interactsAcrossEdges || !original.IsVisible(original.world.physicsViewport, transform.position, transform.rotation));
			}
		}

		protected virtual void UpdateFromOriginal()
		{
			Vector3 position = original.transform.position;
			Quaternion rotation = original.transform.rotation;
			region.Transform(ref position, ref rotation);
			transform.position = position;
			transform.rotation = rotation;
		}
	}
}
