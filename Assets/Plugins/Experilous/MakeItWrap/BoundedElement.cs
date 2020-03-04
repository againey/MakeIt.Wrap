/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.MakeItWrap
{
	public interface IBoundedElement
	{
		ElementBounds bounds { get; }
		void RefreshBounds();
	}

	public abstract class BoundedElement : MonoBehaviour, IBoundedElement
	{
		public abstract ElementBounds bounds { get; }
		public virtual void RefreshBounds() { }

#if UNITY_EDITOR
		protected virtual void OnDrawGizmosSelected()
		{
			if (bounds == null) RefreshBounds();
			if (bounds != null) bounds.DrawGizmosSelected(transform, Color.white);
		}
#endif
	}
}
