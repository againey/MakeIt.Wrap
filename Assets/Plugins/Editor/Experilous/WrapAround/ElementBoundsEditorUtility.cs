/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using UnityEditor;
using System;

namespace Experilous.WrapAround
{
	public static class ElementBoundsEditorUtility
	{
		private static GUIContent[] _boundsTypeLabels = new GUIContent[]
		{
			new GUIContent("Local Origin Bounds"),
			new GUIContent("Point Bounds"),
			new GUIContent("SphereBounds"),
			new GUIContent("Axis Aligned Box Bounds"),
		};

		public static bool OnInspectorGUI(BoundedElement element)
		{
			bool changed = false;

			var originalBounds = element.bounds;

			if (originalBounds != null)
			{
				EditorGUILayout.LabelField("Bounds", string.Format("{0} ({1})", originalBounds.GetType().GetPrettyName(), originalBounds.GetInstanceID()));
			}
			else
			{
				EditorGUILayout.LabelField("Bounds", "<null>");
			}

			int originalBoundsTypeIndex;
			bool originalFixedScale;
			bool originalFixedRotation;

			int boundsTypeIndex;
			bool fixedScale;
			bool fixedRotation;

			Vector3 center = Vector3.zero;
			float radius = 0f;
			Vector3 extents = Vector3.zero;

			var boundsTypeGUIContent = new GUIContent("Bounds Type", "The method by which a bounding volume is obtained for this wrap-around element, which will affect when the appropriate warp-around behavior is applied.");
			var isFixedScaleGUIContent = new GUIContent("Fixed Scale", "This element's transform scale is never expected to change anywhere in its ancestry.");
			var isFixedRotationGUIContent = new GUIContent("Fixed Rotation", "This element's transform rotation is never expected to change anywhere in its ancestry.");

			if (element.bounds is LocalOriginBounds)
			{
				originalBoundsTypeIndex = 0;
				originalFixedScale = false;
				originalFixedRotation = false;

				boundsTypeIndex = EditorGUILayout.Popup(boundsTypeGUIContent, originalBoundsTypeIndex, _boundsTypeLabels);
				fixedScale = false;
				fixedRotation = false;
			}
			else if (element.bounds is PointBounds)
			{
				originalBoundsTypeIndex = 1;
				originalFixedScale = (element.bounds is FixedPointBounds || element.bounds is RotatablePointBounds);
				originalFixedRotation = (element.bounds is FixedPointBounds || element.bounds is ScalablePointBounds);

				boundsTypeIndex = EditorGUILayout.Popup(boundsTypeGUIContent, originalBoundsTypeIndex, _boundsTypeLabels);
				fixedScale = EditorGUILayout.Toggle(isFixedScaleGUIContent, originalFixedScale);
				fixedRotation = EditorGUILayout.Toggle(isFixedRotationGUIContent, originalFixedRotation);

				center = EditorGUILayout.Vector3Field(new GUIContent("Position"), ((PointBounds)element.bounds).position);

				if (GUILayout.Button(new GUIContent("Auto Compute Bounds", "Recalculate this element's center position based on the above settings and the configuration of all relevant sibling components and nested game objects.")))
				{
					var computedBox = AxisAlignedBoxBounds.InverseTransform(element.transform, element.ComputeAxisAlignedBoxBounds(), fixedScale);
					center = computedBox.center;
				}
			}
			else if (element.bounds is SphereBounds)
			{
				var sphereBounds = (SphereBounds)element.bounds;

				originalBoundsTypeIndex = 2;
				originalFixedScale = (element.bounds is FixedSphereBounds || element.bounds is RotatableSphereBounds);
				originalFixedRotation = (element.bounds is FixedSphereBounds || element.bounds is ScalableSphereBounds);

				boundsTypeIndex = EditorGUILayout.Popup(boundsTypeGUIContent, originalBoundsTypeIndex, _boundsTypeLabels);
				fixedScale = EditorGUILayout.Toggle(isFixedScaleGUIContent, originalFixedScale);
				fixedRotation = EditorGUILayout.Toggle(isFixedRotationGUIContent, originalFixedRotation);

				center = EditorGUILayout.Vector3Field(new GUIContent("Center"), sphereBounds.sphere.center);
				radius = EditorGUILayout.FloatField(new GUIContent("Radius"), sphereBounds.sphere.radius);

				if (GUILayout.Button(new GUIContent("Auto Compute Bounds", "Recalculate this element's bounding volume based on the above settings and the configuration of all relevant sibling components and nested game objects.")))
				{
					var computedSphere = SphereBounds.InverseTransform(element.transform, element.ComputeSphereBounds(), fixedScale, fixedRotation);
					center = computedSphere.center;
					radius = computedSphere.radius;
				}

				extents = new Vector3(radius, radius, radius);
			}
			else if (element.bounds is AxisAlignedBoxBounds)
			{
				var axisAlignedBoxBounds = (AxisAlignedBoxBounds)element.bounds;

				originalBoundsTypeIndex = 3;
				originalFixedScale = (element.bounds is FixedAxisAlignedBoxBounds);
				originalFixedRotation = true;

				boundsTypeIndex = EditorGUILayout.Popup(boundsTypeGUIContent, originalBoundsTypeIndex, _boundsTypeLabels);
				fixedScale = EditorGUILayout.Toggle(isFixedScaleGUIContent, originalFixedScale);
				var originalEnabledState = GUI.enabled;
				GUI.enabled = false;
				EditorGUILayout.Toggle(isFixedRotationGUIContent, originalFixedRotation);
				GUI.enabled = originalEnabledState;
				fixedRotation = true;

				center = EditorGUILayout.Vector3Field(new GUIContent("Center"), axisAlignedBoxBounds.box.center);
				extents = EditorGUILayout.Vector3Field(new GUIContent("Extents"), axisAlignedBoxBounds.box.extents);

				if (GUILayout.Button(new GUIContent("Auto Compute Bounds", "Recalculate this element's bounding volume based on the above settings and the configuration of all relevant sibling components and nested game objects.")))
				{
					var computedBox = AxisAlignedBoxBounds.InverseTransform(element.transform, element.ComputeAxisAlignedBoxBounds(), fixedScale);
					center = computedBox.center;
					extents = computedBox.extents;
				}

				radius = extents.magnitude;
			}
			else
			{
				originalBoundsTypeIndex = 4;
				originalFixedScale = false;
				originalFixedRotation = false;

				var customBoundsTypeLabels = new GUIContent[_boundsTypeLabels.Length + 1];
				Array.Copy(_boundsTypeLabels, customBoundsTypeLabels, _boundsTypeLabels.Length);
				customBoundsTypeLabels[_boundsTypeLabels.Length] = new GUIContent(string.Format("Other ({0})", element.bounds != null ? element.bounds.GetType().GetPrettyName() : "None"));

				boundsTypeIndex = EditorGUILayout.Popup(boundsTypeGUIContent, originalBoundsTypeIndex, customBoundsTypeLabels);
				fixedScale = false;
				fixedRotation = false;
			}

			if (boundsTypeIndex != originalBoundsTypeIndex)
			{
				switch (boundsTypeIndex)
				{
					case 0:
						element.bounds = LocalOriginBounds.Create(element.gameObject);
						break;
					case 1:
						element.bounds = PointBounds.Create(element.gameObject, center, fixedScale, fixedRotation);
						break;
					case 2:
						element.bounds = SphereBounds.Create(element.gameObject, new Sphere(center, radius), fixedScale, fixedRotation);
						break;
					case 3:
						element.bounds = AxisAlignedBoxBounds.Create(element.gameObject, new Bounds(center, extents * 2f), fixedScale);
						break;
					default:
						throw new NotImplementedException();
				}
			}
			else
			{
				switch (boundsTypeIndex)
				{
					case 1:
						if (fixedScale != originalFixedScale || fixedRotation != originalFixedRotation)
						{
							element.bounds = PointBounds.Create(element.gameObject, center, fixedScale, fixedRotation);
						}
						else
						{
							var pointBounds = (PointBounds)element.bounds;
							if (center != pointBounds.position)
							{
								changed = true;
								pointBounds.position = center;
							}
						}
						break;
					case 2:
						if (fixedScale != originalFixedScale || fixedRotation != originalFixedRotation)
						{
							element.bounds = SphereBounds.Create(element.gameObject, new Sphere(center, radius), fixedScale, fixedRotation);
						}
						else
						{
							var sphereBounds = (SphereBounds)element.bounds;
							if (center != sphereBounds.sphere.center || radius != sphereBounds.sphere.radius)
							{
								changed = true;
								sphereBounds.sphere = new Sphere(center, radius);
							}
						}
						break;
					case 3:
						if (fixedScale != originalFixedScale)
						{
							element.bounds = AxisAlignedBoxBounds.Create(element.gameObject, new Bounds(center, extents * 2f), fixedScale);
						}
						else
						{
							var axisAlignedBoxBounds = (AxisAlignedBoxBounds)element.bounds;
							if (center != axisAlignedBoxBounds.box.center || extents != axisAlignedBoxBounds.box.extents)
							{
								changed = true;
								axisAlignedBoxBounds.box = new Bounds(center, extents * 2f);
							}
						}
						break;
				}
			}

			if (!ReferenceEquals(element.bounds, originalBounds))
			{
				changed = true;

				var prefabType = PrefabUtility.GetPrefabType(element.gameObject);
				if (prefabType == PrefabType.Prefab)
				{
					if (element.bounds != null)
					{
						AssetDatabase.AddObjectToAsset(element.bounds, element.gameObject);
					}

					if (originalBounds != null)
					{
						UnityEngine.Object.DestroyImmediate(originalBounds, true);
					}
				}
			}

			return changed;
		}

