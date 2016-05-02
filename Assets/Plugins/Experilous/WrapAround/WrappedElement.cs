/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	[ExecuteInEditMode]
	public class WrappedElement : MonoBehaviour
	{
#if UNITY_EDITOR
		public bool fixedScale;
		public bool fixedRotation;

		public enum BoundsType
		{
			Automatic,
			LocalOrigin,
			Point,
			Sphere,
			AxisAlignedBox,
			Reference,
		}

		[System.Serializable]
		public class BoundsState
		{
			public BoundsType type;
			public BoundedElement reference;

			public BoundsState()
			{
				type = BoundsType.Automatic;
				reference = null;
			}

			public BoundsState(BoundsType type)
			{
				if (type == BoundsType.Reference) throw new System.ArgumentException();
				this.type = type;
				reference = null;
			}

			public BoundsState(BoundedElement reference)
			{
				type = BoundsType.Reference;
				this.reference = reference;
			}
		}

		public LightElement wrappedLightElement;
		public BoundsState wrappedLightBounds = new BoundsState();
		public ColliderElement wrappedColliderElement;
		public BoundsState wrappedColliderBounds = new BoundsState();
		public RigidbodyElement wrappedRigidbodyElement;
		public BoundsState wrappedRigidbodyBounds = new BoundsState();

		public bool canHaveWrappedLightElement
		{
			get
			{
				var lights = GetComponentsInChildren<Light>();
				foreach (var light in lights)
				{
					if (light.type == LightType.Point || light.type == LightType.Spot)
					{
						return true;
					}
				}

				return false;
			}
		}

		public bool canHaveWrappedColliderElement
		{
			get
			{
				var ancestorRigidbodies = GetComponentsInParent<Rigidbody>(true);
				if (ancestorRigidbodies.Length > 0)
				{
					return false;
				}

				var colliders = GetComponentsInChildren<Collider>();
				foreach (var collider in colliders)
				{
					if (ReferenceEquals(collider.gameObject, gameObject))
					{
						return true;
					}

					ancestorRigidbodies = GetComponentsInParent<Rigidbody>(true);
					if (ancestorRigidbodies.Length == 0)
					{
						return true;
					}
				}

				return false;
			}
		}

		public bool canHaveWrappedRigidbodyElement
		{
			get
			{
				return GetComponent<Rigidbody>() != null;
			}
		}
#endif
	}
}
