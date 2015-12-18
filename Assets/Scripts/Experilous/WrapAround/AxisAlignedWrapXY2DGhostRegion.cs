using System;
using System.Collections.Generic;
using UnityEngine;

namespace Experilous.WrapAround
{
	public class AxisAlignedWrapXY2DGhostRegion : GhostRegion
	{
		private readonly HashSet<Element> _ghostedElements = new HashSet<Element>();
		private float _xOffset;
		private float _yOffset;
		private Viewport _viewport;

		public AxisAlignedWrapXY2DGhostRegion(float xOffset, float yOffset, Viewport viewport)
		{
			_xOffset = xOffset;
			_yOffset = yOffset;
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
			position.y += _yOffset;
		}
	}
}
