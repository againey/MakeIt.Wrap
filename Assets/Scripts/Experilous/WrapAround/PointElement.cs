using System;
using UnityEngine;

namespace Experilous.WrapAround
{
	public class PointElement : Element
	{
		public override bool IsVisible(Vector3 position, Quaternion rotation)
		{
			return World.Viewport.IsVisible(position);
		}
	}
}
