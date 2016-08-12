/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using Experilous.MakeIt.Utilities;

namespace Experilous.MakeIt.Wrap
{
	public abstract class RhomboidWorldBase : World
	{
		public abstract Vector3 untransformedOrigin { get; }
		public abstract Vector3 untransformedAxis0Vector { get; }
		public abstract Vector3 untransformedAxis1Vector { get; }
		public abstract Vector3 untransformedAxis2Vector { get; }
		public abstract bool axis0IsWrapped { get; }
		public abstract bool axis1IsWrapped { get; }
		public abstract bool axis2IsWrapped { get; }

		private Vector3 _transformedAxis0Vector;
		private Vector3 _transformedAxis1Vector;
		private Vector3 _transformedAxis2Vector;
		private Plane _axis0NegativePlane;
		private Plane _axis0PositivePlane;
		private Plane _axis1NegativePlane;
		private Plane _axis1PositivePlane;
		private Plane _axis2NegativePlane;
		private Plane _axis2PositivePlane;

		[Tooltip("The maximum bounding sphere radius of all possible visible elements that might occur within this wrap-around world.")]
		public float maxVisibleObjectRadius;

		[Tooltip("The maximum bounding sphere radius of all possible physically active elements that might occur within this wrap-around world.")]
		public float maxPhysicsObjectRadius;

		private List<GhostRegion> _physicsGhostRegions;

		protected void Start()
		{
			var p0 = transform.TransformPoint(untransformedOrigin);
			var pX = transform.TransformPoint(untransformedOrigin + untransformedAxis0Vector);
			var pY = transform.TransformPoint(untransformedOrigin + untransformedAxis1Vector);
			var pZ = transform.TransformPoint(untransformedOrigin + untransformedAxis2Vector);
			var pXYZ = transform.TransformPoint(untransformedOrigin + untransformedAxis0Vector + untransformedAxis1Vector + untransformedAxis2Vector);

			_transformedAxis0Vector = pX - p0;
			_transformedAxis1Vector = pY - p0;
			_transformedAxis2Vector = pZ - p0;

			var normalYZ = Vector3.Cross(_transformedAxis1Vector, _transformedAxis2Vector).normalized;
			var normalXZ = Vector3.Cross(_transformedAxis2Vector, _transformedAxis0Vector).normalized;
			var normalXY = Vector3.Cross(_transformedAxis0Vector, _transformedAxis1Vector).normalized;

			if (Vector3.Dot(normalYZ, _transformedAxis0Vector) < 0f) normalYZ = -normalYZ;
			if (Vector3.Dot(normalXZ, _transformedAxis1Vector) < 0f) normalXZ = -normalXZ;
			if (Vector3.Dot(normalXY, _transformedAxis2Vector) < 0f) normalXY = -normalXY;

			_axis0NegativePlane.SetNormalAndPosition(normalYZ, p0);
			_axis0PositivePlane.SetNormalAndPosition(-normalYZ, pXYZ);
			_axis1NegativePlane.SetNormalAndPosition(normalXZ, p0);
			_axis1PositivePlane.SetNormalAndPosition(-normalXZ, pXYZ);
			_axis2NegativePlane.SetNormalAndPosition(normalXY, p0);
			_axis2PositivePlane.SetNormalAndPosition(-normalXY, pXYZ);

			_physicsGhostRegions = new List<GhostRegion>();
			GetPhysicsGhostRegions(_physicsGhostRegions);
		}

		public override void GetPhysicsGhostRegions(List<GhostRegion> ghostRegions)
		{
			GetGhostRegions(maxPhysicsObjectRadius * 2f, ghostRegions);
		}

		public override void GetVisibleGhostRegions(Camera camera, List<GhostRegion> ghostRegions)
		{
			GetGhostRegions(camera, maxVisibleObjectRadius, ghostRegions);
		}

