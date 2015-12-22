﻿using UnityEngine;

namespace Experilous.WrapAround
{
	public class RenderableElement : MonoBehaviour
	{
		public Viewport viewport;

		protected MeshFilter[] _meshFilters;

		protected void Awake()
		{
			_meshFilters = GetComponentsInChildren<MeshFilter>();
		}

		protected void LateUpdate()
		{
			var position = transform.position;
			foreach (var ghostRegion in viewport.visibleGhostRegions)
			{
				var regionTransformation = ghostRegion.transformation;
				var ghostPosition = regionTransformation.MultiplyPoint3x4(position);
				if (viewport.IsVisible(ghostPosition))
				{
					RenderGhosts(regionTransformation);
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
