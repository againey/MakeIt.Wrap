/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using Experilous.Core;

namespace Experilous.MakeItWrap
{
	/// <summary>
	/// A wrap-around world element that is physically active and could potentially collide
	/// with other objects across wrapped world boundaries.
	/// </summary>
	/// <remarks>
	/// Attach this component to a game object with a rigidbody component whenever you want
	/// any associated colliders to be able to collide with other rigidbodies across wrapped
	/// world boundaries.  This component will create ghosts of itself at the opposite end(s)
	/// of the world which are capable of causing collisions that the original object in its
	/// canonical location would not cause since the standard physics engine cannot handle the
	/// wrapped world boundaries.
	/// 
	/// The ghost prefab should include a rigidbody component and all collider components, as
	/// well as any descendants with colliders that aren't attached to descendant rigidbodies,
	/// but all other components should probably be absent from the ghost prefab.
	/// 
	/// This component cannot currently handle rigidbodies with attached joints of any kind.
	/// </remarks>
	/// <seealso cref="World"/>
	/// <seealso cref="ElementBounds"/>
	/// <seealso cref="GhostRegion"/>
	/// <seealso cref="WorldProvider"/>
	/// <seealso cref="IWorldConsumer"/>
	/// <seealso cref="GhostableElement{TDerivedElement,TGhost}"/>
	/// <seealso cref="Rigidbody2DElementGhost"/>
	/// <seealso cref="Rigidbody2D"/>
	/// <seealso cref="Collider2D"/>
	/// <seealso cref="RigidbodyElement"/>
	[RequireComponent(typeof(Rigidbody2D))]
	[AddComponentMenu("Make It Wrap/Elements/Rigidbody 2D")]
	public class Rigidbody2DElement : GhostableElement<Rigidbody2DElement, Rigidbody2DElementGhost>, IWorldConsumer
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
			this.DisableAndThrowOnUnassignedReference(world, "The Rigidbody2DElement component requires a reference to a World component.");
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
				() => { return GameObjectHierarchy.GetCollider2DGroupAxisAlignedBoxBounds(transform); },
				() => { return GameObjectHierarchy.GetCollider2DGroupSphereBounds(transform); });

#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) UnityEditor.SceneView.RepaintAll();
#endif
		}

		protected void InstantiateGhost(GhostRegion ghostRegion)
		{
			var ghost = Instantiate(ghostPrefab);
			ghost.transform.SetParent(transform.parent, false);
			ghost.name = name + " (Rigidbody2D Ghost)";
			ghost.region = ghostRegion;
			ghost.original = this;

			var rigidbody = GetComponent<Rigidbody2D>();

			ghost.transform.localScale = rigidbody.transform.localScale;
			ghostRegion.Transform(rigidbody, ghost.GetComponent<Rigidbody2D>());

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
			RemoveMatchingComponentsFromGhost(components, (Component component) => { return !(component is Rigidbody2D || component is Collider2D || component is Transform); });
		}
	}
}
