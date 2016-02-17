using UnityEngine;

namespace Experilous.WrapAround
{
	public class GhostBase : MonoBehaviour
	{
	}

	public class Ghost<TElement, TDerivedGhost> : GhostBase where TElement : GhostableElementBase where TDerivedGhost : GhostBase
	{
		[HideInInspector] public TElement original;
		[HideInInspector] public GhostRegion region;
		[HideInInspector] public TDerivedGhost nextGhost;

		public void Remove()
		{
			original.Remove(this);
		}

		public void Destroy()
		{
			Remove();
			Destroy(gameObject);
		}
	}
}
