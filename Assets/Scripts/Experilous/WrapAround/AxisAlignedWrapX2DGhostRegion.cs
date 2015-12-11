using System;
using System.Collections.Generic;
using UnityEngine;

namespace Experilous.WrapAround
{
	public class AxisAlignedWrapX2DGhostRegion : GhostRegion
	{
		private readonly HashSet<Element> _ghostedElements = new HashSet<Element>();
		private float _xOffset;

		public AxisAlignedWrapX2DGhostRegion(float xOffset)
		{
			_xOffset = xOffset;
		}

		public override bool HasGhost(Element element)
		{
			return _ghostedElements.Contains(element);
		}

		public override void AddElement(Element element)
		{
			_ghostedElements.Add(element);
		}

		public override void Transform(ref Vector3 position, ref Quaternion rotation)
		{
			position.x += _xOffset;
		}

		public override void DestroyGhost(ElementGhost ghost)
		{
			_ghostedElements.Remove(ghost.original);
			UnityEngine.Object.Destroy(ghost.gameObject);
		}
	}
}
