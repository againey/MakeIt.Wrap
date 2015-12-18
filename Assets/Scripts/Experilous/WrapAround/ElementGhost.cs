using UnityEngine;

namespace Experilous.WrapAround
{
	public abstract class ElementGhost : MonoBehaviour
	{
		public Element original;
		public GhostRegion region;

		public abstract void UpdateFromOriginal();
	}
}
