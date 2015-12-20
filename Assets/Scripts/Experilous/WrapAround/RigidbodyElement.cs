using UnityEngine;

namespace Experilous.WrapAround
{
	[RequireComponent(typeof(Rigidbody))]
	public class RigidbodyElement : MonoBehaviour
	{
		public Viewport viewport;
		public RigidbodyElementGhost ghostPrefab;

		public virtual bool IsVisible(RigidbodyTopLevelElementGhost ghost)
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

			var rigidbody = GetComponent<Rigidbody>();

			var ghostComponents = ghost.GetComponentsInChildren<RigidbodyElementGhost>();
			foreach (var ghostComponent in ghostComponents)
			{
				ghostComponent.region = ghostRegion;
				ghostComponent.original = rigidbody; //TODO:  This is the wrong rigidbody!  Needs to be the appropriate child.
				ghostComponent.transform.localScale = rigidbody.transform.localScale;
				ghostRegion.Transform(rigidbody, ghostComponent.GetComponent<Rigidbody>());
			}

			var topLevel = ghost.GetComponent<RigidbodyTopLevelElementGhost>();
			topLevel.region = ghostRegion;
			topLevel.original = this;

			ghostRegion.AddElement(GetInstanceID());

			ghost.gameObject.SetActive(true);
		}

		protected RigidbodyElementGhost defaultGhostPrefab
		{
			get
			{
				var prefab = Instantiate(this);
				prefab.gameObject.SetActive(false);
				prefab.transform.SetParent(transform.parent, false);
				prefab.name = name + " (Ghost)";
				AdjustComponents(prefab.transform);
				return prefab.GetComponent<RigidbodyElementGhost>();
			}
		}

		protected bool AdjustComponents(Transform transform)
		{
			var hasChildren = false;
			for (int i = 0; i < transform.childCount; ++i)
			{
				hasChildren = AdjustComponents(transform.GetChild(i)) || hasChildren;
			}

			var components = transform.GetComponents<Component>();
			var hasRigidbody = false;
			var hasCollider = false;
			foreach (var component in components)
			{
				if (component is Rigidbody)
				{
					hasRigidbody = true;
				}
				else if (component is Collider)
				{
					hasCollider = true;
				}
			}

			if (!hasRigidbody && !hasCollider)
			{
				if (!hasChildren)
				{
					Destroy(transform.gameObject);
					return false;
				}
				else
				{
					foreach (var component in components)
					{
						if (!(component is Transform))
						{
							Destroy(component);
						}
					}
					return true;
				}
			}
			else
			{
				var gameObject = transform.gameObject;
				foreach (var component in components)
				{
					if (component is Rigidbody)
					{
						var rigidbodyGhost = gameObject.AddComponent<RigidbodyElementGhost>();
						rigidbodyGhost.original = component as Rigidbody;
					}
					else if (!(component is RigidbodyElementGhost || component is Collider || component is Transform))
					{
						Destroy(component);
					}
				}
				return true;
			}
		}
	}
}
