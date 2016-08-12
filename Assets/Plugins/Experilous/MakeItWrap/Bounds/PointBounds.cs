/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	public abstract class PointBounds : ElementBounds
	{
		public Vector3 position;

		public PointBounds(Vector3 position)
		{
			this.position = position;
		}

		public static PointBounds Create(Vector3 position, bool fixedScale, bool fixedRotation)
		{
			if (fixedScale)
			{
				if (fixedRotation)
				{
					return new FixedPointBounds(position);
				}
				else
				{
					return new RotatablePointBounds(position);
				}
			}
			else
			{
				if (fixedRotation)
				{
					return new ScalablePointBounds(position);
				}
				else
				{
					return new DynamicPointBounds(position);
				}
			}
		}

		public static PointBounds Create(Vector3 position, Transform transform, bool fixedScale, bool fixedRotation)
		{
			if (fixedScale)
			{
				if (fixedRotation)
				{
					return new FixedPointBounds(position - transform.position);
				}
				else
				{
					return new RotatablePointBounds(transform.InverseTransformDirection(position - transform.position));
				}
			}
			else
			{
				var scale = transform.lossyScale;
				if (fixedRotation)
				{
					return new ScalablePointBounds((position - transform.position).DivideComponents(scale));
				}
				else
				{
					return new DynamicPointBounds(transform.InverseTransformVector(position - transform.position));
				}
			}
		}
	}
}
