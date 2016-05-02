/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System;

namespace Experilous.WrapAround
{
	[CustomEditor(typeof(WrappedElement))]
	public class WrappedElementEditor : Editor
	{
		[NonSerialized] private static Texture2D _refreshButtonIcon;
		[NonSerialized] private static Texture2D _refreshButtonIconDisabled;
		[NonSerialized] private static GUIStyle _refreshButtonStyle;
		[NonSerialized] private static Texture2D _saveAsPrefabButtonIcon;
		[NonSerialized] private static Texture2D _saveAsPrefabButtonIconDisabled;
		[NonSerialized] private static GUIStyle _saveAsPrefabButtonStyle;

		private void InitializeStyles()
		{
			if (_refreshButtonIcon == null)
			{
				_refreshButtonIcon = new Texture2D(0, 0, TextureFormat.ARGB32, false);
				var iconPath = System.IO.Path.Combine(AssetUtility.GetFullScriptFolder(this), "RefreshIcon.png");
				_refreshButtonIcon.LoadImage(System.IO.File.ReadAllBytes(iconPath));
			}

			if (_refreshButtonIconDisabled == null)
			{
				_refreshButtonIconDisabled = new Texture2D(0, 0, TextureFormat.ARGB32, false);
				var iconPath = System.IO.Path.Combine(AssetUtility.GetFullScriptFolder(this), "RefreshIconDisabled.png");
				_refreshButtonIconDisabled.LoadImage(System.IO.File.ReadAllBytes(iconPath));
			}

			if (_refreshButtonStyle == null)
			{
				_refreshButtonStyle = new GUIStyle(EditorStyles.label);
				_refreshButtonStyle.padding = new RectOffset(1, 1, 1, 1);
			}

			if (_saveAsPrefabButtonStyle == null)
			{
				_saveAsPrefabButtonStyle = new GUIStyle(EditorStyles.label);
				_saveAsPrefabButtonStyle.padding = new RectOffset(0, 0, 0, 0);
			}

			if (_saveAsPrefabButtonIcon == null)
			{
				_saveAsPrefabButtonIcon = (Texture2D)EditorGUIUtility.Load("PrefabModel Icon");
			}
		}

