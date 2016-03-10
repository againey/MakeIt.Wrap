/******************************************************************************\
 *  Copyright (C) 2016 Experilous <againey@experilous.com>
 *  
 *  This file is subject to the terms and conditions defined in the file
 *  'Assets/Plugins/Experilous/License.txt', which is a part of this package.
 *
\******************************************************************************/

using UnityEngine;
using System;

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
	public abstract class GhostableElementBase : BoundedElement
	{
		[NonSerialized] protected ElementBounds _bounds;

		public override ElementBounds bounds { get { return _bounds; } }

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

		protected abstract bool IsGameObjectExcludedFromGhost(Component[] components);
		protected abstract bool IsGameObjectNecessaryForGhost(Component[] components);
		protected abstract void RemoveUnnecessaryComponentsFromGhost(Component[] components);

		public bool AdjustGhostComponents(Transform transform, Transform topLevel)
		{
			var hasChildren = false;
			int childIndex = 0;
			int childCount = transform.childCount;
			while (childIndex < childCount)
			{
				var child = transform.GetChild(childIndex);
				if (child.GetComponent<GhostBase>() == null && AdjustGhostComponents(child, topLevel))
				{
					hasChildren = true;
					++childIndex;
				}
				else
				{
					DestroyImmediate(child.gameObject);
					--childCount;
				}
			}

			var components = transform.GetComponents<Component>();

			bool isExcludedDescendant = IsGameObjectExcludedFromGhost(components) && !ReferenceEquals(transform, topLevel);
			bool isNecessary = IsGameObjectNecessaryForGhost(components) || ReferenceEquals(transform, topLevel);

			if (isExcludedDescendant)
			{
				return false;
			}
			else if (!isNecessary)
			{
				if (!hasChildren)
				{
					return false;
				}
				else
				{
					RemoveMatchingComponentsFromGhost(components, (Component component) => { return !(component is Transform); });
					return true;
				}
			}
			else
			{
				RemoveUnnecessaryComponentsFromGhost(components);
				return true;
			}
		}

		protected void RemoveMatchingComponentsFromGhost(Component[] components, Predicate<Component> predicate)
		{
			bool noneRemoved = false;
			bool allRemoved = false;
			while (!noneRemoved && !allRemoved)
			{
				noneRemoved = true;
				allRemoved = true;
				foreach (var component in components)
				{
					if (component != null)
					{
						if (predicate(component))
						{
							if (CanDestroy(component, components))
							{
								DestroyImmediate(component);
								if (component == null)
								{
									noneRemoved = false;
								}
								else
								{
									allRemoved = false;
								}
							}
							else
							{
								allRemoved = false;
							}
						}
					}
				}
			}
		}

		private bool CanDestroy(Component component, Component[] components)
		{
			foreach (var otherComponent in components)
			{
				if (otherComponent != null)
				{
					var requirements = Utility.GetAttributes<RequireComponent>(otherComponent.GetType());
					foreach (var requirement in requirements)
					{
						if (requirement.m_Type0 != null && requirement.m_Type0.IsInstanceOfType(component)) return false;
						if (requirement.m_Type1 != null && requirement.m_Type1.IsInstanceOfType(component)) return false;
						if (requirement.m_Type2 != null && requirement.m_Type2.IsInstanceOfType(component)) return false;
					}
				}
			}

			return true;
		}
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
	public abstract class GhostableElement<TDerivedElement, TGhost> : GhostableElementBase
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

		protected void Start()
		{
			if (_bounds == null) RefreshBounds();

			if (ghostPrefab == null)
			{
				var ghostTemplate = Instantiate(this).gameObject;
				ghostTemplate.SetActive(false);
				ghostTemplate.transform.SetParent(transform, false);
				ghostTemplate.hideFlags = HideFlags.HideAndDontSave;
				ghostTemplate.name = string.Format("{0} Ghost ({1})", name, typeof(TDerivedElement).GetPrettyName());
				AdjustGhostComponents(ghostTemplate.transform, ghostTemplate.transform);
				ghostPrefab = ghostTemplate.AddComponent<TGhost>();
			}
		}

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
			ghost.gameObject.SetActive(true);
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

		protected void OnDestroy()
		{
			var ghost = firstGhost;
			while (ghost != null)
			{
				var nextGhost = ghost.nextGhost;
				Destroy(ghost.gameObject);
				ghost = nextGhost;
			}
			firstGhost = null;
		}

#if UNITY_EDITOR
		protected virtual Color GetGizmoColor() { return Color.white; }
		protected virtual Color GetGhostGizmoColor() { return Color.gray; }

		protected override void OnDrawGizmosSelected()
		{
			if (bounds == null) RefreshBounds();
			if (bounds != null)
			{
				bounds.DrawGizmosSelected(transform, GetGizmoColor());
				DrawGhostGizmosSelected(GetGhostGizmoColor());
			}
		}

		protected void DrawGhostGizmosSelected(Color color)
		{
			var ghost = firstGhost;
			while (ghost != null)
			{
				bounds.DrawGizmosSelected(ghost.transform, color);
				ghost = ghost.nextGhost;
			}
		}
#endif
	}

	public static class GhostableElementUtility
	{
		public static TElement GetGhostableComponent<TElement, TGhost>(this GameObject gameObject)
			where TElement : GhostableElement<TElement, TGhost>
			where TGhost : Ghost<TElement, TGhost>
		{
			var element = gameObject.GetComponent<TElement>();
			if (element != null) return element;
			var ghost = gameObject.GetComponent<TGhost>();
			if (ghost != null) return ghost.original;
			return null;
		}
	}
}
