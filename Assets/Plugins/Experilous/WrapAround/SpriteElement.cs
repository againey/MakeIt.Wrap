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
	public class SpriteElement : GhostableElement<SpriteElement, SpriteElementGhost>, IViewportConsumer
	{
		public Viewport viewport;
		public AbstractBounds bounds;

		public bool hasViewport { get { return viewport != null ; } }
		public void SetViewport(Viewport viewport) { this.viewport = viewport; }

		public virtual bool IsVisible(SpriteElementGhost ghost)
		{
			return bounds.IsVisible(viewport, ghost.transform);
		}

		public virtual bool IsVisible(Vector3 position, Quaternion rotation)
		{
			return bounds.IsVisible(viewport, position, rotation);
		}

		public bool IsVisible(GhostRegion ghostRegion)
		{
			var position = transform.position;
			var rotation = transform.rotation;
			ghostRegion.Transform(ref position, ref rotation);
			return IsVisible(position, rotation);
		}

		protected void Awake()
		{
			if (bounds == null)
			{
				bounds = GetComponent<AbstractBounds>();
				if (bounds == null)
				{
					bounds = gameObject.AddComponent<PointBounds>();
				}
			}
		}

		protected new void Start()
		{
			base.Start();

			if (viewport == null) viewport = ViewportConsumerUtility.FindViewport(this);
			this.DisableAndThrowOnUnassignedReference(viewport, "The SpriteElement component requires a reference to a Viewport component.");
			this.DisableAndThrowOnUnassignedReference(bounds, "The SpriteElement component requires a reference to an AbstractBounds component.");
		}

		protected void LateUpdate()
		{
			foreach (var ghostRegion in viewport.visibleGhostRegions)
			{
				if (FindGhost(ghostRegion) == null && IsVisible(ghostRegion))
				{
					InstantiateGhost(ghostRegion);
				}
			}
		}

		protected void InstantiateGhost(GhostRegion ghostRegion)
		{
			var ghost = Instantiate(ghostPrefab);
			ghost.transform.SetParent(transform.parent, false);
			ghost.name = name + " (Ghost)";
			ghost.region = ghostRegion;
			ghost.original = this;

			ghostRegion.Transform(transform, ghost.transform);

			Add(ghost);
		}
	}
}
