using System;
using UnityEngine;

namespace Experilous.WrapAround
{
	public class RigidBodyPointElement : PointElement
	{
		protected override void CreateGhost(GhostRegion region, Vector3 position, Quaternion rotation)
		{
			InstantiateGhost<RigidBodyElementGhost>(region, position, rotation);
		}
	}
}
