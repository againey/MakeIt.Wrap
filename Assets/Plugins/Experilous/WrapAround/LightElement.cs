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
	/// A wrap-around world element that is a light source which might illuminate objects
	/// across wrapped world boundaries, or which might cast shadows that wrap across world
	/// boundaries.
	/// </summary>
	/// <remarks>
	/// <para>Attach this component to a game object with a light source whenever you want
	/// it to illuminate light receiving objects across wrapped world boundaries, or whenever
	/// you want the shadows it produces to be cast across wrapped world boundaries.  This
	/// component will create ghosts of itself at the opposite end(s) of the world which
	/// become an additional light source in the scene, producing illumination and shadows
	/// that the original object in its canonical location would not produce since the
	/// standard rendering engine cannot handle the wrapped world boundaries.</para>
	/// 
	/// <para>The ghost prefab should include only the light source component, as well as any
	/// descendants with light sources that should also be applied, but all other components
	/// should probably be absent from the ghost prefab.</para>
	/// 
	/// <para>This component is completely unnecessary for directional light sources when used
	/// with the most common variety of wrapping world in which all wrapping transformations
	/// involve translation only, with no rotation involved.</para>
	/// 
	/// <para>This component currently only works with dynamic lights.</para>
	/// </remarks>
	/// <seealso cref="Viewport"/>
	/// <seealso cref="ElementBounds"/>
	/// <seealso cref="GhostRegion"/>
	/// <seealso cref="ViewportProvider"/>
	/// <seealso cref="IViewportConsumer"/>
	/// <seealso cref="GhostableElement`2{TDerivedElement,TGhost}"/>
	/// <seealso cref="LightElementGhost"/>
	/// <seealso cref="Light"/>
	public class LightElement : GhostableElement<LightElement, LightElementGhost>, IViewportConsumer
	{
		public Viewport viewport;
		public ElementBoundsSource boundsSource = ElementBoundsSource.FixedScale | ElementBoundsSource.Automatic;
		public ElementBoundsProvider boundsProvider;

		public bool hasViewport { get { return viewport != null ; } }
		public Viewport GetViewport() { return viewport; }
		public void SetViewport(Viewport viewport) { this.viewport = viewport; }

		protected new void Start()
		{
			base.Start();

			if (viewport == null) viewport = ViewportConsumerUtility.FindViewport(this);
			this.DisableAndThrowOnUnassignedReference(viewport, "The LightElement component requires a reference to a Viewport component.");
		}

		protected void LateUpdate()
		{
			foreach (var ghostRegion in viewport.visibleGhostRegions)
			{
				if (FindGhost(ghostRegion) == null && _bounds.IsVisible(viewport, transform, ghostRegion))
				{
					InstantiateGhost(ghostRegion);
				}
			}
		}

		public override void RefreshBounds()
		{
			_bounds = ElementBounds.CreateBounds(boundsSource, boundsProvider, transform,
				() => { return HierarchyUtility.GetLightGroupAxisAlignedBoxBounds(transform); },
				() => { return HierarchyUtility.GetLightGroupSphereBounds(transform); });

#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) UnityEditor.SceneView.RepaintAll();
#endif
		}

		protected void InstantiateGhost(GhostRegion ghostRegion)
		{
			var ghost = Instantiate(ghostPrefab);
			ghost.transform.SetParent(transform.parent, false);
			ghost.name = name + " (Light Ghost)";
			ghost.region = ghostRegion;
			ghost.original = this;

			ghostRegion.Transform(transform, ghost.transform);

			Add(ghost);
		}

		protected override bool IsGameObjectExcludedFromGhost(Component[] components)
		{
			return false;
		}

		protected override bool IsGameObjectNecessaryForGhost(Component[] components)
		{
			foreach (var component in components)
			{
				if (component is Light)
				{
					return true;
				}
			}
			return false;
		}

		protected override void RemoveUnnecessaryComponentsFromGhost(Component[] components)
		{
			RemoveMatchingComponentsFromGhost(components, (Component component) => { return !(component is Light || component is Transform); });
		}
	}
}
