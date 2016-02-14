using UnityEngine;

namespace Experilous.WrapAround
{
	public class RenderableElement : MonoBehaviour, IViewportConsumer
	{
		public Viewport viewport;
		public AbstractBounds bounds;

		protected MeshFilter[] _meshFilters;

		public bool hasViewport { get { return viewport != null ; } }
		public void SetViewport(Viewport viewport) { this.viewport = viewport; }

		protected void Awake()
		{
			_meshFilters = GetComponentsInChildren<MeshFilter>();

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
			if (viewport == null) viewport = ViewportConsumerUtility.FindViewport(this);
		}

		protected void LateUpdate()
		{
			foreach (var ghostRegion in viewport.visibleGhostRegions)
			{
				var position = transform.position;
				var rotation = transform.rotation;
				ghostRegion.Transform(ref position, ref rotation);

				if (bounds.IsVisible(viewport, position, rotation))
				{
					RenderGhosts(ghostRegion.transformation);
				}
			}
		}

		protected void RenderGhosts(Matrix4x4 regionTransformation)
		{
			foreach (var meshFilter in _meshFilters)
			{
				var gameObject = meshFilter.gameObject;
				var renderer = meshFilter.GetComponent<MeshRenderer>();
				if (renderer != null && renderer.enabled && gameObject.activeInHierarchy)
				{
					var meshTransform = gameObject.transform;
					var ghostTransformation = regionTransformation * meshTransform.localToWorldMatrix;

					var mesh = meshFilter.mesh;
					for (int i = 0; i < mesh.subMeshCount; ++i)
					{
						Graphics.DrawMesh(mesh, ghostTransformation, renderer.materials[i], gameObject.layer, null, i, null, renderer.shadowCastingMode, renderer.receiveShadows);
					}
				}
			}
		}
	}
}
