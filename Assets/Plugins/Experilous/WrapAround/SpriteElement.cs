/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	/// <summary>
	/// A wrap-around world element with a sprite that ought to be rendered in visible ghost regions.
	/// </summary>
	/// <remarks>
	/// Attach this component to a game object with one or more sprite renderers whenever you want
	/// those sprites to be visible across wrapped world boundaries.  Every frame, the element will
	/// manually add the meshes to the render queue with the appropriate transformations for each
	/// visible ghost region in which the ghost might also be visible.
	/// </remarks>
	/// <remarks>
	/// <para>Attach this component to a game object with one or more sprite renderers whenever you
	/// want those sprites to be visible across wrapped world boundaries.  This component will create
	/// ghosts of itself at the opposite end(s) of the world that the original object in its canonical
	/// location would not render since the standard rendering engine cannot handle the wrapped world
	/// boundaries.</para>
	/// 
	/// <para>The ghost prefab should include only the sprite renderer component, as well as any
	/// descendants with sprite renderers that should also be applied, but all other components
	/// should probably be absent from the ghost prefab.</para>
	/// </remarks>
	/// <seealso cref="Viewport"/>
	/// <seealso cref="ElementBounds"/>
	/// <seealso cref="GhostRegion"/>
	/// <seealso cref="ViewportProvider"/>
	/// <seealso cref="IViewportConsumer"/>
	/// <seealso cref="GhostableElement`2{TDerivedElement,TGhost}"/>
	/// <seealso cref="SpriteElementGhost"/>
	/// <seealso cref="SpriteRenderer"/>
	public class SpriteElement : GhostableElement<SpriteElement, SpriteElementGhost>, IViewportConsumer
	{
		public Viewport viewport;
		public ElementBoundsSource boundsSource = ElementBoundsSource.FixedScale | ElementBoundsSource.Automatic;
		public ElementBoundsProvider boundsProvider;

		public bool hasViewport { get { return viewport != null ; } }
		public Viewport GetViewport() { return viewport; }
		public void SetViewport(Viewport viewport) { this.viewport = viewport; }

		protected new void Start()
		{
			base.Start();

			if (viewport == null) viewport = ViewportConsumerUtility.FindViewport(this);
			this.DisableAndThrowOnUnassignedReference(viewport, "The SpriteElement component requires a reference to a Viewport component.");
		}

		protected void LateUpdate()
		{
			foreach (var ghostRegion in viewport.visibleGhostRegions)
			{
				if (FindGhost(ghostRegion) == null && bounds.IsVisible(viewport, transform, ghostRegion))
				{
					InstantiateGhost(ghostRegion);
				}
			}
		}

		public override Sphere ComputeSphereBounds()
		{
			return HierarchyUtility.GetSpriteGroupSphereBounds(transform);
		}

		public override Bounds ComputeAxisAlignedBoxBounds()
		{
			return HierarchyUtility.GetSpriteGroupAxisAlignedBoxBounds(transform);
		}

		protected void InstantiateGhost(GhostRegion ghostRegion)
		{
			var ghost = Instantiate(ghostPrefab);
			ghost.transform.SetParent(transform.parent, false);
			ghost.name = name + " (Sprite Ghost)";
			ghost.region = ghostRegion;
			ghost.original = this;

			ghostRegion.Transform(transform, ghost.transform);

			Add(ghost);
		}

		protected override bool IsGameObjectExcludedFromGhost(Component[] components)
		{
			return false;
		}

		protected override bool IsGameObjectNecessaryForGhost(Component[] components)
		{
			foreach (var component in components)
			{
				if (component is SpriteRenderer)
				{
					return true;
				}
			}
			return false;
		}

		protected override void RemoveUnnecessaryComponentsFromGhost(Component[] components)
		{
			RemoveMatchingComponentsFromGhost(components, (Component component) => { return !(component is SpriteRenderer || component is Transform); });
		}
	}
}
