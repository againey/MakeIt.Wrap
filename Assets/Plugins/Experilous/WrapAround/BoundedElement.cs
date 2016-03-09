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
