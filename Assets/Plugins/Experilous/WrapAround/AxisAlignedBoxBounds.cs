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
	public class AxisAlignedBoxBounds : AbstractBounds
	{
		public Bounds box;

		protected void Start()
		{
			if (box.extents == Vector3.zero)
			{
				var meshFilter = GetComponent<MeshFilter>();
				if (meshFilter != null && meshFilter.mesh != null)
				{
					var meshBounds = meshFilter.mesh.bounds;
					var min = meshBounds.min;
					var max = meshBounds.max;
					box = new Bounds(transform.TransformVector(min), Vector3.zero);
					box.Encapsulate(transform.TransformVector(new Vector3(min.x, min.y, max.z)));
					box.Encapsulate(transform.TransformVector(new Vector3(min.x, max.y, min.z)));
					box.Encapsulate(transform.TransformVector(new Vector3(min.x, max.y, max.z)));
					box.Encapsulate(transform.TransformVector(new Vector3(max.x, min.y, min.z)));
					box.Encapsulate(transform.TransformVector(new Vector3(max.x, min.y, max.z)));
					box.Encapsulate(transform.TransformVector(new Vector3(max.x, max.y, min.z)));
					box.Encapsulate(transform.TransformVector(max));
				}
			}
		}

		public override bool IsVisible(Viewport viewport)
		{
			return viewport.IsVisible(transform.position, box);
		}

		public override bool IsVisible(Viewport viewport, Vector3 position)
		{
			return viewport.IsVisible(position, box);
		}

		public override bool IsVisible(Viewport viewport, Vector3 position, Quaternion rotation)
		{
			return viewport.IsVisible(position, box);
		}

		public override bool IsVisible(Viewport viewport, Transform transform)
		{
			return viewport.IsVisible(transform.position, box);
		}

		public override bool IsVisible(Viewport viewport, Rigidbody rigidbody)
		{
			return viewport.IsVisible(rigidbody.position, box);
		}

		public override bool IsCollidable(World world)
		{
			return world.IsCollidable(transform.position, box);
		}

		public override bool IsCollidable(World world, Vector3 position)
		{
			return world.IsCollidable(position, box);
		}

		public override bool IsCollidable(World world, Vector3 position, Quaternion rotation)
		{
			return world.IsCollidable(position, box);
		}

		public override bool IsCollidable(World world, Transform transform)
		{
			return world.IsCollidable(transform.position, box);
		}

		public override bool IsCollidable(World world, Rigidbody rigidbody)
		{
			return world.IsCollidable(rigidbody.position, box);
		}

#if UNITY_EDITOR
		protected void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireCube(transform.position + box.center, box.size);
		}
#endif
	}
}
