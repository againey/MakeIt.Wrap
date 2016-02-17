using UnityEngine;

namespace Experilous.WrapAround
{
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

		protected void Start()
		{
			if (world == null) world = WorldConsumerUtility.FindWorld(this);
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
