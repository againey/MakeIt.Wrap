﻿using System;
using UnityEngine;

namespace Experilous.WrapAround
{
	public class PointElement : Element
	{
		public override bool IsVisible(Viewport viewport, Vector3 position, Quaternion rotation)
		{
			return viewport.IsVisible(position);
		}
	}
}