		#region DrawGizmos...(...)

		public static void DrawGizmosSelected(ElementBounds bounds, Color color)
		{
			if (bounds is PointBounds)
			{
				DrawGizmosSelected(bounds as PointBounds, color);
			}
			else if (bounds is SphereBounds)
			{
				DrawGizmosSelected(bounds as SphereBounds, color);
			}
			else if (bounds is AxisAlignedBoxBounds)
			{
				DrawGizmosSelected(bounds as AxisAlignedBoxBounds, color);
			}
		}

		public static void DrawGizmosSelected(ElementBounds bounds, GhostRegion ghostRegion, Color color)
		{
			if (bounds is PointBounds)
			{
				DrawGizmosSelected(bounds as PointBounds, ghostRegion, color);
			}
			else if (bounds is SphereBounds)
			{
				DrawGizmosSelected(bounds as SphereBounds, ghostRegion, color);
			}
			else if (bounds is AxisAlignedBoxBounds)
			{
				DrawGizmosSelected(bounds as AxisAlignedBoxBounds, ghostRegion, color);
			}
		}

		public static void DrawGizmosSelected(LocalOriginBounds bounds, Color color)
		{
			var transformedPosition = bounds.Transform();
			Gizmos.color = color;
			Gizmos.DrawLine(transformedPosition - new Vector3(0.25f, 0f, 0f), transformedPosition + new Vector3(0.25f, 0f, 0f));
			Gizmos.DrawLine(transformedPosition - new Vector3(0f, 0.25f, 0f), transformedPosition + new Vector3(0f, 0.25f, 0f));
			Gizmos.DrawLine(transformedPosition - new Vector3(0f, 0f, 0.25f), transformedPosition + new Vector3(0f, 0f, 0.25f));
		}

