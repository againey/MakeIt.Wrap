using UnityEngine;

namespace Experilous.WrapAround
{
	[RequireComponent(typeof(Rigidbody))]
	public class SphereBoundedRigidbodyElement : RigidbodyElement
	{
		public float radius;

		private float scaledRadius
		{
			get
			{
				var scale = transform.lossyScale;
				return radius * Mathf.Max(Mathf.Max(scale.x, scale.y), scale.z);
			}
		}

		public override bool IsVisible(RigidbodyTopLevelElementGhost ghost)
		{
			return viewport.IsVisible(ghost.transform.position, scaledRadius);
		}

		public override bool IsVisible(Vector3 position, Quaternion rotation)
		{
			return viewport.IsVisible(position, scaledRadius);
		}
	}
}
