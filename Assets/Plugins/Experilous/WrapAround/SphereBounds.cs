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
	public class SphereBounds : AbstractBounds
	{
		public float radius;

		protected float scaledRadius
		{
			get
			{
				var scale = transform.lossyScale;
				return radius * Mathf.Max(Mathf.Max(scale.x, scale.y), scale.z);
			}
		}
		
		protected void Start()
		{
			if (radius == 0f)
			{
				var sphereCollider = GetComponent<SphereCollider>();
				if (sphereCollider != null)
				{
					radius = sphereCollider.radius;
				}
				else
				{
					var collider = GetComponent<Collider>();
					radius = collider.bounds.extents.magnitude;
				}
			}
		}

		public override bool IsVisible(Viewport viewport)
		{
			return viewport.IsVisible(transform.position, scaledRadius);
		}

		public override bool IsVisible(Viewport viewport, Vector3 position)
		{
			return viewport.IsVisible(position, scaledRadius);
		}

		public override bool IsVisible(Viewport viewport, Vector3 position, Quaternion rotation)
		{
			return viewport.IsVisible(position, scaledRadius);
		}

		public override bool IsVisible(Viewport viewport, Transform transform)
		{
			return viewport.IsVisible(transform.position, scaledRadius);
		}

		public override bool IsVisible(Viewport viewport, Rigidbody rigidbody)
		{
			return viewport.IsVisible(rigidbody.position, scaledRadius);
		}

		public override bool IsCollidable(World world)
		{
			return world.IsCollidable(transform.position, scaledRadius);
		}

		public override bool IsCollidable(World world, Vector3 position)
		{
			return world.IsCollidable(position, scaledRadius);
		}

		public override bool IsCollidable(World world, Vector3 position, Quaternion rotation)
		{
			return world.IsCollidable(position, scaledRadius);
		}

		public override bool IsCollidable(World world, Transform transform)
		{
			return world.IsCollidable(transform.position, scaledRadius);
		}

		public override bool IsCollidable(World world, Rigidbody rigidbody)
		{
			return world.IsCollidable(rigidbody.position, scaledRadius);
		}
	}
}
