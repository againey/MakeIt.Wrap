﻿/******************************************************************************\
 *  Copyright (C) 2016 Experilous <againey@experilous.com>
 *  
 *  This file is subject to the terms and conditions defined in the file
 *  'Assets/Plugins/Experilous/License.txt', which is a part of this package.
 *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	public class RhomboidWorld : RhomboidWorldBase
	{
		public Vector3 origin;
		public Vector3 axis0;
		public Vector3 axis1;
		public Vector3 axis2;
		public bool wrapAxis0;
		public bool wrapAxis1;
		public bool wrapAxis2;

		public override Vector3 untransformedOrigin { get { return origin; } }
		public override Vector3 untransformedAxis0Vector { get { return axis0; } }
		public override Vector3 untransformedAxis1Vector { get { return axis1; } }
		public override Vector3 untransformedAxis2Vector { get { return axis2; } }

		public override bool axis0IsWrapped { get { return wrapAxis0; } }
		public override bool axis1IsWrapped { get { return wrapAxis1; } }
		public override bool axis2IsWrapped { get { return wrapAxis2; } }
	}
}
