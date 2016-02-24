/******************************************************************************\
 *  Copyright (C) 2016 Experilous <againey@experilous.com>
 *  
 *  This file is subject to the terms and conditions defined in the file
 *  'Assets/Plugins/Experilous/License.txt', which is a part of this package.
 *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	/// <summary>
	/// An abstract representation of a volume of space that is a clone of a world with wrap-around
	/// behavior, but transformed to a different location and orientation according to a particular
	/// combination of the world's possible wrapping transformations.
	/// </summary>
	public abstract class GhostRegion
	{
		/// <summary>
		/// A property that indicates if the ghost region is at all active.  When it is inactive,
		/// there is no need to model the behavior of elements with this ghost region's particular
		/// combination of the world's wrapping transformations, as it is known that they would
		/// have no effect.  When the region is active, however, it is at least possible that some
		/// of the elements would need to have their behavior modeled within this region.
		/// </summary>
		public abstract bool isActive { get; set; }

		/// <summary>
		/// Transforms a provided position and rotation according to this region's particular
		/// combination of the world's wrapping transformations.
		/// </summary>
		/// <param name="position">The position to be transformed through translation.</param>
		/// <param name="rotation">The orientation that is to be transformed through rotation.</param>
		public abstract void Transform(ref Vector3 position, ref Quaternion rotation);

		/// <summary>
		/// Applies the translation and rotation values from a source transform to a target
		/// transform, after adjusting the source's translation and rotation according to
		/// this region's particular combination of the world's wrapping transformations.
		/// </summary>
		/// <param name="sourceTransform">The transform which provides the original translation and rotation values.</param>
		/// <param name="targetTransform">The transform which receives the modified translation and rotation values.  This is allowed to be the same as the <paramref name="sourceTransform" /> parameter.</param>
		public abstract void Transform(Transform sourceTransform, Transform targetTransform);

		/// <summary>
		/// Applies the translation and rotation values from a source rigidbody to a target
		/// rigidbody, after adjusting the source's translation and rotation according to
		/// this region's particular combination of the world's wrapping transformations.
		/// </summary>
		/// <param name="sourceRigidbody">The rigidbody which provides the original translation and rotation values.</param>
		/// <param name="targetRigidbody">The rigidbody which receives the modified translation and rotation values.  This is allowed to be the same as the <paramref name="sourceRigidBody" /> parameter.</param>
		/// <remarks>
		/// This overload is provided for performance purposes, because setting a rigidbody's
		/// position and rotation is faster than setting the transform's, as the latter will cause
		/// all attached colliders to recalculate their positions relative to the rigidbody.
		/// </remarks>
		public abstract void Transform(Rigidbody sourceRigidbody, Rigidbody targetRigidbody);

		/// <summary>
		/// The transformation matrix which can be used to apply this region's particular
		/// combination of the world's wrapping transformations.
		/// </summary>
		public abstract Matrix4x4 transformation { get; }
	}
}