		protected void GetGhostRegions(float bufferRadius, List<GhostRegion> ghostRegions)
		{
			if (bufferRadius <= 0f) return;

			var axis0NormalOffset = Vector3.Dot(_transformedAxis0Vector, _axis0NegativePlane.normal);
			var axis1NormalOffset = Vector3.Dot(_transformedAxis1Vector, _axis1NegativePlane.normal);
			var axis2NormalOffset = Vector3.Dot(_transformedAxis2Vector, _axis2NegativePlane.normal);
			var axis0RegionBufferCount = axis0IsWrapped ? Mathf.CeilToInt(bufferRadius / axis0NormalOffset) : 0;
			var axis1RegionBufferCount = axis1IsWrapped ? Mathf.CeilToInt(bufferRadius / axis1NormalOffset) : 0;
			var axis2RegionBufferCount = axis2IsWrapped ? Mathf.CeilToInt(bufferRadius / axis2NormalOffset) : 0;

			int startIndex = 0;
			for (int z = -axis2RegionBufferCount; z <= axis2RegionBufferCount; ++z)
			{
				for (int y = -axis1RegionBufferCount; y <= axis1RegionBufferCount; ++y)
				{
					for (int x = -axis0RegionBufferCount; x <= axis0RegionBufferCount; ++x)
					{
						if (x == 0 && y == 0 && z == 0) continue; // The 0,0 region is the real region, not a ghost region, so skip it.
						InsertGhostRegion(new IntVector3(x, y, z), ghostRegions, ref startIndex);
					}
				}
			}

			ghostRegions.RemoveRange(startIndex, ghostRegions.Count - startIndex);
		}

