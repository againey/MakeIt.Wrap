/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using System;
using Experilous.Core;

namespace Experilous.MakeItWrap
{
	/// <summary>
	/// A wrap-around world element with a mesh that ought to be rendered in visible ghost regions.
	/// </summary>
	/// <remarks>
	/// Attach this component to a game object with one or more mesh filters whenever you want
	/// those meshes to be visible across wrapped world boundaries.  Every frame, the element
	/// will manually add the meshes to the render queue with the appropriate transformations
	/// for each visible ghost region in which the ghost might also be visible.
	/// </remarks>
	/// <seealso cref="Viewport"/>
	/// <seealso cref="ElementBounds"/>
	/// <seealso cref="GhostRegion"/>
	/// <seealso cref="ViewportProvider"/>
	/// <seealso cref="IViewportConsumer"/>
	/// <seealso cref="MeshFilter"/>
	/// <seealso cref="MeshRenderer"/>
	[AddComponentMenu("Make It Wrap/Elements/Mesh")]
	public class MeshElement : BoundedElement, IViewportConsumer
	{
		public Viewport viewport;
		public ElementBoundsSource boundsSource = ElementBoundsSource.FixedScale | ElementBoundsSource.Automatic;
		public ElementBoundsProvider boundsProvider;

		protected MeshFilter[] _meshFilters;

		public bool hasViewport { get { return viewport != null ; } }
		public Viewport GetViewport() { return viewport; }
		public void SetViewport(Viewport viewport) { this.viewport = viewport; }

		[NonSerialized] protected ElementBounds _bounds;

		public override ElementBounds bounds { get { return _bounds; } }

		protected void Awake()
		{
			_meshFilters = GetComponentsInChildren<MeshFilter>();
		}

		protected void Start()
		{
			if (viewport == null) viewport = ViewportConsumerUtility.FindViewport(this);
			this.DisableAndThrowOnUnassignedReference(viewport, "The MeshElement component requires a reference to a Viewport component.");

			if (_bounds == null) RefreshBounds();
		}

		protected void LateUpdate()
		{
			foreach (var ghostRegion in viewport.visibleGhostRegions)
			{
				if (bounds.IsVisible(viewport, transform, ghostRegion))
				{
					RenderGhosts(ghostRegion.transformation);
				}
			}
		}

		public override void RefreshBounds()
		{
			_bounds = ElementBounds.CreateBounds(boundsSource, boundsProvider, transform,
				() => { return GameObjectHierarchy.GetMeshGroupAxisAlignedBoxBounds(transform); },
				() => { return GameObjectHierarchy.GetMeshGroupSphereBounds(transform); });

#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) UnityEditor.SceneView.RepaintAll();
#endif
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

#if UNITY_EDITOR
		protected override void OnDrawGizmosSelected()
		{
			if (bounds == null) RefreshBounds();
			if (bounds != null)
			{
				bounds.DrawGizmosSelected(transform, new Color(0f, 0.5f, 1f, 0.5f));
				DrawGhostGizmosSelected(new Color(0.25f, 0.5f, 1f, 0.25f));
			}
		}

		protected void DrawGhostGizmosSelected(Color color)
		{
			if (viewport == null) return;

			foreach (var ghostRegion in viewport.visibleGhostRegions)
			{
				if (bounds.IsVisible(viewport, transform, ghostRegion))
				{
					bounds.DrawGizmosSelected(transform, ghostRegion, color);
				}
			}
		}
#endif
	}
}
