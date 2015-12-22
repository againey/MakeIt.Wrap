using UnityEngine;

namespace Experilous.WrapAround
{
	[RequireComponent(typeof(Collider))]
	public class ColliderElement : MonoBehaviour
	{
		public Viewport viewport;
		public ColliderElementGhost ghostPrefab;

		public virtual bool IsVisible(ColliderElementGhost ghost)
		{
			return viewport.IsVisible(ghost.transform.position);
		}

		public virtual bool IsVisible(Vector3 position, Quaternion rotation)
		{
			return viewport.IsVisible(position);
		}

		public bool IsVisible(GhostRegion ghostRegion)
		{
			var position = transform.position;
			var rotation = transform.rotation;
			ghostRegion.Transform(ref position, ref rotation);
			return IsVisible(position, rotation);
		}

		protected void FixedUpdate()
		{
			foreach (var ghostRegion in viewport.visibleGhostRegions)
			{
				if (!ghostRegion.HasGhost(GetInstanceID()) && IsVisible(ghostRegion))
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

			ghostRegion.AddElement(GetInstanceID());
		}
	}
}
