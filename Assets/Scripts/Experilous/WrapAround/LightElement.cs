using UnityEngine;

namespace Experilous.WrapAround
{
	[RequireComponent(typeof(Light))]
	public class LightElement : MonoBehaviour
	{
		public Viewport viewport;
		public AbstractBounds bounds;
		public LightElementGhost ghostPrefab;

		public virtual bool IsVisible(LightElementGhost ghost)
		{
			return bounds.IsVisible(viewport, ghost.transform);
		}

		public virtual bool IsVisible(Vector3 position, Quaternion rotation)
		{
			return bounds.IsVisible(viewport, position, rotation);
		}

		public bool IsVisible(GhostRegion ghostRegion)
		{
			var position = transform.position;
			var rotation = transform.rotation;
			ghostRegion.Transform(ref position, ref rotation);
			return IsVisible(position, rotation);
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

		protected void LateUpdate()
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

			ghostRegion.Transform(transform, ghost.transform);

			ghostRegion.AddElement(GetInstanceID());
		}
	}
}
