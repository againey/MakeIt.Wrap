/******************************************************************************\
 *  Copyright (C) 2016 Experilous <againey@experilous.com>
 *  
 *  This file is subject to the terms and conditions defined in the file
 *  'Assets/Plugins/Experilous/License.txt', which is a part of this package.
 *
\******************************************************************************/

using UnityEngine;

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
	/// <seealso cref="AbstractBounds"/>
	/// <seealso cref="GhostRegion"/>
	/// <seealso cref="WorldProvider"/>
	/// <seealso cref="IWorldConsumer"/>
	/// <seealso cref="GhostableElement`2{TDerivedElement,TGhost}"/>
	/// <seealso cref="ColliderElementGhost"/>
	/// <seealso cref="Collider"/>
	[RequireComponent(typeof(Collider))]
	public class ColliderElement : GhostableElement<ColliderElement, ColliderElementGhost>, IWorldConsumer
	{
		public World world;
		public AbstractBounds bounds;

		public bool hasWorld { get { return world != null ; } }
		public void SetWorld(World world) { this.world = world; }

		public virtual bool IsCollidable(ColliderElementGhost ghost)
		{
			return bounds.IsCollidable(world, ghost.transform);
		}

		public virtual bool IsCollidable(Vector3 position, Quaternion rotation)
		{
			return bounds.IsCollidable(world, position, rotation);
		}

		public bool IsCollidable(GhostRegion ghostRegion)
		{
			var position = transform.position;
			var rotation = transform.rotation;
			ghostRegion.Transform(ref position, ref rotation);
			return IsCollidable(position, rotation);
		}

		protected void Awake()
		{
			if (bounds == null)
			{
				bounds = GetComponent<AbstractBounds>();
				if (bounds == null)
				{
					bounds = gameObject.AddComponent<PointBounds>();
				}
			}
		}

		protected new void Start()
		{
			base.Start();

			if (world == null) world = WorldConsumerUtility.FindWorld(this);
			this.DisableAndThrowOnMissingReference(world, "The ColliderElement component requires a reference to a World component.");
			this.DisableAndThrowOnMissingReference(bounds, "The ColliderElement component requires a reference to an AbstractBounds component.");
		}

		protected void FixedUpdate()
		{
			foreach (var ghostRegion in world.physicsGhostRegions)
			{
				if (FindGhost(ghostRegion) == null && IsCollidable(ghostRegion))
				{
					InstantiateGhost(ghostRegion);
				}
			}
		}

		protected void InstantiateGhost(GhostRegion ghostRegion)
		{
			var ghost = Instantiate(ghostPrefab);
			ghost.transform.SetParent(transform.parent, false);
			ghost.name = name + " (Ghost)";
			ghost.region = ghostRegion;
			ghost.original = this;

			ghost.transform.localScale = transform.localScale;
			ghostRegion.Transform(transform, ghost.transform);

			Add(ghost);
		}
	}
}
