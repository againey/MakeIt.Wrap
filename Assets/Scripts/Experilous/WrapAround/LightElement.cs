using UnityEngine;

namespace Experilous.WrapAround
{
	[RequireComponent(typeof(Light))]
	public class LightElement : MonoBehaviour
	{
		public Viewport viewport;
		public LightElementGhost ghostPrefab;

		public virtual bool IsVisible(LightElementGhost ghost)
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

		protected void Start()
		{
			if (ghostPrefab == null)
			{
				ghostPrefab = defaultGhostPrefab;
			}
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

			ghost.region = ghostRegion;
			ghost.original = this;

			ghostRegion.AddElement(GetInstanceID());

			ghost.gameObject.SetActive(true);
		}

		protected LightElementGhost defaultGhostPrefab
		{
			get
			{
				var prefab = Instantiate(this);
				prefab.gameObject.SetActive(false);
				prefab.transform.SetParent(transform.parent, false);
				prefab.name = name + " (Ghost)";
				prefab.enabled = false;
				var ghost = prefab.gameObject.AddComponent<LightElementGhost>();
				Destroy(prefab);
				return ghost;
			}
		}
	}
}
