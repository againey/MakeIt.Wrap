using System;
using System.Collections.Generic;
using UnityEngine;

namespace Experilous.WrapAround
{
	public class AxisAlignedWrapX2DGhostRegion : GhostRegion
	{
		private readonly HashSet<Element> _ghostedElements = new HashSet<Element>();
		private float _xOffset;
		private Viewport _viewport;

		public AxisAlignedWrapX2DGhostRegion(float xOffset, Viewport viewport)
		{
			_xOffset = xOffset;
			_viewport = viewport;
		}

		public override Viewport viewport { get { return _viewport; } }

		public override bool HasGhost(Element element)
		{
			return _ghostedElements.Contains(element);
		}

		public override void AddElement(Element element)
		{
			_ghostedElements.Add(element);
		}

		public override void RemoveElement(Element element)
		{
			_ghostedElements.Remove(element);
		}

		public override void Transform(ref Vector3 position, ref Quaternion rotation)
		{
			position.x += _xOffset;
		}

		public override Matrix4x4 transformation { get { return Matrix4x4.TRS(new Vector3(_xOffset, 0f, 0f), Quaternion.identity, new Vector3(1f, 1f, 1f)); } }
	}
}
