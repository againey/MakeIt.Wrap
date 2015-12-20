using UnityEngine;

namespace Experilous.WrapAround
{
	public class SphereBoundedLightElement : LightElement
	{
		public float radius;

		public override bool IsVisible(LightElementGhost ghost)
		{
			return viewport.IsVisible(ghost.transform.position, radius);
		}

		public override bool IsVisible(Vector3 position, Quaternion rotation)
		{
			return viewport.IsVisible(position, radius);
		}

		protected new void Start()
		{
			base.Start();

			if (radius == 0f)
			{
				var light = GetComponent<Light>();
				if (light.type == LightType.Point)
				{
					radius = light.range;
				}
			}
		}
	}
}
