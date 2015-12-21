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
	}
}