		protected void GetGhostRegions(Camera camera, float bufferRadius, List<GhostRegion> ghostRegions)
		{
			// Get the six planes of the frustum volume, and expand them outward based on the buffer radius.
			var frustumPlanes = UnityEngine.GeometryUtility.CalculateFrustumPlanes(camera);
			if (bufferRadius != 0f)
			{
				for (int i = 0; i < frustumPlanes.Length; ++i)
				{
					frustumPlanes[i] = new Plane(frustumPlanes[i].normal, frustumPlanes[i].distance + bufferRadius);
				}
			}

			// Find the eight corners of the camera view frustum.
			var corners = MIGeometry.FindFrustumCorners(camera, frustumPlanes);

			// Find the maximum ghost region index extents based on these eight corners.
			IntVector3 min = new IntVector3(int.MaxValue, int.MaxValue, int.MaxValue);
			IntVector3 max = new IntVector3(int.MinValue, int.MinValue, int.MinValue);

			for (int i = 0; i < corners.Length; ++i)
			{
				ExpandIndexBounds(GetGhostRegionIndex(corners[i]), ref min, ref max);
			}

			// If the bounds are limited to a single dimension, then just return a linear array of ghost regions.
			if (min.y == max.y && min.z == max.z)
			{
				int startIndex = 0;
				for (int x = min.x; x <= max.x; ++x)
				{
					if (x == 0 && min.y == 0 && min.z == 0) continue; // The 0,0,0 region is the real region, not a ghost region, so skip it.
					InsertGhostRegion(new IntVector3(x, min.y, min.z), ghostRegions, ref startIndex);
				}
				return;
			}
			else if (min.x == max.x && min.z == max.z)
			{
				int startIndex = 0;
				for (int y = min.y; y <= max.y; ++y)
				{
					if (min.x == 0 && y == 0 && min.z == 0) continue; // The 0,0,0 region is the real region, not a ghost region, so skip it.
					InsertGhostRegion(new IntVector3(min.x, y, min.z), ghostRegions, ref startIndex);
				}
				return;
			}
			else if (min.x == max.x && min.y == max.y)
			{
				int startIndex = 0;
				for (int z = min.z; z <= max.z; ++z)
				{
					if (min.x == 0 && min.y == 0 && z == 0) continue; // The 0,0,0 region is the real region, not a ghost region, so skip it.
					InsertGhostRegion(new IntVector3(min.x, min.y, z), ghostRegions, ref startIndex);
				}
				return;
			}

			// If we haven't returned yet, then the bounds are not limited to a single dimension, which means that not all
			// regions in the determined bounds are guaranteed to actually intersect with the frustum, and there's no value
			// in returning any regions that don't intersect.  However, returning such regions shouldn't actually cause any
			// harm in terms of correctness, only performance, so there's some leeway for doing the rejection.

			int unwrappedAxis;
			if (!axis0IsWrapped) unwrappedAxis = 0;
			else if (!axis1IsWrapped) unwrappedAxis = 1;
			else if (!axis2IsWrapped) unwrappedAxis = 2;
			else unwrappedAxis = -1;

			var regionCorners = unwrappedAxis == -1 ? new Vector3[8] : null;
			var regionLines = unwrappedAxis != -1 ? new ScaledRay[4] : null;

			if (unwrappedAxis == -1)
			{
				int startIndex = 0;
				for (int z = min.z; z <= max.z; ++z)
				{
					for (int y = min.y; y <= max.y; ++y)
					{
						for (int x = min.x; x <= max.x; ++x)
						{
							if (x == 0 && y == 0 && z == 0) continue; // The 0,0,0 region is the real region, not a ghost region, so skip it.

							regionCorners[0] = _transformedAxis0Vector * x + _transformedAxis1Vector * y + _transformedAxis2Vector * z;
							regionCorners[1] = regionCorners[0] + _transformedAxis0Vector;
							regionCorners[2] = regionCorners[0] + _transformedAxis1Vector;
							regionCorners[3] = regionCorners[0] + _transformedAxis2Vector;
							regionCorners[4] = regionCorners[1] + _transformedAxis1Vector;
							regionCorners[5] = regionCorners[2] + _transformedAxis2Vector;
							regionCorners[6] = regionCorners[3] + _transformedAxis0Vector;
							regionCorners[7] = regionCorners[4] + _transformedAxis2Vector;

							bool exclude = false;
							foreach (var plane in frustumPlanes)
							{
								if (MIGeometry.AllAreBelow(regionCorners, plane))
								{
									exclude = true;
									break;
								}
							}

							if (!exclude)
							{
								InsertGhostRegion(new IntVector3(x, y, z), ghostRegions, ref startIndex);
							}
						}
					}
				}
			}
			else
			{
				int startIndex = 0;
				for (int z = min.z; z <= max.z; ++z)
				{
					for (int y = min.y; y <= max.y; ++y)
					{
						for (int x = min.x; x <= max.x; ++x)
						{
							if (x == 0 && y == 0 && z == 0) continue; // The 0,0,0 region is the real region, not a ghost region, so skip it.

							switch (unwrappedAxis)
							{
								case 0:
									regionLines[0] = new ScaledRay(_transformedAxis1Vector * y + _transformedAxis2Vector * z, _transformedAxis0Vector);
									regionLines[1] = new ScaledRay(regionLines[0].origin + _transformedAxis1Vector, _transformedAxis0Vector);
									regionLines[2] = new ScaledRay(regionLines[0].origin + _transformedAxis2Vector, _transformedAxis0Vector);
									regionLines[3] = new ScaledRay(regionLines[1].origin + _transformedAxis2Vector, _transformedAxis0Vector);
									break;
								case 1:
									regionLines[0] = new ScaledRay(_transformedAxis0Vector * x + _transformedAxis2Vector * z, _transformedAxis1Vector);
									regionLines[1] = new ScaledRay(regionLines[0].origin + _transformedAxis0Vector, _transformedAxis1Vector);
									regionLines[2] = new ScaledRay(regionLines[0].origin + _transformedAxis2Vector, _transformedAxis1Vector);
									regionLines[3] = new ScaledRay(regionLines[1].origin + _transformedAxis2Vector, _transformedAxis1Vector);
									break;
								case 2:
									regionLines[0] = new ScaledRay(_transformedAxis0Vector * x + _transformedAxis1Vector * y, _transformedAxis2Vector);
									regionLines[1] = new ScaledRay(regionLines[0].origin + _transformedAxis0Vector, _transformedAxis2Vector);
									regionLines[2] = new ScaledRay(regionLines[0].origin + _transformedAxis1Vector, _transformedAxis2Vector);
									regionLines[3] = new ScaledRay(regionLines[1].origin + _transformedAxis1Vector, _transformedAxis2Vector);
									break;
								default:
									throw new System.NotImplementedException();
							}

							bool exclude = true;
							foreach (var plane in frustumPlanes)
							{
								var minT = float.NegativeInfinity;
								var maxT = float.PositiveInfinity;
								if (MIGeometry.TruncateLineSegment(regionLines[0], plane, ref minT, ref maxT) <= 0f) continue;
								if (MIGeometry.TruncateLineSegment(regionLines[1], plane, ref minT, ref maxT) <= 0f) continue;
								if (MIGeometry.TruncateLineSegment(regionLines[2], plane, ref minT, ref maxT) <= 0f) continue;
								if (MIGeometry.TruncateLineSegment(regionLines[3], plane, ref minT, ref maxT) <= 0f) continue;

								exclude = false;
								break;
							}

							if (!exclude)
							{
								InsertGhostRegion(new IntVector3(x, y, z), ghostRegions, ref startIndex);
							}
						}
					}
				}
			}
		}

