using UnityEngine;

namespace Experilous.WrapAround
{
	public abstract class Element : MonoBehaviour
	{
		public World World;

		[SerializeField, HideInInspector]
		private bool _isGhost = false;

		public bool isGhost { get { return _isGhost; } }

		protected void Update()
		{
			if (!_isGhost)
			{
				World.Confine(this);
			}
		}

		protected void LateUpdate()
		{
			if (!_isGhost)
			{
				foreach (var ghostRegion in World.Viewport.visibleGhostRegions)
				{
					if (!ghostRegion.HasGhost(this))
					{
						Vector3 position = transform.position;
						Quaternion rotation = transform.rotation;
						ghostRegion.Transform(ref position, ref rotation);

						if (IsVisible(position, rotation))
						{
							CreateGhost(ghostRegion, position, rotation);
						}
					}
				}
			}
		}

		public abstract bool IsVisible(Vector3 position, Quaternion rotation);

		protected virtual void CreateGhost(GhostRegion region, Vector3 position, Quaternion rotation)
		{
			InstantiateGhost<ElementGhost>(region, position, rotation);
		}

		protected TGhost InstantiateGhost<TGhost>(GhostRegion region, Vector3 position, Quaternion rotation) where TGhost : ElementGhost
		{
			_isGhost = true; // So that _isGhost is true for the ghost when Awake() is called.
			var clone = (GameObject)Instantiate(gameObject, position, rotation);
			_isGhost = false;
			clone.transform.SetParent(transform.parent, false);
			var ghost = clone.AddComponent<TGhost>();
			ghost.Original = this;
			ghost.Region = region;
			region.AddElement(this);
			return ghost;
		}
	}
}
