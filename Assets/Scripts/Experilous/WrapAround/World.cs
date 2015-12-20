using UnityEngine;
using System.Collections.Generic;

namespace Experilous.WrapAround
{
	public abstract class World : MonoBehaviour
	{
		public abstract IEnumerable<GhostRegion> GetGhostRegions(AxisAlignedViewport viewport, object ghostRegions);

		public abstract void Confine(Transform transform);
		public abstract void Confine(Rigidbody rigidbody);
		public abstract object InstantiateGhostRegions(Viewport viewport);
	}
}
