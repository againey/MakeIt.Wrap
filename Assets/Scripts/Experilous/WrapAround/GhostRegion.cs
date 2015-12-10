using UnityEngine;

namespace Experilous.WrapAround
{
	public abstract class GhostRegion
	{
		public abstract bool HasGhost(Element element);
		public abstract void AddElement(Element element);
		public abstract void Transform(ref Vector3 position, ref Quaternion rotation);
		public abstract void DestroyGhost(ElementGhost ghost);
	}
}
