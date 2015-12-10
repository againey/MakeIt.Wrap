using UnityEngine;
using System.Collections.Generic;

namespace Experilous.WrapAround
{
	public abstract class World : MonoBehaviour
	{
		public Viewport Viewport;

		public abstract IEnumerable<GhostRegion> GetVisibleGhostRegions(AxisAligned2DViewport world);
		public abstract void Confine(Element element);
	}
}
