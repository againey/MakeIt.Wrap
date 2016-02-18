﻿using UnityEngine;

namespace Experilous.WrapAround
{
	/// <summary>
	/// A base class for the ghost of any element in a world with wrap-around behavior
	/// which mimics the original object in some way, but at a wrapped position.
	/// </summary>
	/// <seealso cref="Ghost`2{TElement,TDerivedGhost}"/>
	public class GhostBase : MonoBehaviour
	{
	}

	/// <summary>
	/// A generically typed base class for the ghost of any element in a world with wrap-around
	/// behavior which mimics the original object in some way, but at a wrapped position.
	/// </summary>
	/// <typeparam name="TElement">The type of the element which this ghost mimics.</typeparam>
	/// <typeparam name="TDerivedGhost">The fully derived type inheriting from this class, a use of the curiously recurring template pattern (CRTP).</typeparam>
	/// <seealso cref="GhostBase"/>
	/// <seealso cref="GhostableElementBase"/>
	/// <seealso cref="GhostableElement`2{TDerivedElement,TGhost}"/>
	public class Ghost<TElement, TDerivedGhost> : GhostBase where TElement : GhostableElementBase where TDerivedGhost : GhostBase
	{
		/// <summary>
		/// The original element that this ghost object mimics.
		/// </summary>
		[HideInInspector] public TElement original;

		/// <summary>
		/// The ghost region with which this ghost is associated.
		/// </summary>
		[HideInInspector] public GhostRegion region;

		/// <summary>
		/// The reference to the next ghost along the singly linked list of ghosts contained by this ghost's element.
		/// </summary>
		[HideInInspector] public TDerivedGhost nextGhost;

		/// <summary>
		/// Removes this ghost object from it's original element's list of ghosts.
		/// </summary>
		public void Remove()
		{
			original.Remove(this);
		}

		/// <summary>
		/// Destroys the <c>GameObject</c> instance to which this ghost <c>Component</c> is attached,
		/// and removes the ghost from it's element's list of ghosts.
		/// </summary>
		public void Destroy()
		{
			Remove();
			Destroy(gameObject);
		}
	}
}