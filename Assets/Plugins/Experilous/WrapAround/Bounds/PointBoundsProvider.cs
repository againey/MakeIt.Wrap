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
	public class PointBoundsProvider : ElementBoundsProvider
	{
		public Vector3 position;

		public override ElementBounds CreateBounds(bool fixedScale, bool fixedOrientation)
		{
			return PointBounds.Create(position, fixedScale, fixedOrientation);
		}
	}
}
