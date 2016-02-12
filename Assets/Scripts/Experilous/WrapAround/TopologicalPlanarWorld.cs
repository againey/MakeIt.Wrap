using UnityEngine;
using System.Collections.Generic;
using Experilous.Topological;
using System;

namespace Experilous.WrapAround
{
	public class TopologicalPlanarWorld : RhomboidWorldBase
	{
		public PlanarSurface surface;

		protected new void Start()
		{
			if (surface == null)
			{
				enabled = false;
				throw new MissingReferenceException("A topological planar world component requires a planar surface in order to function.");
			}

			base.Start();
		}

		public override Vector3 untransformedOrigin { get { return surface.origin; } }
		public override Vector3 untransformedAxis0Vector { get { return surface.axis0.vector; } }
		public override Vector3 untransformedAxis1Vector { get { return surface.axis1.vector; } }
		public override Vector3 untransformedAxis2Vector { get { return surface.surfaceNormal; } }

		public override bool axis0IsWrapped { get { return surface.axis0.isWrapped; } }
		public override bool axis1IsWrapped { get { return surface.axis1.isWrapped; } }
		public override bool axis2IsWrapped { get { return false; } }
	}
}
