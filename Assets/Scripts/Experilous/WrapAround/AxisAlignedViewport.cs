using UnityEngine;

namespace Experilous.WrapAround
{
	public abstract class AxisAlignedViewport : Viewport
	{
		public abstract Vector3 min { get; }
		public abstract Vector3 max { get; }

		public abstract Vector3 bufferedMin { get; }
		public abstract Vector3 bufferedMax { get; }
	}
}
