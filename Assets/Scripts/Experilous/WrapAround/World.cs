using UnityEngine;
using System.Collections.Generic;

namespace Experilous.WrapAround
{
	public abstract class World : MonoBehaviour
	{
		public abstract Viewport cameraViewport { get; }
		public abstract Viewport physicsViewport { get; }

		public abstract IEnumerable<GhostRegion> GetGhostRegions(AxisAligned2DViewport viewport);

		public abstract void Confine(Element element);
	}
}
