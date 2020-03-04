/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using System;
using Experilous.Numerics;

namespace Experilous.MakeItWrap
{
	public class RhomboidGhostRegion : GhostRegion, IEquatable<RhomboidGhostRegion>, IEquatable<IntVector3>, IComparable<RhomboidGhostRegion>, IComparable<IntVector3>
	{
		private IntVector3 _index;
		private Vector3 _offset;
		private bool _isActive = true;

		public RhomboidGhostRegion(IntVector3 index, Vector3 offset)
		{
			_index = index;
			_offset = offset;
		}

		public override bool isActive { get { return _isActive; } set { _isActive = value; } }

		public override void Transform(ref Vector3 position)
		{
			position += _offset;
		}

		public override void Transform(ref Vector3 position, ref Quaternion rotation)
		{
			position += _offset;
		}

		public override void Transform(ref Vector3 position, ref Quaternion rotation, ref Vector3 scale)
		{
			position += _offset;
		}

		public override void Transform(ref Matrix4x4 transformation)
		{
			transformation *= Matrix4x4.TRS(_offset, Quaternion.identity, Vector3.one);
		}

		public override Vector3 Transform(Vector3 position)
		{
			return position + _offset;
		}

		public override Bounds Transform(Bounds axisAlignedBox)
		{
			return new Bounds(axisAlignedBox.center + _offset, axisAlignedBox.size);
		}

		public override Sphere Transform(Sphere sphere)
		{
			return new Sphere(sphere.center + _offset, sphere.radius);
		}

		public override void Transform(Transform sourceTransform, Transform targetTransform)
		{
			targetTransform.position = sourceTransform.position + _offset;
			targetTransform.rotation = sourceTransform.rotation;
		}

		public override void Transform(Rigidbody sourceRigidbody, Rigidbody targetRigidbody)
		{
			targetRigidbody.position = sourceRigidbody.position + _offset;
			targetRigidbody.rotation = sourceRigidbody.rotation;
		}

		public override void Transform(Rigidbody2D sourceRigidbody, Rigidbody2D targetRigidbody)
		{
			targetRigidbody.position = sourceRigidbody.position + (Vector2)_offset;
			targetRigidbody.rotation = sourceRigidbody.rotation;
		}

		public override Matrix4x4 transformation { get { return Matrix4x4.TRS(_offset, Quaternion.identity, Vector3.one); } }

		public override string ToString()
		{
			return string.Format("Rhomboid Ghost Region ({0}, {1})", _offset, _isActive ? "active" : "inactive");
		}

		public bool Equals(RhomboidGhostRegion other)
		{
			return this == other;
		}

		public bool Equals(IntVector3 index)
		{
			return this == index;
		}

		public override bool Equals(object obj)
		{
			return obj is RhomboidGhostRegion && this == (RhomboidGhostRegion)obj || obj is IntVector3 && this == (IntVector3)obj;
		}

		public override int GetHashCode()
		{
			return _index.GetHashCode();
		}

		public int CompareTo(RhomboidGhostRegion other)
		{
			return _index.CompareTo(other._index);
		}

		public int CompareTo(IntVector3 index)
		{
			return _index.CompareTo(index);
		}

		public static bool operator ==(RhomboidGhostRegion lhs, RhomboidGhostRegion rhs) { return lhs._index == rhs._index; }
		public static bool operator !=(RhomboidGhostRegion lhs, RhomboidGhostRegion rhs) { return lhs._index != rhs._index; }
		public static bool operator < (RhomboidGhostRegion lhs, RhomboidGhostRegion rhs) { return lhs._index <  rhs._index; }
		public static bool operator <=(RhomboidGhostRegion lhs, RhomboidGhostRegion rhs) { return lhs._index <= rhs._index; }
		public static bool operator > (RhomboidGhostRegion lhs, RhomboidGhostRegion rhs) { return lhs._index >  rhs._index; }
		public static bool operator >=(RhomboidGhostRegion lhs, RhomboidGhostRegion rhs) { return lhs._index >= rhs._index; }

		public static bool operator ==(RhomboidGhostRegion lhs, IntVector3 rhs) { return lhs._index == rhs; }
		public static bool operator !=(RhomboidGhostRegion lhs, IntVector3 rhs) { return lhs._index != rhs; }
		public static bool operator < (RhomboidGhostRegion lhs, IntVector3 rhs) { return lhs._index <  rhs; }
		public static bool operator <=(RhomboidGhostRegion lhs, IntVector3 rhs) { return lhs._index <= rhs; }
		public static bool operator > (RhomboidGhostRegion lhs, IntVector3 rhs) { return lhs._index >  rhs; }
		public static bool operator >=(RhomboidGhostRegion lhs, IntVector3 rhs) { return lhs._index >= rhs; }

		public static bool operator ==(IntVector3 lhs, RhomboidGhostRegion rhs) { return lhs == rhs._index; }
		public static bool operator !=(IntVector3 lhs, RhomboidGhostRegion rhs) { return lhs != rhs._index; }
		public static bool operator < (IntVector3 lhs, RhomboidGhostRegion rhs) { return lhs <  rhs._index; }
		public static bool operator <=(IntVector3 lhs, RhomboidGhostRegion rhs) { return lhs <= rhs._index; }
		public static bool operator > (IntVector3 lhs, RhomboidGhostRegion rhs) { return lhs >  rhs._index; }
		public static bool operator >=(IntVector3 lhs, RhomboidGhostRegion rhs) { return lhs >= rhs._index; }
	}
}
