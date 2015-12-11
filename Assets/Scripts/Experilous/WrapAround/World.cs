using UnityEngine;
using System.Collections.Generic;

namespace Experilous.WrapAround
{
	public abstract class World : MonoBehaviour
	{
		public Viewport CameraViewport;
		public Viewport PhysicsViewport;

		public abstract IEnumerable<GhostRegion> GetVisibleGhostRegions(AxisAligned2DViewport viewport);

		public abstract void Confine(Element element);
	}
}
