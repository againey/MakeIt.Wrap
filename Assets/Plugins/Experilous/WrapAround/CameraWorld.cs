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
	public class CameraWorld : RhomboidWorldBase
	{
		public Camera screenCamera;

		public float wrappingPlaneDistance = 0f;

		public float nearPlaneDistance = 0f;
		public float farPlaneDistance = 0f;

		public bool wrapHorizontalAxis = true;
		public bool wrapVerticalAxis = true;
		public bool wrapForwardAxis = false;

		private Vector3 _untransformedOrigin;
		private Vector3 _untransformedAxis0Vector;
		private Vector3 _untransformedAxis1Vector;
		private Vector3 _untransformedAxis2Vector;

		protected new void Start()
		{
			if (screenCamera == null) screenCamera = GetComponent<Camera>();
			this.DisableAndThrowOnUnassignedReference(screenCamera, "The CameraWorld component requires a reference to a Camera component.");

			if (nearPlaneDistance == 0f && farPlaneDistance == 0f)
			{
				nearPlaneDistance = screenCamera.nearClipPlane;
				farPlaneDistance = screenCamera.farClipPlane;
			}

			if (wrappingPlaneDistance == 0f)
			{
				wrappingPlaneDistance = (nearPlaneDistance + farPlaneDistance) * 0.5f;
			}

			var frustumPlanes = UnityEngine.GeometryUtility.CalculateFrustumPlanes(screenCamera);
			var leftPlaneIndex = GeometryUtility.FindMatchingPlane(screenCamera.transform.right, frustumPlanes);
			var rightPlaneIndex = GeometryUtility.FindMatchingPlane(-screenCamera.transform.right, frustumPlanes);
			var lowerPlaneIndex = GeometryUtility.FindMatchingPlane(screenCamera.transform.up, frustumPlanes);
			var upperPlaneIndex = GeometryUtility.FindMatchingPlane(-screenCamera.transform.up, frustumPlanes);
			var wrappingPlane = new Plane(screenCamera.transform.forward, screenCamera.transform.position + screenCamera.transform.forward * nearPlaneDistance);

			var origin = GeometryUtility.Intersect(frustumPlanes[leftPlaneIndex], frustumPlanes[lowerPlaneIndex], wrappingPlane);
			var rightPosition = GeometryUtility.Intersect(frustumPlanes[rightPlaneIndex], frustumPlanes[lowerPlaneIndex], wrappingPlane);
			var upperPosition = GeometryUtility.Intersect(frustumPlanes[leftPlaneIndex], frustumPlanes[upperPlaneIndex], wrappingPlane);

			_untransformedOrigin = transform.InverseTransformPoint(origin);
			_untransformedAxis0Vector = transform.InverseTransformVector(rightPosition - origin);
			_untransformedAxis1Vector = transform.InverseTransformVector(upperPosition - origin);
			_untransformedAxis2Vector = transform.InverseTransformVector(screenCamera.transform.forward * (farPlaneDistance - nearPlaneDistance));

			base.Start();
		}

		public override Vector3 untransformedOrigin { get { return _untransformedOrigin; } }
		public override Vector3 untransformedAxis0Vector { get { return _untransformedAxis0Vector; } }
		public override Vector3 untransformedAxis1Vector { get { return _untransformedAxis1Vector; } }
		public override Vector3 untransformedAxis2Vector { get { return _untransformedAxis2Vector; } }

		public override bool axis0IsWrapped { get { return wrapHorizontalAxis; } }
		public override bool axis1IsWrapped { get { return wrapVerticalAxis; } }
		public override bool axis2IsWrapped { get { return wrapForwardAxis; } }
	}
}