		public override void OnInspectorGUI()
		{
			InitializeStyles();

			var element = (WrappedElement)target;

			element.fixedScale = EditorGUILayout.Toggle(new GUIContent("Fixed Scale"), element.fixedScale);
			element.fixedRotation = EditorGUILayout.Toggle(new GUIContent("Fixed Rotation"), element.fixedRotation);

			#region Wrap Lights

			if (element.canHaveWrappedLightElement || element.wrappedLightElement != null)
			{
				EditorGUILayout.Space();

				bool isEnabled = (element.wrappedLightElement != null);
				isEnabled = EditorGUILayout.ToggleLeft(new GUIContent("Wrap Lights"), isEnabled);

				if (isEnabled)
				{
					if (element.wrappedLightElement == null)
					{
						element.wrappedLightElement = element.gameObject.AddComponent<LightElement>();
						element.wrappedLightElement.hideFlags = HideFlags.HideInInspector;
					}

					EditorGUI.indentLevel += 1;

					var rect = EditorGUILayout.GetControlRect(true, EditorStyles.popup.CalcHeight(GUIContent.none, float.PositiveInfinity), EditorStyles.popup);
					var id = GUIUtility.GetControlID(FocusType.Native);
					rect = EditorGUI.PrefixLabel(rect, id, new GUIContent("Bounds"));
					rect.xMax -= 18;
					if (GUI.Button(rect, BoundsPopupContent.GetStateContent(element.wrappedLightBounds), EditorStyles.popup))
					{
						PopupWindow.Show(rect, new BoundsPopupContent(element, element.wrappedLightElement, element.wrappedLightBounds, rect.width));
					}
					rect.x = rect.xMax + 3;
					rect.width = 15;
					var canAutoGenerate = CanAutoGenerate(element.wrappedLightBounds);
					GUIExtensions.PushEnable(canAutoGenerate);
					if (GUI.Button(rect, new GUIContent(canAutoGenerate ? _refreshButtonIcon : _refreshButtonIconDisabled, "Auto-Generate Bounds"), _refreshButtonStyle))
					{
						Debug.Log("Auto  Generate");
					}
					GUIExtensions.PopEnable();

					rect = EditorGUILayout.GetControlRect(false, EditorStyles.objectField.CalcHeight(GUIContent.none, float.PositiveInfinity), EditorStyles.objectField);
					rect.xMax -= 36;
					EditorGUI.ObjectField(rect, new GUIContent("Ghost"), null, typeof(LightElementGhost), true);
					rect.x = rect.xMax + 3;
					rect.width = 15;
					GUI.Button(rect, new GUIContent(_refreshButtonIcon, "Auto-Generate Ghost"), _refreshButtonStyle);
					rect.x = rect.xMax + 3;
					rect.width = 15;
					GUI.Button(rect, new GUIContent(_saveAsPrefabButtonIcon, "Save Ghost As Prefab"), _saveAsPrefabButtonStyle);

					EditorGUI.indentLevel -= 1;
				}
				else if (element.wrappedLightElement != null)
				{
					DestroyImmediate(element.wrappedLightElement);
					element.wrappedLightElement = null;
				}
			}

			#endregion

			#region Collider

			if (element.canHaveWrappedColliderElement || element.wrappedColliderElement != null)
			{
				EditorGUILayout.Space();

				bool isEnabled = (element.wrappedColliderElement != null);
				isEnabled = EditorGUILayout.ToggleLeft(new GUIContent("Wrap Collider"), isEnabled);

				if (isEnabled)
				{
					if (element.wrappedColliderElement == null)
					{
						element.wrappedColliderElement = element.gameObject.AddComponent<ColliderElement>();
						element.wrappedColliderElement.hideFlags = HideFlags.HideInInspector;
					}

					EditorGUI.indentLevel += 1;
					EditorGUILayout.Popup(new GUIContent("Bounds"), 0, new GUIContent[] { new GUIContent("One"), new GUIContent("Two"), new GUIContent("Three"), });
					EditorGUILayout.ObjectField("Ghost", null, typeof(ColliderElement), true);
					EditorGUI.indentLevel -= 1;
				}
				else if (element.wrappedColliderElement != null)
				{
					DestroyImmediate(element.wrappedColliderElement);
					element.wrappedColliderElement = null;
				}
			}

			#endregion

			#region Rigidbody

			if (element.canHaveWrappedRigidbodyElement || element.wrappedRigidbodyElement != null)
			{
				EditorGUILayout.Space();

				bool isEnabled = (element.wrappedRigidbodyElement != null);
				isEnabled = EditorGUILayout.ToggleLeft(new GUIContent("Wrap Rigidbody"), isEnabled);

				if (isEnabled)
				{
					if (element.wrappedRigidbodyElement == null)
					{
						element.wrappedRigidbodyElement = element.gameObject.AddComponent<RigidbodyElement>();
						element.wrappedRigidbodyElement.hideFlags = HideFlags.HideInInspector;
					}

					EditorGUI.indentLevel += 1;
					EditorGUILayout.Popup(new GUIContent("Bounds"), 0, new GUIContent[] { new GUIContent("One"), new GUIContent("Two"), new GUIContent("Three"), });
					EditorGUILayout.ObjectField("Ghost", null, typeof(RigidbodyElement), true);
					EditorGUI.indentLevel -= 1;
				}
				else if (element.wrappedRigidbodyElement != null)
				{
					DestroyImmediate(element.wrappedRigidbodyElement);
					element.wrappedRigidbodyElement = null;
				}
			}

			#endregion

			EditorGUILayout.Space();
		}

		private static bool CanAutoGenerate(WrappedElement.BoundsState boundsState)
		{
			return
				boundsState.type != WrappedElement.BoundsType.LocalOrigin &&
				boundsState.type != WrappedElement.BoundsType.Reference;
		}

		private void OnElementTypeGUI(ref bool isExpanded, ref bool isEnabled, AnimBool foldoutAnimation, GUIContent labelContent, System.Action<bool, bool> onInnerGUI)
		{
			float foldoutMinWidth, foldoutMaxWidth;
			EditorStyles.foldout.CalcMinMaxWidth(GUIContent.none, out foldoutMinWidth, out foldoutMaxWidth);

			EditorGUILayout.BeginVertical();

			EditorGUILayout.BeginHorizontal();
			var foldoutRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, GUILayout.Width(0));
			isExpanded = EditorGUI.Foldout(foldoutRect, isExpanded && isEnabled, GUIContent.none, false);
			EditorGUILayout.BeginVertical();

