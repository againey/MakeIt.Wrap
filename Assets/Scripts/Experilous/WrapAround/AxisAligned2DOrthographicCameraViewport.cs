using System;
using System.Collections.Generic;
using UnityEngine;

namespace Experilous.WrapAround
{
	[RequireComponent(typeof(Camera))]
	public class AxisAligned2DOrthographicCameraViewport : AxisAligned2DViewport
	{
		private Camera _camera;

		protected new void Start()
		{
			_camera = GetComponent<Camera>();
			if (!_camera.orthographic) throw new InvalidOperationException("The camera component associated with an axis aligned 2D orthographic camera viewport must have its orthographic property set to true.");
			RecalculateBounds();
			base.Start();
		}

		protected void LateUpdate()
		{
			RecalculateBounds();
			RecalculateVisibleGhostRegions();
		}

		protected void RecalculateBounds()
		{
			var nearBottomLeft = _camera.ViewportToWorldPoint(new Vector3(0f, 0f, _camera.nearClipPlane));
			var nearBottomRight = _camera.ViewportToWorldPoint(new Vector3(1f, 0f, _camera.nearClipPlane));
			var nearTopLeft = _camera.ViewportToWorldPoint(new Vector3(0f, 1f, _camera.nearClipPlane));
			var nearTopRight = _camera.ViewportToWorldPoint(new Vector3(1f, 1f, _camera.nearClipPlane));
			var farBottomLeft = _camera.ViewportToWorldPoint(new Vector3(0f, 0f, _camera.farClipPlane));
			var farBottomRight = _camera.ViewportToWorldPoint(new Vector3(1f, 0f, _camera.farClipPlane));
			var farTopLeft = _camera.ViewportToWorldPoint(new Vector3(0f, 1f, _camera.farClipPlane));
			var farTopRight = _camera.ViewportToWorldPoint(new Vector3(1f, 1f, _camera.farClipPlane));

			_box.SetMinMax(
				new Vector3(
					Mathf.Min(nearBottomLeft.x, nearBottomRight.x, nearTopLeft.x, nearTopRight.x, farBottomLeft.x, farBottomRight.x, farTopLeft.x, farTopRight.x),
					Mathf.Min(nearBottomLeft.y, nearBottomRight.y, nearTopLeft.y, nearTopRight.y, farBottomLeft.y, farBottomRight.y, farTopLeft.y, farTopRight.y),
					Mathf.Min(nearBottomLeft.z, nearBottomRight.z, nearTopLeft.z, nearTopRight.z, farBottomLeft.z, farBottomRight.z, farTopLeft.z, farTopRight.z)),
				new Vector3(
					Mathf.Max(nearBottomLeft.x, nearBottomRight.x, nearTopLeft.x, nearTopRight.x, farBottomLeft.x, farBottomRight.x, farTopLeft.x, farTopRight.x),
					Mathf.Max(nearBottomLeft.y, nearBottomRight.y, nearTopLeft.y, nearTopRight.y, farBottomLeft.y, farBottomRight.y, farTopLeft.y, farTopRight.y),
					Mathf.Max(nearBottomLeft.z, nearBottomRight.z, nearTopLeft.z, nearTopRight.z, farBottomLeft.z, farBottomRight.z, farTopLeft.z, farTopRight.z)));
		}
	}
}
