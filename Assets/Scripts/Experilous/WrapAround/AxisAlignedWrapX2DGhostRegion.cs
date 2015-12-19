using System;
using System.Collections.Generic;
using UnityEngine;

namespace Experilous.WrapAround
{
	public class AxisAlignedWrapX2DGhostRegion : GhostRegion
	{
		private readonly HashSet<int> _ghostedElements = new HashSet<int>();
		private float _xOffset;
		private Viewport _viewport;
		private bool _isActive;

		public AxisAlignedWrapX2DGhostRegion(float xOffset, Viewport viewport)
		{
			_xOffset = xOffset;
			_viewport = viewport;
		}

		public override Viewport viewport { get { return _viewport; } }
		public override bool isActive { get { return _isActive; } set { _isActive = value; } }

		public override bool HasGhost(Element element)
		{
			return HasGhost(element.GetInstanceID());
		}

		public override bool HasGhost(int instanceId)
		{
			return _ghostedElements.Contains(instanceId);
		}

		public override void AddElement(Element element)
		{
			AddElement(element.GetInstanceID());
		}

		public override void AddElement(int instanceId)
		{
			_ghostedElements.Add(instanceId);
		}

		public override void RemoveElement(Element element)
		{
			RemoveElement(element.GetInstanceID());
		}

		public override void RemoveElement(int instanceId)
		{
			_ghostedElements.Remove(instanceId);
		}

		public override void Transform(ref Vector3 position, ref Quaternion rotation)
		{
			position.x += _xOffset;
		}

		public override void Transform(Transform sourceTransform, Transform targetTransform)
		{
			targetTransform.position = sourceTransform.position + new Vector3(_xOffset, 0f, 0f);
			targetTransform.rotation = sourceTransform.rotation;
		}

		public override void Transform(Rigidbody sourceRigidbody, Rigidbody targetRigidbody)
		{
			targetRigidbody.position = sourceRigidbody.position + new Vector3(_xOffset, 0f, 0f);
			targetRigidbody.rotation = sourceRigidbody.rotation;
		}

		public override Matrix4x4 transformation { get { return Matrix4x4.TRS(new Vector3(_xOffset, 0f, 0f), Quaternion.identity, new Vector3(1f, 1f, 1f)); } }
	}
}