		private bool FindGhostRegion(IntVector3 regionIndex3D, List<GhostRegion> ghostRegions, out int index, int startIndex = 0)
		{
			index = startIndex;
			if (index >= ghostRegions.Count) return false;
			var currentRegion = (RhomboidGhostRegion)ghostRegions[index];
			if (regionIndex3D == currentRegion) return true;
			if (regionIndex3D < currentRegion) return false;

			var endIndex = ghostRegions.Count;

			while (startIndex < endIndex)
			{
				index = (startIndex + endIndex) / 2;
				currentRegion = (RhomboidGhostRegion)ghostRegions[index];
				if (regionIndex3D == currentRegion)
				{
					return true;
				}
				else if (regionIndex3D < currentRegion)
				{
					endIndex = index;
				}
				else
				{
					startIndex = index + 1;
				}
			}

			index = startIndex;
			return false;
		}

		private void InsertGhostRegion(IntVector3 regionIndex3D, List<GhostRegion> ghostRegions, ref int startIndex)
		{
			int insertionIndex;
			if (FindGhostRegion(regionIndex3D, ghostRegions, out insertionIndex, startIndex))
			{
				ghostRegions.RemoveRange(startIndex, insertionIndex - startIndex);
			}
			else if (insertionIndex == startIndex)
			{
				ghostRegions.Insert(insertionIndex,
					new RhomboidGhostRegion(
						regionIndex3D,
						_transformedAxis0Vector * regionIndex3D.x +
						_transformedAxis1Vector * regionIndex3D.y +
						_transformedAxis2Vector * regionIndex3D.z));
			}
			else
			{
				ghostRegions.RemoveRange(startIndex, insertionIndex - startIndex - 1);
				ghostRegions[startIndex] =
					new RhomboidGhostRegion(
						regionIndex3D,
						_transformedAxis0Vector * regionIndex3D.x +
						_transformedAxis1Vector * regionIndex3D.y +
						_transformedAxis2Vector * regionIndex3D.z);
			}
			++startIndex;
		}

		private IntVector3 GetGhostRegionIndex(Vector3 position)
		{
			position = transform.InverseTransformPoint(position);

			return new IntVector3(
				axis0IsWrapped ? Mathf.FloorToInt(-MIGeometry.GetIntersectionParameter(_axis0NegativePlane, new ScaledRay(position, _transformedAxis0Vector))) : 0,
				axis1IsWrapped ? Mathf.FloorToInt(-MIGeometry.GetIntersectionParameter(_axis1NegativePlane, new ScaledRay(position, _transformedAxis1Vector))) : 0,
				axis2IsWrapped ? Mathf.FloorToInt(-MIGeometry.GetIntersectionParameter(_axis2NegativePlane, new ScaledRay(position, _transformedAxis2Vector))) : 0);
		}