			EditorGUILayout.BeginHorizontal();
			isEnabled = EditorGUILayout.Toggle(labelContent, isEnabled, GUILayout.Width(EditorStyles.toggle.CalcSize(GUIContent.none).x));
			EditorGUILayout.EndHorizontal();

			foldoutAnimation.target = isExpanded && isEnabled;

			Debug.LogFormat("Inner: {0}, {1}", isExpanded, isEnabled);

			GUIExtensions.PushEnable(isEnabled);
			onInnerGUI(EditorGUILayout.BeginFadeGroup(foldoutAnimation.faded), isEnabled);
			EditorGUILayout.EndFadeGroup();
			GUIExtensions.PopEnable();

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
		}

		private class BoundsPopupContent : PopupWindowContent
		{
			private WrappedElement _element;
			private Component _elementComponent;
			private WrappedElement.BoundsState _boundsState;

			private float _width;

			[NonSerialized] private static GUIStyle _itemStyle;
			[NonSerialized] private static GUIStyle _separatorStyle;

			public static GUIContent GetStateContent(WrappedElement.BoundsState boundsState)
			{
				switch (boundsState.type)
				{
					case WrappedElement.BoundsType.Automatic:
						return new GUIContent("Automatic", "Automatically generate the bounds from all relevant components on the game object and its descendents.");
					case WrappedElement.BoundsType.LocalOrigin:
						return new GUIContent("Local Origin", "Use the game object's local origin as point bounds for this wrapped component.");
					case WrappedElement.BoundsType.Point:
						return new GUIContent("Point", "Manually specify the point bounds for this wrapped component.");
					case WrappedElement.BoundsType.Sphere:
						return new GUIContent("Sphere", "Manually specify the sphere bounds for this wrapped component.");
					case WrappedElement.BoundsType.AxisAlignedBox:
						return new GUIContent("Axis Aligned Box", "Manually specify the axis aligned box bounds for this wrapped component.");
					case WrappedElement.BoundsType.Reference:
						if (boundsState.reference == null)
							throw new ArgumentException();

						var referenceType = boundsState.reference.GetType();
						if (referenceType == typeof(LightElement))
							return new GUIContent("Other (Wrapped Light)", "Use the same bounds that are specified for the wrapped light component.");
						else if (referenceType == typeof(ColliderElement))
							return new GUIContent("Other (Wrapped Collider)", "Use the same bounds that are specified for the wrapped collider component.");
						else if (referenceType == typeof(RigidbodyElement))
							return new GUIContent("Other (Wrapped Rigidbody)", "Use the same bounds that are specified for the wrapped rigidbody component.");
						else
							throw new NotImplementedException();

					default:
						throw new NotImplementedException();
				}
			}

			private static void InitializeStyles()
			{
				if (_itemStyle == null)
				{
					_itemStyle = new GUIStyle("MenuItem");
					_itemStyle.fixedHeight = 0;
					_itemStyle.padding = new RectOffset(19, 3, 4, 3);
					_itemStyle.margin = new RectOffset(0, 0, 0, 0);
				}

				if (_separatorStyle == null)
				{
					_separatorStyle = new GUIStyle("MenuItem");
					_separatorStyle.fixedHeight = 1;
					_separatorStyle.padding = new RectOffset(0, 0, 0, 0);
					_separatorStyle.margin = new RectOffset(0, 0, 2, 2);

					var texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
					texture.SetPixel(0, 0, Color.gray);
					texture.Apply();
					_separatorStyle.normal.background = texture;
				}
			}

			public BoundsPopupContent(WrappedElement element, Component elementComponent, WrappedElement.BoundsState boundsState, float width)
			{
				_element = element;
				_elementComponent = elementComponent;
				_width = width;
				_boundsState = boundsState;
			}

