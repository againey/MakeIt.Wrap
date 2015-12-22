using UnityEngine;

namespace Experilous.WrapAround
{
	public class SphereBoundedRenderableElement : RenderableElement
	{
		public float radius;

		protected new void LateUpdate()
		{
			var position = transform.position;
			var scale = transform.lossyScale;
			var scaledRadius = radius * Mathf.Max(Mathf.Max(scale.x, scale.y), scale.z);
			foreach (var ghostRegion in viewport.visibleGhostRegions)
			{
				var regionTransformation = ghostRegion.transformation;
				var ghostPosition = regionTransformation.MultiplyPoint3x4(position);
				if (viewport.IsVisible(ghostPosition, scaledRadius))
				{
					RenderGhosts(regionTransformation);
				}
			}
		}

		protected void Start()
		{
			if (radius == 0f)
			{
				var meshFilter = GetComponent<MeshFilter>();
				if (meshFilter != null)
				{
					var mesh = meshFilter.mesh;
					if (mesh != null)
					{
						radius = mesh.bounds.extents.magnitude;
					}
				}
			}
		}
	}
}
