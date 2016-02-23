using UnityEngine;
using System;

namespace Experilous.WrapAround
{
	public class RhomboidGhostRegion : GhostRegion, IEquatable<RhomboidGhostRegion>, IEquatable<Index3D>, IComparable<RhomboidGhostRegion>, IComparable<Index3D>
	{
		private Index3D _index;
		private Vector3 _offset;
		private bool _isActive = true;

		public RhomboidGhostRegion(Index3D index, Vector3 offset)
		{
			_index = index;
			_offset = offset;
		}

		public override bool isActive { get { return _isActive; } set { _isActive = value; } }

		public override void Transform(ref Vector3 position, ref Quaternion rotation)
		{
			position += _offset;
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

		public override Matrix4x4 transformation { get { return Matrix4x4.TRS(_offset, Quaternion.identity, Vector3.one); } }

		public override string ToString()
		{
			return string.Format("Rhomboid Ghost Region ({0}, {1})", _offset, _isActive ? "active" : "inactive");
		}

		public bool Equals(RhomboidGhostRegion other)
		{
			return this == other;
		}

		public bool Equals(Index3D index)
		{
			return this == index;
		}

		public override bool Equals(object obj)
		{
			return obj is RhomboidGhostRegion && this == (RhomboidGhostRegion)obj || obj is Index3D && this == (Index3D)obj;
		}

		public override int GetHashCode()
		{
			return _index.GetHashCode();
		}

		public int CompareTo(RhomboidGhostRegion other)
		{
			return _index.CompareTo(other._index);
		}

		public int CompareTo(Index3D index)
		{
			return _index.CompareTo(index);
		}

		public static bool operator ==(RhomboidGhostRegion lhs, RhomboidGhostRegion rhs) { return lhs._index == rhs._index; }
		public static bool operator !=(RhomboidGhostRegion lhs, RhomboidGhostRegion rhs) { return lhs._index != rhs._index; }
		public static bool operator < (RhomboidGhostRegion lhs, RhomboidGhostRegion rhs) { return lhs._index <  rhs._index; }
		public static bool operator <=(RhomboidGhostRegion lhs, RhomboidGhostRegion rhs) { return lhs._index <= rhs._index; }
		public static bool operator > (RhomboidGhostRegion lhs, RhomboidGhostRegion rhs) { return lhs._index >  rhs._index; }
		public static bool operator >=(RhomboidGhostRegion lhs, RhomboidGhostRegion rhs) { return lhs._index >= rhs._index; }

		public static bool operator ==(RhomboidGhostRegion lhs, Index3D rhs) { return lhs._index == rhs; }
		public static bool operator !=(RhomboidGhostRegion lhs, Index3D rhs) { return lhs._index != rhs; }
		public static bool operator < (RhomboidGhostRegion lhs, Index3D rhs) { return lhs._index <  rhs; }
		public static bool operator <=(RhomboidGhostRegion lhs, Index3D rhs) { return lhs._index <= rhs; }
		public static bool operator > (RhomboidGhostRegion lhs, Index3D rhs) { return lhs._index >  rhs; }
		public static bool operator >=(RhomboidGhostRegion lhs, Index3D rhs) { return lhs._index >= rhs; }

		public static bool operator ==(Index3D lhs, RhomboidGhostRegion rhs) { return lhs == rhs._index; }
		public static bool operator !=(Index3D lhs, RhomboidGhostRegion rhs) { return lhs != rhs._index; }
		public static bool operator < (Index3D lhs, RhomboidGhostRegion rhs) { return lhs <  rhs._index; }
		public static bool operator <=(Index3D lhs, RhomboidGhostRegion rhs) { return lhs <= rhs._index; }
		public static bool operator > (Index3D lhs, RhomboidGhostRegion rhs) { return lhs >  rhs._index; }
		public static bool operator >=(Index3D lhs, RhomboidGhostRegion rhs) { return lhs >= rhs._index; }
	}
}
