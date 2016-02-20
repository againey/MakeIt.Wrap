using UnityEngine;

namespace Experilous.WrapAround
{
	/// <summary>
	/// A wrap-around world element that is a light source which might illuminate objects
	/// across wrapped world boundaries, or which might cast shadows that wrap across world
	/// boundaries.
	/// </summary>
	/// <remarks>
	/// Attach this component to a game object with a light source whenever you want it to
	/// illuminate light receiving objects across wrapped world boundaries, or whenever you
	/// want the shadows it produces to be cast across wrapped world boundaries.  This
	/// component will create ghosts of itself at the opposite end(s) of the world which
	/// become an additional light source in the scene, producing illumination and shadows
	/// that the original object in its canonical location would not produce since the
	/// standard rendering engine cannot handle the wrapped world boundaries.
	/// 
	/// The ghost prefab should include only the light source component, as well as any
	/// descendants with light sources that should also be applied, but all other components
	/// should probably be absent from the ghost prefab.
	/// 
	/// This component is completely unnecessary for directional light sources when used
	/// with the most common variety of wrapping world in which all wrapping transformations
	/// involve translation only, with no rotation involved.
	/// 
	/// This component currently is only expected to work with dynamic lights.
	/// </remarks>
	/// <seealso cref="Viewport"/>
	/// <seealso cref="AbstractBounds"/>
	/// <seealso cref="GhostRegion"/>
	/// <seealso cref="ViewportProvider"/>
	/// <seealso cref="IViewportConsumer"/>
	/// <seealso cref="GhostableElement`2{TDerivedElement,TGhost}"/>
	/// <seealso cref="LightElementGhost"/>
	/// <seealso cref="Light"/>
	public class LightElement : GhostableElement<LightElement, LightElementGhost>, IViewportConsumer
	{
		public Viewport viewport;
		public AbstractBounds bounds;

		public bool hasViewport { get { return viewport != null ; } }
		public void SetViewport(Viewport viewport) { this.viewport = viewport; }

		public virtual bool IsVisible(LightElementGhost ghost)
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
			this.DisableAndThrowOnMissingReference(viewport, "The LightElement component requires a reference to a Viewport component.");
			this.DisableAndThrowOnMissingReference(bounds, "The LightElement component requires a reference to an AbstractBounds component.");
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
