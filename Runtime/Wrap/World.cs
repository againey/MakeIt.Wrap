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
using System.Collections.Generic;
using MakeIt.Numerics;

namespace MakeIt.Wrap
{
	/// <summary>
	/// An abstract representation of a world that has wrap-around behavior on one or more pairs of world edges.
	/// The world needs to know its physical dimensions and the exact nature of its wrapping edges, so that it
	/// can create ghost regions which represent transformed clones of the world, can check if a volume is within
	/// the bounds of the canonical world volume, and can restrict objects to the canonical world volume.
	/// </summary>
	/// <seealso cref="GhostRegion"/>
	/// <seealso cref="Viewport"/>
	/// <seealso cref="WorldProvider"/>
	/// <seealso cref="IWorldConsumer"/>
	public abstract class World : MonoBehaviour
	{
		/// <summary>
		/// Populates a list of ghost regions that are appropriate for accurately modeling the behavior of
		/// physics-enabled objects in a way that is consistent with the wrap-around behavior of the world.
		/// </summary>
		/// <param name="ghostRegions">The list of physically active ghost regions.  Any elements in this list when the call is made will be removed before the current ghost regions are added.</param>
		public abstract void GetPhysicsGhostRegions(List<GhostRegion> ghostRegions);

		/// <summary>
		/// Populates a list of ghost regions that are potentially visible given the shape of the provided camera's view frustum.
		/// </summary>
		/// <param name="camera">The camera that describes what areas of the world are current visible.</param>
		/// <param name="ghostRegions">The list of visible ghost regions.  Any elements in this list when the call is made will be removed before the current ghost regions are added.</param>
		public abstract void GetVisibleGhostRegions(Camera camera, List<GhostRegion> ghostRegions);

		/// <summary>
		/// Returns a cached list of physically active ghost regions.  This list is not expected to change
		/// as long as the world's shape, wrapping behavior, and assumptions about
		/// the maximum size of physically active objects remain the same.
		/// </summary>
		public abstract IEnumerable<GhostRegion> physicsGhostRegions { get; }

		#region IsCollidable

		// Various overloads for testing if an object is potentially collidable with other objects.
		// These various overloads represent the second phase of a double-dispatch scheme between
		// any world element type (or delegated bound type) and any world type.  The first phase
		// starts at the element, dependent on the information that it has regarding its position
		// and orientation in space.  The element passes this information on to the world, without
		// needing to know the world's type or how the world does the collision check.

		// It is expected and normal to add new overloads to this list if new element types are
		// added that have information that cannot be efficiently transformed into the parameters
		// provided by the already existing overloads.  However, every world type will then need
		// to be updated to understand and perform collision checks with these new overloads.

		/// <summary>
		/// Returns whether a physically active object at the specified position could possibly collide with
		/// another physically active object whose origin is strictly confined to the world's canonical bounds.
		/// </summary>
		/// <param name="position">The position of the physically active object.</param>
		/// <returns>Returns <c>true</c> if the physically active object could possibly collide with another object; <c>false</c> otherwise</returns>
		public abstract bool IsCollidable(Vector3 position);

		/// <summary>
		/// Returns whether a physically active object with the specified axis aligned bounding box could possibly collide
		/// with another physically active object whose origin is strictly confined to the world's canonical bounds.
		/// </summary>
		/// <param name="box">The axis aligned bounding box of the physically active object.</param>
		/// <returns>Returns <c>true</c> if the physically active object could possibly collide with another object; <c>false</c> otherwise</returns>
		public abstract bool IsCollidable(Bounds axisAlignedBox);

		/// <summary>
		/// Returns whether a physically active object with the specifed bounding sphere could possibly collide with
		/// another physically active object whose origin is strictly confined to the world's canonical bounds.
		/// </summary>
		/// <param name="position">The bounding sphere of the physically active object.</param>
		/// <returns>Returns <c>true</c> if the physically active object could possibly collide with another object; <c>false</c> otherwise</returns>
		public abstract bool IsCollidable(Sphere sphere);

		#endregion

		#region Intersects

		/// <summary>
		/// Returns whether the specified position is within <paramref name="buffer"/> units of the world's canonical bounds.
		/// </summary>
		/// <param name="position">The position to test against the world's bounds.</param>
		/// <param name="buffer">The buffer thickness beyond the world's bounds.</param>
		/// <returns>Returns <c>true</c> if <paramref name="position"/> is within <paramref name="buffer"/> units of the world's bounds; <c>false</c> otherwise</returns>
		public abstract bool Intersects(Vector3 position, float buffer = 0f);

