using System;
using System.Collections.Generic;
using UnityEngine;

namespace Experilous.WrapAround
{
	public class AxisAlignedWrapXY2DGhostRegion : GhostRegion
	{
		private readonly HashSet<int> _ghostedElements = new HashSet<int>();
		private float _xOffset;
		private float _yOffset;
		private bool _isActive = true;

		public AxisAlignedWrapXY2DGhostRegion(float xOffset, float yOffset)
		{
			_xOffset = xOffset;
			_yOffset = yOffset;
		}

		public override bool isActive { get { return _isActive; } set { _isActive = value; } }

		public override bool HasGhost(int instanceId)
		{
			return _ghostedElements.Contains(instanceId);
		}

		public override void AddElement(int instanceId)
		{
			_ghostedElements.Add(instanceId);
		}

		public override void RemoveElement(int instanceId)
		{
			_ghostedElements.Remove(instanceId);
		}

		public override void Transform(ref Vector3 position, ref Quaternion rotation)
		{
			position.x += _xOffset;
			position.y += _yOffset;
		}

		public override void Transform(Transform sourceTransform, Transform targetTransform)
		{
			targetTransform.position = sourceTransform.position + new Vector3(_xOffset, _yOffset, 0f);
			targetTransform.rotation = sourceTransform.rotation;
		}

		public override void Transform(Rigidbody sourceRigidbody, Rigidbody targetRigidbody)
		{
			targetRigidbody.position = sourceRigidbody.position + new Vector3(_xOffset, _yOffset, 0f);
			targetRigidbody.rotation = sourceRigidbody.rotation;
		}

		public override Matrix4x4 transformation { get { return Matrix4x4.TRS(new Vector3(_xOffset, _yOffset, 0f), Quaternion.identity, new Vector3(1f, 1f, 1f)); } }
	}
}
