using UnityEngine;

namespace Experilous.WrapAround
{
	[RequireComponent(typeof(Light))]
	public class LightElement : GhostableElement<LightElement, LightElementGhost>
	{
		public Viewport viewport;
		public AbstractBounds bounds;

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

		protected void Start()
		{
			if (viewport == null)
			{
				var provider = GetComponentInParent<ViewportProvider>();
				if (provider != null)
				{
					viewport = provider.viewport;
				}
			}
		}

		protected void LateUpdate()
		{
			foreach (var ghostRegion in viewport.visibleGhostRegions)
			{
				if (FindGhost(ghostRegion) == null && IsVisible(ghostRegion))
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

			Add(ghost);
		}
	}
}
