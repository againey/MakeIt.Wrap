using UnityEngine;

namespace Experilous.WrapAround
{
	/// <summary>
	/// A base class for any element in a world with wrap-around behavior which in some circumstances needs
	/// to instantiate a game object which mimics the original object in some way, but at a wrapped position.
	/// </summary>
	/// <remarks>
	/// <c>GhostElement&lt;TDerivedElement, TGhost&gt;</c> is the real base class with the meat of
	/// functionality, but this simple base class is required due to the way C# handles generics.
	/// </remarks>
	/// <seealso cref="GhostableElement`2{TDerivedElement,TGhost};"/>
	/// <seealso cref="GhostBase"/>
	public abstract class GhostableElementBase : MonoBehaviour
	{
		/// <summary>
		/// Checks whether or not the element's list of ghosts contains the specified ghost object.
		/// </summary>
		/// <param name="ghost">The ghost which is being searched for in the element's list of ghosts.</param>
		/// <returns>Returns <c>true</c> if the ghost is found in the element's list of ghosts; <c>false</c> otherwise.</returns>
		public abstract bool Contains(GhostBase ghost);

		/// <summary>
		/// Removes the specified ghost object from the element's list of ghosts.
		/// </summary>
		/// <param name="ghost">The ghost which is to be removed from the element's list of ghosts.</param>
		/// <returns>Returns <c>true</c> if the ghost was found and removed from the element's list of ghosts; <c>false</c> if the ghost was not found in the list.</returns>
		public abstract bool Remove(GhostBase ghost);
	}

	/// <summary>
	/// A generically typed base class for any element in a world with wrap-around behavior which in some
	/// circumstances needs to instantiate a game object which mimics the original object in some way, but
	/// at a wrapped position.
	/// </summary>
	/// <typeparam name="TDerivedElement">The fully derived type inheriting from this class, a use of the curiously recurring template pattern (CRTP).</typeparam>
	/// <typeparam name="TGhost">The type of ghost that the derived element type will create and store within its list of ghosts.</typeparam>
	/// <seealso cref="GhostableElementBase"/>
	/// <seealso cref="GhostBase"/>
	/// <seealso cref="Ghost`2{TElement,TDerivedGhost};"/>
	/// <seealso cref="GhostRegion"/>
	public class GhostableElement<TDerivedElement, TGhost> : GhostableElementBase
		where TDerivedElement : GhostableElement<TDerivedElement, TGhost>
		where TGhost : Ghost<TDerivedElement, TGhost>
	{
		/// <summary>
		/// The prefab used by the derived element type as a template for instantiating ghosts.  It will
		/// typically have a subset of components and/or child objects possessed by the original element,
		/// only those components and children necessary for the wrapped ghost behaviors.
		/// </summary>
		public TGhost ghostPrefab;

		/// <summary>
		/// The head reference of a singly linked list of ghosts contained by this element.
		/// </summary>
		[HideInInspector] public TGhost firstGhost;

		/// <summary>
		/// Checks whether or not the element's list of ghosts contains the specified ghost object.
		/// </summary>
		/// <param name="ghost">The ghost which is being searched for in the element's list of ghosts.</param>
		/// <returns>Returns <c>true</c> if the ghost is found in the element's list of ghosts; <c>false</c> otherwise.</returns>
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

		/// <summary>
		/// Adds the specified ghost object to the element's list of ghosts.
		/// </summary>
		/// <param name="ghost">The ghost which is to be added to the element's list of ghosts.</param>
		protected void Add(TGhost ghost)
		{
			ghost.nextGhost = firstGhost;
			firstGhost = ghost;
		}

		/// <summary>
		/// Removes the specified ghost object from the element's list of ghosts.
		/// </summary>
		/// <param name="ghost">The ghost which is to be removed from the element's list of ghosts.</param>
		/// <returns>Returns <c>true</c> if the ghost was found and removed from the element's list of ghosts; <c>false</c> if the ghost was not found in the list.</returns>
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

		/// <summary>
		/// Searches in the element's list of ghosts for the ghost object associated with the specified ghost region.
		/// </summary>
		/// <param name="region">The ghost region which is potentially associated with a ghost object of this element.</param>
		/// <returns>Returns the ghost object associated with the specified ghost region, if any exists in this element's list of ghosts; <c>null</c> otherwise.</returns>
		/// <seealso cref="GhostRegion"/>
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