		private void ExpandIndexBounds(IntVector3 index, ref IntVector3 min, ref IntVector3 max)
		{
			min.x = Mathf.Min(min.x, index.x);
			min.y = Mathf.Min(min.y, index.y);
			min.z = Mathf.Min(min.z, index.z);
			max.x = Mathf.Max(max.x, index.x);
			max.y = Mathf.Max(max.y, index.y);
			max.z = Mathf.Max(max.z, index.z);
		}

		public override IEnumerable<GhostRegion> physicsGhostRegions { get { return _physicsGhostRegions; } }

		#region IsCollidable

		public override bool IsCollidable(Vector3 position)
		{
			return
				(!axis0IsWrapped || (
					_axis0NegativePlane.GetDistanceToPoint(position) >= -maxPhysicsObjectRadius &&
					_axis0PositivePlane.GetDistanceToPoint(position) >= -maxPhysicsObjectRadius)) &&
				(!axis1IsWrapped || (
					_axis1NegativePlane.GetDistanceToPoint(position) >= -maxPhysicsObjectRadius &&
					_axis1PositivePlane.GetDistanceToPoint(position) >= -maxPhysicsObjectRadius)) &&
				(!axis2IsWrapped || (
					_axis2NegativePlane.GetDistanceToPoint(position) >= -maxPhysicsObjectRadius &&
					_axis2PositivePlane.GetDistanceToPoint(position) >= -maxPhysicsObjectRadius));
		}

		public override bool IsCollidable(Sphere sphere)
		{
			var position = sphere.center;
			var extendedRadius = sphere.radius + maxPhysicsObjectRadius;
			return
				(!axis0IsWrapped || (
					_axis0NegativePlane.GetDistanceToPoint(position) >= -extendedRadius &&
					_axis0PositivePlane.GetDistanceToPoint(position) >= -extendedRadius)) &&
				(!axis1IsWrapped || (
					_axis1NegativePlane.GetDistanceToPoint(position) >= -extendedRadius &&
					_axis1PositivePlane.GetDistanceToPoint(position) >= -extendedRadius)) &&
				(!axis2IsWrapped || (
					_axis2NegativePlane.GetDistanceToPoint(position) >= -extendedRadius &&
					_axis2PositivePlane.GetDistanceToPoint(position) >= -extendedRadius));
		}

		public override bool IsCollidable(Bounds box)
		{
			return
				(!axis0IsWrapped || (
					box.IsAboveOrIntersects(_axis0NegativePlane.Shift(-maxPhysicsObjectRadius)) &&
					box.IsAboveOrIntersects(_axis0PositivePlane.Shift(-maxPhysicsObjectRadius)))) &&
				(!axis1IsWrapped || (
					box.IsAboveOrIntersects(_axis1NegativePlane.Shift(-maxPhysicsObjectRadius)) &&
					box.IsAboveOrIntersects(_axis1PositivePlane.Shift(-maxPhysicsObjectRadius)))) &&
				(!axis2IsWrapped || (
					box.IsAboveOrIntersects(_axis2NegativePlane.Shift(-maxPhysicsObjectRadius)) &&
					box.IsAboveOrIntersects(_axis2PositivePlane.Shift(-maxPhysicsObjectRadius))));
		}

		#endregion

		#region Intersects

		public override bool Intersects(Vector3 position, float buffer = 0f)
		{
			return
				(!axis0IsWrapped || (
					_axis0NegativePlane.GetDistanceToPoint(position) >= -buffer &&
					_axis0PositivePlane.GetDistanceToPoint(position) >= -buffer)) &&
				(!axis1IsWrapped || (
					_axis1NegativePlane.GetDistanceToPoint(position) >= -buffer &&
					_axis1PositivePlane.GetDistanceToPoint(position) >= -buffer)) &&
				(!axis2IsWrapped || (
					_axis2NegativePlane.GetDistanceToPoint(position) >= -buffer &&
					_axis2PositivePlane.GetDistanceToPoint(position) >= -buffer));
		}