		public static void DrawGizmosSelected(PointBounds bounds, Color color)
		{
			var transformedPosition = bounds.Transform();
			Gizmos.color = color;
			Gizmos.DrawLine(transformedPosition - new Vector3(0.25f, 0f, 0f), transformedPosition + new Vector3(0.25f, 0f, 0f));
			Gizmos.DrawLine(transformedPosition - new Vector3(0f, 0.25f, 0f), transformedPosition + new Vector3(0f, 0.25f, 0f));
			Gizmos.DrawLine(transformedPosition - new Vector3(0f, 0f, 0.25f), transformedPosition + new Vector3(0f, 0f, 0.25f));
		}

		public static void DrawGizmosSelected(PointBounds bounds, GhostRegion ghostRegion, Color color)
		{
			var transformedPosition = bounds.Transform(ghostRegion);
			Gizmos.color = color;
			Gizmos.DrawLine(transformedPosition - new Vector3(0.25f, 0f, 0f), transformedPosition + new Vector3(0.25f, 0f, 0f));
			Gizmos.DrawLine(transformedPosition - new Vector3(0f, 0.25f, 0f), transformedPosition + new Vector3(0f, 0.25f, 0f));
			Gizmos.DrawLine(transformedPosition - new Vector3(0f, 0f, 0.25f), transformedPosition + new Vector3(0f, 0f, 0.25f));
		}

		public static void DrawGizmosSelected(SphereBounds bounds, Color color)
		{
			var transformedSphere = bounds.Transform();
			Gizmos.color = color;
			Gizmos.DrawWireSphere(transformedSphere.center, transformedSphere.radius);
		}

		public static void DrawGizmosSelected(SphereBounds bounds, GhostRegion ghostRegion, Color color)
		{
			var transformedSphere = bounds.Transform(ghostRegion);
			Gizmos.color = color;
			Gizmos.DrawWireSphere(transformedSphere.center, transformedSphere.radius);
		}

		public static void DrawGizmosSelected(AxisAlignedBoxBounds bounds, Color color)
		{
			var transformedBox = bounds.Transform();
			Gizmos.color = color;
			Gizmos.DrawWireCube(transformedBox.center, transformedBox.size);
		}

		public static void DrawGizmosSelected(AxisAlignedBoxBounds bounds, GhostRegion ghostRegion, Color color)
		{
			var transformedBox = bounds.Transform(ghostRegion);
			Gizmos.color = color;
			Gizmos.DrawWireCube(transformedBox.center, transformedBox.size);
		}

		#endregion
	}
}
