/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
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
	/// <seealso cref="GhostableElement`2{TDerivedElement,TGhost}"/>
	/// <seealso cref="RigidbodyElementGhost"/>
	/// <seealso cref="Rigidbody"/>
	/// <seealso cref="Collider"/>
	/// <seealso cref="Rigidbody2DElement"/>
	[RequireComponent(typeof(Rigidbody))]
	public class RigidbodyElement : GhostableElement<RigidbodyElement, RigidbodyElementGhost>, IWorldConsumer
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
			this.DisableAndThrowOnUnassignedReference(world, "The RigidbodyElement component requires a reference to a World component.");
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
				() => { return HierarchyUtility.GetColliderGroupAxisAlignedBoxBounds(transform); },
				() => { return HierarchyUtility.GetColliderGroupSphereBounds(transform); });

#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) UnityEditor.SceneView.RepaintAll();
#endif
		}

		protected void InstantiateGhost(GhostRegion ghostRegion)
		{
			var ghost = Instantiate(ghostPrefab);
			ghost.transform.SetParent(transform.parent, false);
			ghost.name = name + " (Rigidbody Ghost)";
			ghost.region = ghostRegion;
			ghost.original = this;

			var rigidbody = GetComponent<Rigidbody>();

			ghost.transform.localScale = rigidbody.transform.localScale;
			ghostRegion.Transform(rigidbody, ghost.GetComponent<Rigidbody>());

			Add(ghost);
		}

		protected override bool IsGameObjectExcludedFromGhost(Component[] components)
		{
			foreach (var component in components)
			{
				if (component is Rigidbody)
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
				if (component is Collider)
				{
					return true;
				}
			}
			return false;
		}

		protected override void RemoveUnnecessaryComponentsFromGhost(Component[] components)
		{
			RemoveMatchingComponentsFromGhost(components, (Component component) => { return !(component is Rigidbody || component is Collider || component is Transform); });
		}
	}
}