		/// <summary>
		/// Returns whether any point within the specified axis aligned bounding box is within <paramref name="buffer"/> units of the world's canonical bounds.
		/// </summary>
		/// <param name="box">The axis aligned bounding box to test against the world's bounds.</param>
		/// <param name="buffer">The buffer thickness beyond the world's bounds.</param>
		/// <returns>Returns <c>true</c> if some point within <paramref name="box"/> is within <paramref name="buffer"/> units of the world's bounds; <c>false</c> otherwise</returns>
		public abstract bool Intersects(Bounds box, float buffer = 0f);

		/// <summary>
		/// Returns whether any point within the specified bounding sphere is within <paramref name="buffer"/> units of the world's canonical bounds.
		/// </summary>
		/// <param name="sphere">The bounding sphere to test against the world's bounds.</param>
		/// <param name="buffer">The buffer thickness beyond the world's bounds.</param>
		/// <returns>Returns <c>true</c> if some point within <paramref name="sphere"/> is within <paramref name="buffer"/> units of the world's bounds; <c>false</c> otherwise</returns>
		public abstract bool Intersects(Sphere sphere, float buffer = 0f);

		#endregion

		#region Contains

		/// <summary>
		/// Returns whether the specified position is within the world's canonical bounds, with <paramref name="buffer"/> units to spare.
		/// </summary>
		/// <param name="position">The position to test against the world's bounds.</param>
		/// <param name="buffer">The buffer thickness within the world's bounds.</param>
		/// <returns>Returns <c>true</c> if <paramref name="position"/> is within and no closer than <paramref name="buffer"/> units to the world's bounds; <c>false</c> otherwise</returns>
		public abstract bool Contains(Vector3 position, float buffer = 0f);

		/// <summary>
		/// Returns whether the specified axis aligned bounding box is fully within the world's canonical bounds, with <paramref name="buffer"/> units to spare.
		/// </summary>
		/// <param name="box">The axis aligned bounding box to test against the world's bounds.</param>
		/// <param name="buffer">The buffer thickness within the world's bounds.</param>
		/// <returns>Returns <c>true</c> if <paramref name="box"/> is fully within and no closer than <paramref name="buffer"/> units to the world's bounds; <c>false</c> otherwise</returns>
		public abstract bool Contains(Bounds box, float buffer = 0f);

		/// <summary>
		/// Returns whether the specified bounding sphere is fully within the world's canonical bounds, with <paramref name="buffer"/> units to spare.
		/// </summary>
		/// <param name="sphere">The bounding sphere to test against the world's bounds.</param>
		/// <param name="buffer">The buffer thickness within the world's bounds.</param>
		/// <returns>Returns <c>true</c> if <paramref name="sphere"/> is fully within and no closer than <paramref name="buffer"/> units to the world's bounds; <c>false</c> otherwise</returns>
		public abstract bool Contains(Sphere sphere, float buffer = 0f);

		#endregion

		/// <summary>
		/// Applies the necessary transformations to the supplied transform according to the relevant edge wrapping
		/// behavior to keep the transform confined to the canonical world bounds.
		/// </summary>
		/// <param name="transform">The transform that needs to be confined to the canonical world bounds.</param>
		public abstract void Confine(Transform transform);

		/// <summary>
		/// Applies the necessary transformations to the supplied rigidbody according to the relevant edge wrapping
		/// behavior to keep the rigidbody confined to the canonical world bounds.
		/// </summary>
		/// <param name="rigidbody">The rigidbody that needs to be confined to the canonical world bounds.</param>
		/// <remarks>
		/// This overload is provided for performance purposes, because setting a rigidbody's
		/// position and rotation is faster than setting the transform's, as the latter will cause
		/// all attached colliders to recalculate their positions relative to the rigidbody.
		/// </remarks>
		public abstract void Confine(Rigidbody rigidbody);

		/// <summary>
		/// Applies the necessary transformations to the supplied rigidbody according to the relevant edge wrapping
		/// behavior to keep the rigidbody confined to the canonical world bounds.
		/// </summary>
		/// <param name="rigidbody">The rigidbody that needs to be confined to the canonical world bounds.</param>
		/// <remarks>
		/// This overload is provided for performance purposes, because setting a rigidbody's
		/// position and rotation is faster than setting the transform's, as the latter will cause
		/// all attached colliders to recalculate their positions relative to the rigidbody.
		/// </remarks>
		public abstract void Confine(Rigidbody2D rigidbody);
	}
}
