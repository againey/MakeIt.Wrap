﻿/******************************************************************************\
* Copyright Andy Gainey                                                        *
*                                                                              *
* Licensed under the Apache License, Version 2.0 (the "License");              *
* you may not use this file except in compliance with the License.             *
* You may obtain a copy of the License at                                      *
*                                                                              *
*     http://www.apache.org/licenses/LICENSE-2.0                               *
*                                                                              *
* Unless required by applicable law or agreed to in writing, software          *
* distributed under the License is distributed on an "AS IS" BASIS,            *
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.     *
* See the License for the specific language governing permissions and          *
* limitations under the License.                                               *
\******************************************************************************/

using UnityEngine;
using System;
using MakeIt.Numerics;

namespace MakeIt.Wrap
{
	/// <summary>
	/// Abstract representation of a bounding volume that can perform the first phase of
	/// a double-dispatch visibility or collision check with a viewport or world.
	/// </summary>
	/// <seealso cref="Viewport"/>
	/// <seealso cref="World"/>
	[Serializable]
	public abstract class ElementBounds
	{
		public abstract bool IsVisible(Viewport viewport, Transform transform);
		public abstract bool IsVisible(Viewport viewport, Transform transform, GhostRegion ghostRegion);

		public abstract bool IsCollidable(World world, Transform transform);
		public abstract bool IsCollidable(World world, Transform transform, GhostRegion ghostRegion);

		public abstract bool Intersects(World world, Transform transform, float buffer = 0f);
		public abstract bool Intersects(World world, Transform transform, GhostRegion ghostRegion, float buffer = 0f);

		public abstract bool ContainedBy(World world, Transform transform, float buffer = 0f);
		public abstract bool ContainedBy(World world, Transform transform, GhostRegion ghostRegion, float buffer = 0f);

		public static ElementBounds CreateBounds(ElementBoundsSource boundsSource, ElementBoundsProvider boundsProvider, Transform transform, Func<Bounds> automaticAxisAlignedBox, Func<Sphere> automaticSphere)
		{
			var fixedScale = (boundsSource & ElementBoundsSource.FixedScale) != 0;
			var fixedRotation = (boundsSource & ElementBoundsSource.FixedRotation) != 0;

			switch (boundsSource & ElementBoundsSource.Source)
			{
				case ElementBoundsSource.None:
					return null;

				case ElementBoundsSource.LocalOrigin:
					return new LocalOriginBounds();

				case ElementBoundsSource.Automatic:
					if (fixedRotation) goto case ElementBoundsSource.AutomaticAxisAlignedBox;
					goto case ElementBoundsSource.AutomaticSphere;

				case ElementBoundsSource.AutomaticAxisAlignedBox:
					return AxisAlignedBoxBounds.Create(automaticAxisAlignedBox(), transform, fixedScale);

				case ElementBoundsSource.AutomaticSphere:
					return SphereBounds.Create(automaticSphere(), transform, fixedScale, fixedRotation);

				case ElementBoundsSource.Manual:
					return (boundsProvider != null) ? boundsProvider.CreateBounds(fixedScale, fixedRotation) : null;

				default:
					throw new NotImplementedException();
			}
		}

#if UNITY_EDITOR
		public virtual void DrawGizmosSelected(Transform transform, Color color) { }
		public virtual void DrawGizmosSelected(Transform transform, GhostRegion ghostRegion, Color color) { }
#endif
	}

	[Flags]
	public enum ElementBoundsSource
	{
		FixedScale = 0x0001,
		FixedRotation = 0x0002,
		FixedFlags = FixedScale | FixedRotation,

		None = 0x0000,
		LocalOrigin = 0x0010,
		Automatic = 0x0020,
		AutomaticAxisAlignedBox = 0x0030,
		AutomaticSphere = 0x0040,
		Manual = 0x0050,
		Source = LocalOrigin | Automatic | AutomaticAxisAlignedBox | AutomaticSphere | Manual,
	}

	public abstract class ElementBoundsProvider : MonoBehaviour
	{
		public abstract ElementBounds CreateBounds(bool fixedScale, bool fixedRotation);
	}
}