			public override Vector2 GetWindowSize()
			{
				InitializeStyles();

				int componentCount = 0;
				if (_element.wrappedLightElement != null && !ReferenceEquals(_element.wrappedLightElement, _elementComponent)) ++componentCount;
				if (_element.wrappedColliderElement != null && !ReferenceEquals(_element.wrappedColliderElement, _elementComponent)) ++componentCount;
				if (_element.wrappedRigidbodyElement != null && !ReferenceEquals(_element.wrappedRigidbodyElement, _elementComponent)) ++componentCount;

				float height = _itemStyle.CalcHeight(GUIContent.none, _width) * (5 + componentCount);
				height += 5f; // Auto/Manual separator
				if (componentCount > 0) height += 5f; // Manual/Reference separator

				return new Vector2(_width, height);
			}

			public override void OnGUI(Rect rect)
			{
				InitializeStyles();

				if (GUILayout.Button(GetStateContent(new WrappedElement.BoundsState(WrappedElement.BoundsType.Automatic)), _itemStyle))
				{
					_boundsState.type = WrappedElement.BoundsType.Automatic;
					_boundsState.reference = null;
					editorWindow.Close();
				}

				GUILayout.Box(GUIContent.none, _separatorStyle, GUILayout.ExpandWidth(true));

				if (GUILayout.Button(GetStateContent(new WrappedElement.BoundsState(WrappedElement.BoundsType.LocalOrigin)), _itemStyle))
				{
					_boundsState.type = WrappedElement.BoundsType.LocalOrigin;
					_boundsState.reference = null;
					editorWindow.Close();
				}

				if (GUILayout.Button(GetStateContent(new WrappedElement.BoundsState(WrappedElement.BoundsType.Point)), _itemStyle))
				{
					_boundsState.type = WrappedElement.BoundsType.Point;
					_boundsState.reference = null;
					editorWindow.Close();
				}

				if (GUILayout.Button(GetStateContent(new WrappedElement.BoundsState(WrappedElement.BoundsType.Sphere)), _itemStyle))
				{
					_boundsState.type = WrappedElement.BoundsType.Sphere;
					_boundsState.reference = null;
					editorWindow.Close();
				}

				if (GUILayout.Button(GetStateContent(new WrappedElement.BoundsState(WrappedElement.BoundsType.AxisAlignedBox)), _itemStyle))
				{
					_boundsState.type = WrappedElement.BoundsType.AxisAlignedBox;
					_boundsState.reference = null;
					editorWindow.Close();
				}

				var listComponents =
					(_element.wrappedLightElement != null && !ReferenceEquals(_element.wrappedLightElement, _elementComponent)) ||
					(_element.wrappedColliderElement != null && !ReferenceEquals(_element.wrappedColliderElement, _elementComponent)) ||
					(_element.wrappedRigidbodyElement != null && !ReferenceEquals(_element.wrappedRigidbodyElement, _elementComponent));

				if (listComponents)
				{
					GUILayout.Box(GUIContent.none, _separatorStyle, GUILayout.ExpandWidth(true));

					if (_element.wrappedLightElement != null && !ReferenceEquals(_element.wrappedLightElement, _elementComponent))
					{
						if (GUILayout.Button(GetStateContent(new WrappedElement.BoundsState(_element.wrappedLightElement)), _itemStyle))
						{
							_boundsState.type = WrappedElement.BoundsType.Reference;
							_boundsState.reference = _element.wrappedLightElement;
							editorWindow.Close();
						}
					}

					if (_element.wrappedColliderElement != null && !ReferenceEquals(_element.wrappedColliderElement, _elementComponent))
					{
						if (GUILayout.Button(GetStateContent(new WrappedElement.BoundsState(_element.wrappedColliderElement)), _itemStyle))
						{
							_boundsState.type = WrappedElement.BoundsType.Reference;
							_boundsState.reference = _element.wrappedColliderElement;
							editorWindow.Close();
						}
					}

					if (_element.wrappedRigidbodyElement != null && !ReferenceEquals(_element.wrappedRigidbodyElement, _elementComponent))
					{
						if (GUILayout.Button(GetStateContent(new WrappedElement.BoundsState(_element.wrappedRigidbodyElement)), _itemStyle))
						{
							_boundsState.type = WrappedElement.BoundsType.Reference;
							_boundsState.reference = _element.wrappedRigidbodyElement;
							editorWindow.Close();
						}
					}
				}

				editorWindow.Repaint();
			}
		}
	}
}
