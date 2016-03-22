/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	public abstract class BoundedElement : MonoBehaviour
	{
		[SerializeField] private ElementBounds _bounds;
		public abstract Sphere ComputeSphereBounds();
		public abstract Bounds ComputeAxisAlignedBoxBounds();

		public ElementBounds bounds
		{
			get
			{
				return _bounds;
			}
			set
			{
				if (value == null) throw new System.ArgumentNullException("value");
				if (!ReferenceEquals(gameObject, value.gameObject)) throw new System.ArgumentException("Bounds component must belong to the same game object as the bounded element to which it is being assigned.", "value");
				if (ReferenceEquals(_bounds, value)) return;

				DestroyBounds();

				_bounds = value;
				_bounds.hideFlags = HideFlags.HideInInspector;
			}
		}

		protected void Reset()
		{
			DestroyBounds();
			_bounds = LocalOriginBounds.Create(gameObject);
		}

		private void DestroyBounds()
		{
			if (_bounds == null) return;
#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
			{
				DestroyImmediate(_bounds);
			}
			else
			{
#endif
				Destroy(_bounds);
#if UNITY_EDITOR
			}
#endif
		}
	}
}
