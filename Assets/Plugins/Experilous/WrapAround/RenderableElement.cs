/******************************************************************************\
 *  Copyright (C) 2016 Experilous <againey@experilous.com>
 *  
 *  This file is subject to the terms and conditions defined in the file
 *  'Assets/Plugins/Experilous/License.txt', which is a part of this package.
 *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	/// <summary>
	/// A wrap-around world element that ought to be rendered in visible ghost regions.
	/// </summary>
	/// <remarks>
	/// Attach this component to a game object with one or more mesh filters whenever you want
	/// those meshes to be visible across wrapped world boundaries.  Every frame, the element
	/// will manually add the meshes to the render queue with the appropriate transformations
	/// for each visible ghost region in which the ghost might also be visible.
	/// </remarks>
	/// <seealso cref="Viewport"/>
	/// <seealso cref="AbstractBounds"/>
	/// <seealso cref="GhostRegion"/>
	/// <seealso cref="ViewportProvider"/>
	/// <seealso cref="IViewportConsumer"/>
	/// <seealso cref="MeshFilter"/>
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
			this.DisableAndThrowOnUnassignedReference(viewport, "The RenderableElement component requires a reference to a Viewport component.");
			this.DisableAndThrowOnUnassignedReference(bounds, "The RenderableElement component requires a reference to an AbstractBounds component.");
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
