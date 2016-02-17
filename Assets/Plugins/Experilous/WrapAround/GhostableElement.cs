using UnityEngine;

namespace Experilous.WrapAround
{
	public abstract class GhostableElementBase : MonoBehaviour
	{
		public abstract bool Contains(GhostBase ghost);
		public abstract bool Remove(GhostBase ghost);
	}

	public class GhostableElement<TDerivedElement, TGhost> : GhostableElementBase
		where TDerivedElement : GhostableElement<TDerivedElement, TGhost>
		where TGhost : Ghost<TDerivedElement, TGhost>
	{
		public TGhost ghostPrefab;
		[HideInInspector] public TGhost firstGhost;

		public override bool Contains(GhostBase targetGhost)
		{
			var ghost = firstGhost;
			while (ghost != null)
			{
				if (ReferenceEquals(ghost, targetGhost)) return true;
				ghost = ghost.nextGhost;
			}
			return false;
		}

		protected void Add(TGhost ghost)
		{
			ghost.nextGhost = firstGhost;
			firstGhost = ghost;
		}

		public override bool Remove(GhostBase targetGhost)
		{
			if (firstGhost == null) return false;
			if (ReferenceEquals(firstGhost, targetGhost))
			{
				firstGhost = firstGhost.nextGhost;
				return true;
			}

			var prevGhost = firstGhost;
			var nextGhost = firstGhost.nextGhost;
			while (nextGhost != null)
			{
				if (ReferenceEquals(nextGhost, targetGhost))
				{
					prevGhost.nextGhost = nextGhost.nextGhost;
					return true;
				}
				prevGhost = nextGhost;
				nextGhost = nextGhost.nextGhost;
			}

			return false;
		}

		public TGhost FindGhost(GhostRegion region)
		{
			var ghost = firstGhost;
			while (ghost != null && ghost.region != region)
			{
				ghost = ghost.nextGhost;
			}
			return ghost;
		}
	}
}
