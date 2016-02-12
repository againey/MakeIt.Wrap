using UnityEngine;
using System.Collections.Generic;

namespace Experilous.WrapAround
{
	public abstract class World : MonoBehaviour
	{
		public abstract void GetPhysicsGhostRegions(List<GhostRegion> ghostRegions);
		public abstract void GetVisibleGhostRegions(Camera camera, List<GhostRegion> ghostRegions);

		public abstract IEnumerable<GhostRegion> physicsGhostRegions { get; }

		public abstract bool IsCollidable(Vector3 position);
		public abstract bool IsCollidable(Vector3 position, float radius);
		public abstract bool IsCollidable(Bounds box);
		public abstract bool IsCollidable(Vector3 position, Bounds box);

		public abstract void Confine(Transform transform);
		public abstract void Confine(Rigidbody rigidbody);
	}
}
