/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using Experilous.Numerics;

namespace Experilous.MakeItWrap
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
		/// Updates the specified translation transformations according to this region's particular combination of the world's wrapping transformations.
		/// </summary>
		/// <param name="position">The translation transformation to be transformed.</param>
		public abstract void Transform(ref Vector3 position);

		/// <summary>
		/// Updates the specified translation and rotation transformations according to this region's particular combination of the world's wrapping transformations.
		/// </summary>
		/// <param name="position">The translation transformation to be transformed.</param>
		/// <param name="rotation">The rotation transformation to be transformed.</param>
		public abstract void Transform(ref Vector3 position, ref Quaternion rotation);

		/// <summary>
		/// Updates the specified translation, rotation, and scaling transformations according to this region's particular combination of the world's wrapping transformations.
		/// </summary>
		/// <param name="position">The translation transformation to be transformed.</param>
		/// <param name="rotation">The rotation transformation to be transformed.</param>
		/// <param name="scale">The scaling transformation to be transformed.</param>
		public abstract void Transform(ref Vector3 position, ref Quaternion rotation, ref Vector3 scale);

		/// <summary>
		/// Updates the specified transformation matrix according to this region's particular combination of the world's wrapping transformations.
		/// </summary>
		/// <param name="transformation">The transformation matrix to be transformed.</param>
		public abstract void Transform(ref Matrix4x4 transformation);

		/// <summary>
		/// Transforms the specified position according to this region's particular combination of the world's wrapping transformations.
		/// </summary>
		/// <param name="position">The position to be transformed.</param>
		/// <returns>The transformed position.</returns>
		public abstract Vector3 Transform(Vector3 position);

		/// <summary>
		/// Transforms the specified axis aligned bounding box according to this region's particular combination of the world's wrapping transformations.
		/// </summary>
		/// <param name="axisAlignedBox">The axis aligned bounding box to be transformed.</param>
		/// <returns>The transformed axis aligned bounding box.</returns>
		public abstract Bounds Transform(Bounds axisAlignedBox);

		/// <summary>
		/// Transforms the specified bounding sphere according to this region's particular combination of the world's wrapping transformations.
		/// </summary>
		/// <param name="sphere">The bounding sphere to be transformed.</param>
		/// <returns>The transformed bounding sphere.</returns>
		public abstract Sphere Transform(Sphere sphere);

		/// <summary>
		/// Sets the target transform to the source transform after applying this region's particular combination of the world's wrapping transformations.
		/// </summary>
		/// <param name="sourceTransform">The transform which provides the original translation and rotation values.</param>
		/// <param name="targetTransform">The transform which receives the modified translation and rotation values.  This is allowed to be the same as the <paramref name="sourceTransform" /> parameter.</param>
		public abstract void Transform(Transform sourceTransform, Transform targetTransform);

		/// <summary>
		/// Sets the target rigidbody transform to the source rigidbody transform after applying this region's particular combination of the world's wrapping transformations.
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
		/// Sets the target rigidbody transform to the source rigidbody transform after applying this region's particular combination of the world's wrapping transformations.
		/// </summary>
		/// <param name="sourceRigidbody">The rigidbody which provides the original translation and rotation values.</param>
		/// <param name="targetRigidbody">The rigidbody which receives the modified translation and rotation values.  This is allowed to be the same as the <paramref name="sourceRigidBody" /> parameter.</param>
		/// <remarks>
		/// This overload is provided for performance purposes, because setting a rigidbody's
		/// position and rotation is faster than setting the transform's, as the latter will cause
		/// all attached colliders to recalculate their positions relative to the rigidbody.
		/// </remarks>
		public abstract void Transform(Rigidbody2D sourceRigidbody, Rigidbody2D targetRigidbody);

		/// <summary>
		/// The transformation matrix which can be used to apply this region's particular
		/// combination of the world's wrapping transformations.
		/// </summary>
		public abstract Matrix4x4 transformation { get; }
	}
}
