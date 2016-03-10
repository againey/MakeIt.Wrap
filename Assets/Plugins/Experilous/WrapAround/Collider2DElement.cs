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
	/// A wrap-around world element that is has static colliders which might collide with
	/// objects across wrapped world boundaries.
	/// </summary>
	/// <remarks>
	/// Attach this component to a game object with colliders but no rigidbody whenever you
	/// want it to be able to collide with other colliders across wrapped world boundaries.
	/// This component will create ghosts of itself at the opposite end(s) of the world which
	/// are capable of causing collisions that the original object in its canonical location
	/// would not cause since the standard physics engine cannot handle the wrapped world
	/// boundaries.
	/// 
	/// The ghost prefab should include all collider components, as well as any descendants
	/// with colliders that aren't attached to descendant rigidbodies, but all other components
	/// should probably be absent from the ghost prefab.
	/// </remarks>
	/// <seealso cref="World"/>
	/// <seealso cref="ElementBounds"/>
	/// <seealso cref="GhostRegion"/>
	/// <seealso cref="WorldProvider"/>
	/// <seealso cref="IWorldConsumer"/>
	/// <seealso cref="GhostableElement`2{TDerivedElement,TGhost}"/>
	/// <seealso cref="Collider2DElementGhost"/>
	/// <seealso cref="Collider2D"/>
	/// <seealso cref="ColliderElement"/>
	[RequireComponent(typeof(Collider2D))]
	public class Collider2DElement : GhostableElement<Collider2DElement, Collider2DElementGhost>, IWorldConsumer
	{
		public World world;
		public ElementBoundsSource boundsSource = ElementBoundsSource.FixedScale | ElementBoundsSource.Automatic;
		public ElementBoundsProvider boundsProvider;

		public bool hasWorld { get { return world != null ; } }
		public World GetWorld() { return world; }
		public void SetWorld(World world) { this.world = world; }

		protected new void Start()
		{
			base.Start();

			if (world == null) world = WorldConsumerUtility.FindWorld(this);
			this.DisableAndThrowOnUnassignedReference(world, "The Collider2DElement component requires a reference to a World component.");
		}

		protected void FixedUpdate()
		{
			foreach (var ghostRegion in world.physicsGhostRegions)
			{
				if (FindGhost(ghostRegion) == null && _bounds.IsCollidable(world, transform, ghostRegion))
				{
					InstantiateGhost(ghostRegion);
				}
			}
		}

		public override void RefreshBounds()
		{
			_bounds = ElementBounds.CreateBounds(boundsSource, boundsProvider, transform,
				() => { return HierarchyUtility.GetCollider2DGroupAxisAlignedBoxBounds(transform); },
				() => { return HierarchyUtility.GetCollider2DGroupSphereBounds(transform); });

#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) UnityEditor.SceneView.RepaintAll();
#endif
		}

		protected void InstantiateGhost(GhostRegion ghostRegion)
		{
			var ghost = Instantiate(ghostPrefab);
			ghost.transform.SetParent(transform.parent, false);
			ghost.name = name + " (Collider2D Ghost)";
			ghost.region = ghostRegion;
			ghost.original = this;

			ghost.transform.localScale = transform.localScale;
			ghostRegion.Transform(transform, ghost.transform);

			Add(ghost);
		}

		protected override bool IsGameObjectExcludedFromGhost(Component[] components)
		{
			foreach (var component in components)
			{
				if (component is Rigidbody2D)
				{
					return true;
				}
			}
			return false;
		}

		protected override bool IsGameObjectNecessaryForGhost(Component[] components)
		{
			foreach (var component in components)
			{
				if (component is Collider2D)
				{
					return true;
				}
			}
			return false;
		}

		protected override void RemoveUnnecessaryComponentsFromGhost(Component[] components)
		{
			RemoveMatchingComponentsFromGhost(components, (Component component) => { return !(component is Collider2D || component is Transform); });
		}
	}
}
