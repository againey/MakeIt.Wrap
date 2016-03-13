/******************************************************************************\
 *  Copyright (C) 2016 Experilous <againey@experilous.com>
 *  
 *  This file is subject to the terms and conditions defined in the file
 *  'Assets/Plugins/Experilous/License.txt', which is a part of this package.
 *
\******************************************************************************/

using UnityEngine;
using Experilous.Topological;

namespace Experilous.WrapAround
{
	public class TopologicalPlanarWorld : RhomboidWorldBase
	{
		[Tooltip("The topological surface that defines the overall shape of this world.")]
		public PlanarSurface surface;

		protected new void Start()
		{
			this.DisableAndThrowOnUnassignedReference(surface, "The TopologicalPlanarWorld component requires a reference to a PlanarSurface component.");

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
