using UnityEngine;

namespace Experilous.WrapAround
{
	public class ElementGhost : MonoBehaviour
	{
		public Element Original;
		public GhostRegion Region;

		protected void LateUpdate()
		{
			UpdateFromOriginal();

			if (isStale)
			{
				Region.DestroyGhost(this);
			}
		}

		protected virtual bool isStale
		{
			get
			{
				return
					!Original.IsVisible(Original.World.CameraViewport, transform.position, transform.rotation) &&
					(!Original.InteractsAcrossEdges || !Original.IsVisible(Original.World.PhysicsViewport, transform.position, transform.rotation));
			}
		}

		protected virtual void UpdateFromOriginal()
		{
			Vector3 position = Original.transform.position;
			Quaternion rotation = Original.transform.rotation;
			Region.Transform(ref position, ref rotation);
			transform.position = position;
			transform.rotation = rotation;
		}
	}
}
