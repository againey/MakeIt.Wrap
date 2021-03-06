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
using MakeIt.Core;

namespace MakeIt.Wrap
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
	/// <seealso cref="GhostableElement{TDerivedElement,TGhost}"/>
	/// <seealso cref="LightElementGhost"/>
	/// <seealso cref="Light"/>
	[AddComponentMenu("MakeIt.Wrap/Elements/Light")]
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
				() => { return GameObjectHierarchy.GetLightGroupAxisAlignedBoxBounds(transform); },
				() => { return GameObjectHierarchy.GetLightGroupSphereBounds(transform); });

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