		public override bool Intersects(Bounds box, float buffer = 0f)
		{
			return
				(!axis0IsWrapped || (
					!box.IsBelow(_axis0NegativePlane.Shift(-buffer)) &&
					!box.IsBelow(_axis0PositivePlane.Shift(-buffer)))) &&
				(!axis1IsWrapped || (
					!box.IsBelow(_axis1NegativePlane.Shift(-buffer)) &&
					!box.IsBelow(_axis1PositivePlane.Shift(-buffer)))) &&
				(!axis2IsWrapped || (
					!box.IsBelow(_axis2NegativePlane.Shift(-buffer)) &&
					!box.IsBelow(_axis2PositivePlane.Shift(-buffer))));
		}

		public override bool Intersects(Sphere sphere, float buffer = 0f)
		{
			return Intersects(sphere.center, sphere.radius + buffer);
		}

		#endregion

		#region Contains

		public override bool Contains(Vector3 position, float buffer = 0f)
		{
			return
				(!axis0IsWrapped || (
					_axis0NegativePlane.GetDistanceToPoint(position) >= buffer &&
					_axis0PositivePlane.GetDistanceToPoint(position) >= buffer)) &&
				(!axis1IsWrapped || (
					_axis1NegativePlane.GetDistanceToPoint(position) >= buffer &&
					_axis1PositivePlane.GetDistanceToPoint(position) >= buffer)) &&
				(!axis2IsWrapped || (
					_axis2NegativePlane.GetDistanceToPoint(position) >= buffer &&
					_axis2PositivePlane.GetDistanceToPoint(position) >= buffer));
		}

		public override bool Contains(Bounds box, float buffer = 0f)
		{
			return
				(!axis0IsWrapped || (
					box.IsAboveOrTouches(_axis0NegativePlane.Shift(buffer)) &&
					box.IsAboveOrTouches(_axis0PositivePlane.Shift(buffer)))) &&
				(!axis1IsWrapped || (
					box.IsAboveOrTouches(_axis1NegativePlane.Shift(buffer)) &&
					box.IsAboveOrTouches(_axis1PositivePlane.Shift(buffer)))) &&
				(!axis2IsWrapped || (
					box.IsAboveOrTouches(_axis2NegativePlane.Shift(buffer)) &&
					box.IsAboveOrTouches(_axis2PositivePlane.Shift(buffer))));
		}

		public override bool Contains(Sphere sphere, float buffer = 0f)
		{
			return Contains(sphere.center, sphere.radius + buffer);
		}

		#endregion

		#region Confine

		public override void Confine(Transform transform)
		{
			var position = transform.position;
			Confine(ref position);
			transform.position = position;
		}

		public override void Confine(Rigidbody rigidbody)
		{
			var position = rigidbody.position;
			Confine(ref position);
			rigidbody.position = position;
		}

		public override void Confine(Rigidbody2D rigidbody)
		{
			var position = rigidbody.position;
			Confine(ref position);
			rigidbody.position = position;
		}

		public void Confine(ref Vector3 position)
		{
			if (axis0IsWrapped) position -= Mathf.Floor(-MIGeometry.GetIntersectionParameter(_axis0NegativePlane, new ScaledRay(position, _transformedAxis0Vector))) * _transformedAxis0Vector;
			if (axis1IsWrapped) position -= Mathf.Floor(-MIGeometry.GetIntersectionParameter(_axis1NegativePlane, new ScaledRay(position, _transformedAxis1Vector))) * _transformedAxis1Vector;
			if (axis2IsWrapped) position -= Mathf.Floor(-MIGeometry.GetIntersectionParameter(_axis2NegativePlane, new ScaledRay(position, _transformedAxis2Vector))) * _transformedAxis2Vector;
		}

		public void Confine(ref Vector2 position)
		{
			if (axis0IsWrapped) position -= (Vector2)(Mathf.Floor(-MIGeometry.GetIntersectionParameter(_axis0NegativePlane, new ScaledRay(position, _transformedAxis0Vector))) * _transformedAxis0Vector);
			if (axis1IsWrapped) position -= (Vector2)(Mathf.Floor(-MIGeometry.GetIntersectionParameter(_axis1NegativePlane, new ScaledRay(position, _transformedAxis1Vector))) * _transformedAxis1Vector);
		}

		#endregion
	}
}
