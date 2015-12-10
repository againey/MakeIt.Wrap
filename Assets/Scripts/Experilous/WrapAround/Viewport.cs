using UnityEngine;
using System.Collections.Generic;

namespace Experilous.WrapAround
{
	public abstract class Viewport : MonoBehaviour
	{
		public World World;

		public abstract bool IsVisible(Vector3 position);
		public abstract bool IsVisible(PointElement element);

		public abstract IEnumerable<GhostRegion> visibleGhostRegions { get; }
	}
}
