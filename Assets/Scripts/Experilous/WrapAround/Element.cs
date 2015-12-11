using UnityEngine;

namespace Experilous.WrapAround
{
	public abstract class Element : MonoBehaviour
	{
		public World world;
		public bool interactsAcrossEdges = false;

		[SerializeField, HideInInspector]
		private bool _isGhost = false;

		public bool isGhost { get { return _isGhost; } }

		protected void Update()
		{
			if (!_isGhost)
			{
				world.Confine(this);
			}
		}

		protected void LateUpdate()
		{
			if (!_isGhost)
			{
				var cameraViewport = world.cameraViewport;
				foreach (var ghostRegion in cameraViewport.visibleGhostRegions)
				{
					if (!ghostRegion.HasGhost(this))
					{
						Vector3 position = transform.position;
						Quaternion rotation = transform.rotation;
						ghostRegion.Transform(ref position, ref rotation);

						if (IsVisible(cameraViewport, position, rotation))
						{
							CreateGhost(ghostRegion, position, rotation);
						}
					}
				}

				if (interactsAcrossEdges)
				{
					var physicsViewport = world.physicsViewport;
					foreach (var ghostRegion in physicsViewport.visibleGhostRegions)
					{
						if (!ghostRegion.HasGhost(this))
						{
							Vector3 position = transform.position;
							Quaternion rotation = transform.rotation;
							ghostRegion.Transform(ref position, ref rotation);

							if (IsVisible(physicsViewport, position, rotation))
							{
								CreateGhost(ghostRegion, position, rotation);
							}
						}
					}
				}
			}
		}

		public abstract bool IsVisible(Viewport viewport, Vector3 position, Quaternion rotation);

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
			ghost.original = this;
			ghost.region = region;
			region.AddElement(this);
			return ghost;
		}
	}
}
