using UnityEngine;

namespace Experilous.WrapAround
{
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

			ghost.transform.localScale = transform.localScale;
			ghostRegion.Transform(transform, ghost.transform);

			Add(ghost);
		}
	}
}
