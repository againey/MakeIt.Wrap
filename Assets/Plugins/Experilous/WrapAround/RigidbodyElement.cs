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
	/// <seealso cref="AbstractBounds"/>
	/// <seealso cref="GhostRegion"/>
	/// <seealso cref="WorldProvider"/>
	/// <seealso cref="IWorldConsumer"/>
	/// <seealso cref="GhostableElement`2{TDerivedElement,TGhost}"/>
	/// <seealso cref="RigidbodyElementGhost"/>
	/// <seealso cref="Rigidbody"/>
	/// <seealso cref="Collider"/>
	[RequireComponent(typeof(Rigidbody))]
	public class RigidbodyElement : GhostableElement<RigidbodyElement, RigidbodyElementGhost>, IWorldConsumer
	{
		public World world;
		public AbstractBounds bounds;

		public bool hasWorld { get { return world != null ; } }
		public void SetWorld(World world) { this.world = world; }

		public virtual bool IsCollidable(RigidbodyElementGhost ghost)
		{
			return bounds.IsCollidable(world, ghost.rigidbody);
		}

		public virtual bool IsCollidable(Vector3 position, Quaternion rotation)
		{
			return world.IsCollidable(position);
		}

		public bool IsCollidable(GhostRegion ghostRegion)
		{
			var position = transform.position;
			var rotation = transform.rotation;
			ghostRegion.Transform(ref position, ref rotation);
			return bounds.IsCollidable(world, position, rotation);
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
			this.DisableAndThrowOnMissingReference(world, "The RigidbodyElement component requires a reference to a World component.");
			this.DisableAndThrowOnMissingReference(bounds, "The RigidbodyElement component requires a reference to an AbstractBounds component.");
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

			var rigidbody = GetComponent<Rigidbody>();

			ghost.transform.localScale = rigidbody.transform.localScale;
			ghostRegion.Transform(rigidbody, ghost.GetComponent<Rigidbody>());

			Add(ghost);
		}
	}
}
