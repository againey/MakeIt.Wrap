﻿using UnityEngine;
using System.Collections.Generic;

namespace Experilous.WrapAround
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

		public float maxVisibleObjectRadius;
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
			ghostRegions.Clear();

			if (bufferRadius <= 0f) return;

			var axis0NormalOffset = Vector3.Dot(_transformedAxis0Vector, _axis0NegativePlane.normal);
			var axis1NormalOffset = Vector3.Dot(_transformedAxis1Vector, _axis1NegativePlane.normal);
			var axis2NormalOffset = Vector3.Dot(_transformedAxis2Vector, _axis2NegativePlane.normal);
			var axis0RegionBufferCount = axis0IsWrapped ? Mathf.CeilToInt(bufferRadius / axis0NormalOffset) : 0;
			var axis1RegionBufferCount = axis1IsWrapped ? Mathf.CeilToInt(bufferRadius / axis1NormalOffset) : 0;
			var axis2RegionBufferCount = axis1IsWrapped ? Mathf.CeilToInt(bufferRadius / axis2NormalOffset) : 0;

			for (int z = -axis2RegionBufferCount; z <= axis2RegionBufferCount; ++z)
			{
				for (int y = -axis1RegionBufferCount; y <= axis1RegionBufferCount; ++y)
				{
					for (int x = -axis0RegionBufferCount; x <= axis0RegionBufferCount; ++x)
					{
						if (x == 0 && y == 0 && z == 0) continue; // The 0,0 region is the real region, not a ghost region, so skip it.

						ghostRegions.Add(new RhomboidGhostRegion(_transformedAxis0Vector * x + _transformedAxis1Vector * y + _transformedAxis2Vector * z));
					}
				}
			}
		}

		protected void GetGhostRegions(Camera camera, float bufferRadius, List<GhostRegion> ghostRegions)
		{
			ghostRegions.Clear();

			// Get the six planes of the frustum volume, and expand them outward based on the buffer radius.
			var frustumPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
			if (bufferRadius != 0f)
			{
				for (int i = 0; i < frustumPlanes.Length; ++i)
				{
					frustumPlanes[i] = new Plane(frustumPlanes[i].normal, frustumPlanes[i].distance + bufferRadius);
				}
			}

			// Find the eight corners of the camera view frustum.
			var corners = MathUtility.FindFrustumCorners(camera, frustumPlanes);

			// Find the maximum ghost region index extents based on these eight corners.
			Index3D min = new Index3D(int.MaxValue, int.MaxValue, int.MaxValue);
			Index3D max = new Index3D(int.MinValue, int.MinValue, int.MinValue);

			for (int i = 0; i < corners.Length; ++i)
			{
				ExpandIndexBounds(GetGhostRegionIndex(corners[i]), ref min, ref max);
			}

			// If the bounds are limited to a single dimension, then just return a linear array of ghost regions.
			if (min.y == max.y && min.z == max.z)
			{
				var yzVector = _transformedAxis1Vector * min.y + _transformedAxis2Vector * min.z;
				for (int x = min.x; x <= max.x; ++x)
				{
					if (x == 0 && min.y == 0 && min.z == 0) continue; // The 0,0,0 region is the real region, not a ghost region, so skip it.
					ghostRegions.Add(new RhomboidGhostRegion(_transformedAxis0Vector * x + yzVector));
				}
				return;
			}
			else if (min.x == max.x && min.z == max.z)
			{
				var xzVector = _transformedAxis0Vector * min.x + _transformedAxis2Vector * min.z;
				for (int y = min.y; y <= max.y; ++y)
				{
					if (min.x == 0 && y == 0 && min.z == 0) continue; // The 0,0,0 region is the real region, not a ghost region, so skip it.
					ghostRegions.Add(new RhomboidGhostRegion(_transformedAxis1Vector * y + xzVector));
				}
				return;
			}
			else if (min.x == max.x && min.y == max.y)
			{
				var xyVector = _transformedAxis0Vector * min.x + _transformedAxis1Vector * min.y;
				for (int z = min.z; z <= max.z; ++z)
				{
					if (min.x == 0 && min.y == 0 && z == 0) continue; // The 0,0,0 region is the real region, not a ghost region, so skip it.
					ghostRegions.Add(new RhomboidGhostRegion(_transformedAxis2Vector * z + xyVector));
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

			for (int z = min.z; z <= max.z; ++z)
			{
				for (int y = min.y; y <= max.y; ++y)
				{
					for (int x = min.x; x <= max.x; ++x)
					{
						if (x == 0 && y == 0 && z == 0) continue; // The 0,0,0 region is the real region, not a ghost region, so skip it.

						if (unwrappedAxis == -1)
						{
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
								if (MathUtility.AllAreBelow(regionCorners, plane))
								{
									exclude = true;
									break;
								}
							}

							if (!exclude)
							{
								ghostRegions.Add(new RhomboidGhostRegion(regionCorners[0]));
							}
						}
						else
						{
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
								if (MathUtility.TruncateLineSegment(regionLines[0], plane, ref minT, ref maxT) <= 0f) continue;
								if (MathUtility.TruncateLineSegment(regionLines[1], plane, ref minT, ref maxT) <= 0f) continue;
								if (MathUtility.TruncateLineSegment(regionLines[2], plane, ref minT, ref maxT) <= 0f) continue;
								if (MathUtility.TruncateLineSegment(regionLines[3], plane, ref minT, ref maxT) <= 0f) continue;

								exclude = false;
								break;
							}

							if (!exclude)
							{
								ghostRegions.Add(new RhomboidGhostRegion(regionLines[0].origin));
							}
						}
					}
				}
			}
		}

		private Index3D GetGhostRegionIndex(Vector3 position)
		{
			position = transform.InverseTransformPoint(position);

			return new Index3D(
				axis0IsWrapped ? Mathf.FloorToInt(-MathUtility.GetIntersectionParameter(_axis0NegativePlane, new ScaledRay(position, _transformedAxis0Vector))) : 0,
				axis1IsWrapped ? Mathf.FloorToInt(-MathUtility.GetIntersectionParameter(_axis1NegativePlane, new ScaledRay(position, _transformedAxis1Vector))) : 0,
				axis2IsWrapped ? Mathf.FloorToInt(-MathUtility.GetIntersectionParameter(_axis2NegativePlane, new ScaledRay(position, _transformedAxis2Vector))) : 0);
		}

		private void ExpandIndexBounds(Index3D index, ref Index3D min, ref Index3D max)
		{
			min.x = Mathf.Min(min.x, index.x);
			min.y = Mathf.Min(min.y, index.y);
			min.z = Mathf.Min(min.z, index.z);
			max.x = Mathf.Max(max.x, index.x);
			max.y = Mathf.Max(max.y, index.y);
			max.z = Mathf.Max(max.z, index.z);
		}

		public override IEnumerable<GhostRegion> physicsGhostRegions { get { return _physicsGhostRegions; } }

		public override bool IsCollidable(Vector3 position)
		{
			return
				_axis0NegativePlane.GetDistanceToPoint(position) >= -maxPhysicsObjectRadius &&
				_axis0PositivePlane.GetDistanceToPoint(position) >= -maxPhysicsObjectRadius &&
				_axis1NegativePlane.GetDistanceToPoint(position) >= -maxPhysicsObjectRadius &&
				_axis1PositivePlane.GetDistanceToPoint(position) >= -maxPhysicsObjectRadius;
		}

		public override bool IsCollidable(Vector3 position, float radius)
		{
			var extendedRadius = radius + maxPhysicsObjectRadius;
			return
				_axis0NegativePlane.GetDistanceToPoint(position) >= -extendedRadius &&
				_axis0PositivePlane.GetDistanceToPoint(position) >= -extendedRadius &&
				_axis1NegativePlane.GetDistanceToPoint(position) >= -extendedRadius &&
				_axis1PositivePlane.GetDistanceToPoint(position) >= -extendedRadius;
		}

		public override bool IsCollidable(Bounds box)
		{
			return
				box.IsAboveOrIntersects(_axis0NegativePlane) ||
				box.IsAboveOrIntersects(_axis0PositivePlane) ||
				box.IsAboveOrIntersects(_axis1NegativePlane) ||
				box.IsAboveOrIntersects(_axis1PositivePlane);
		}

		public override bool IsCollidable(Vector3 position, Bounds box)
		{
			return IsCollidable(new Bounds(box.center + position, box.size));
		}

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

		public void Confine(ref Vector3 position)
		{
			var localPosition = transform.InverseTransformPoint(position);

			if (axis0IsWrapped) localPosition -= Mathf.Floor(-MathUtility.GetIntersectionParameter(_axis0NegativePlane, new ScaledRay(position, _transformedAxis0Vector))) * _transformedAxis0Vector;
			if (axis1IsWrapped) localPosition -= Mathf.Floor(-MathUtility.GetIntersectionParameter(_axis1NegativePlane, new ScaledRay(position, _transformedAxis1Vector))) * _transformedAxis1Vector;
			if (axis2IsWrapped) localPosition -= Mathf.Floor(-MathUtility.GetIntersectionParameter(_axis2NegativePlane, new ScaledRay(position, _transformedAxis2Vector))) * _transformedAxis2Vector;

			position = transform.TransformPoint(localPosition);
		}
	}
}
