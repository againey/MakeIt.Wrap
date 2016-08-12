/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	/// <summary>
	/// A world with wrap-around behavior whose shape is defined by a camera's view frustum at a specified depth.
	/// </summary>
	/// <remarks>This component is convenient when wrap-around behavior is expected to occur literally at the
	/// screen or viewport edges.  It is most appropriate for a fixed camera, ideally one with an orthographic
	/// projection.</remarks>
	[AddComponentMenu("Wrap-Around Worlds/Worlds/Camera World")]
	public class CameraWorld : RhomboidWorldBase
	{
		[Tooltip("The camera whose view frustum will define the shape of this world.")]
		public Camera screenCamera;

		[Tooltip("The depth at which the view frustum is used to define the shape of this world.  Irrelevant for a camera with an orthographic projection.")]
		public float wrappingPlaneDistance = 0f;

		[Tooltip("The near depth at which wrapping behavior will occur, if wrapping along the forward axis is enabled.")]
		public float nearPlaneDistance = 0f;

		[Tooltip("The far depth at which wrapping behavior will occur, if wrapping along the forward axis is enabled.")]
		public float farPlaneDistance = 0f;

		[Tooltip("Wrap across the left and right edges of the view.")]
		public bool wrapHorizontalAxis = true;

		[Tooltip("Wrap across the top and bottom edges of the view.")]
		public bool wrapVerticalAxis = true;

		[Tooltip("Wrap across the near and far of the view.")]
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
			var leftPlaneIndex = GeometryTools.FindMatchingPlane(screenCamera.transform.right, frustumPlanes);
			var rightPlaneIndex = GeometryTools.FindMatchingPlane(-screenCamera.transform.right, frustumPlanes);
			var lowerPlaneIndex = GeometryTools.FindMatchingPlane(screenCamera.transform.up, frustumPlanes);
			var upperPlaneIndex = GeometryTools.FindMatchingPlane(-screenCamera.transform.up, frustumPlanes);
			var wrappingPlane = new Plane(screenCamera.transform.forward, screenCamera.transform.position + screenCamera.transform.forward * nearPlaneDistance);

			var origin = GeometryTools.Intersect(frustumPlanes[leftPlaneIndex], frustumPlanes[lowerPlaneIndex], wrappingPlane);
			var rightPosition = GeometryTools.Intersect(frustumPlanes[rightPlaneIndex], frustumPlanes[lowerPlaneIndex], wrappingPlane);
			var upperPosition = GeometryTools.Intersect(frustumPlanes[leftPlaneIndex], frustumPlanes[upperPlaneIndex], wrappingPlane);

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
