/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using Experilous.MakeItTile;
using Experilous.Core;

namespace Experilous.MakeItWrap
{
	[AddComponentMenu("Make It Wrap/Worlds/Tiled Planar World")]
	public class TiledPlanarWorld : RhomboidWorldBase
	{
		[Tooltip("The tiled surface that defines the overall shape of this world.")]
		public PlanarSurface surface;

		protected new void Start()
		{
			this.DisableAndThrowOnUnassignedReference(surface, "The TiledPlanarWorld component requires a reference to a PlanarSurface component.");

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
