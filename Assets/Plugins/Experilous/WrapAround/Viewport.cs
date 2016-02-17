using UnityEngine;
using System.Collections.Generic;

namespace Experilous.WrapAround
{
	/// <summary>
	/// An abstract representation of a view onto a world with wrap-around behavior.
	/// </summary>
	/// <seealso cref="World"/>
	/// <seealso cref="GhostRegion"/>
	/// <seealso cref="ViewportProvider"/>
	/// <seealso cref="IViewportConsumer"/>
	/// <seealso cref="IWorldConsumer"/>
	public abstract class Viewport : MonoBehaviour, IWorldConsumer
	{
		/// <summary>
		/// The world that the viewport is observing.
		/// </summary>
		public World world;

		/// <summary>
		/// The list of ghost regions for the associated world that are potentially visible through this viewport.
		/// </summary>
		public abstract IEnumerable<GhostRegion> visibleGhostRegions { get; }

		/// <summary>
		/// Recalculates and caches the list of ghost regions for the associated world that are potentially visible
		/// through this viewport, given its current configuration.
		/// </summary>
		public abstract void RecalculateVisibleGhostRegions();

		#region IsVisible

		// Various overloads for testing if an object is potentially visible through the viewport.
		// These various overloads represent the second phase of a double-dispatch scheme between
		// any world element type (or delegated bound type) and any viewport type.  The first phase
		// starts at the element, dependent on the information that it has regarding its position
		// and orientation in space.  The element passes this information on to the viewport, without
		// needing to know the viewport's type or how the viewport does the visibility check.

		// It is expected and normal to add new overloads to this list if new element types are
		// added that have information that cannot be efficiently transformed into the parameters
		// provided by the already existing overloads.  However, every viewport type will then need
		// to be updated to understand and perform visibility checks with these new overloads.

		/// <summary>
		/// Returns whether an object at the specified position could possibly be visible when the world
		/// is observed through this viewport with its current configuration.
		/// </summary>
		/// <param name="position">The position of the object.</param>
		/// <returns>Returns <c>true</c> is the object is potentially visible through this viewport; <c>false</c> otherwise.</returns>
		public abstract bool IsVisible(Vector3 position);

		/// <summary>
		/// Returns whether an object at the specified position and with the specified bounding radius could
		/// possibly be visible when the world is observed through this viewport with its current configuration.
		/// </summary>
		/// <param name="position">The position of the object.</param>
		/// <param name="radius">The radius of the object's bounding sphere.</param>
		/// <returns>Returns <c>true</c> is the object is potentially visible through this viewport; <c>false</c> otherwise.</returns>
		public abstract bool IsVisible(Vector3 position, float radius);

		/// <summary>
		/// Returns whether an object with the specified axis aligned bounding box could possibly be visible
		/// when the world is observed through this viewport with its current configuration.
		/// </summary>
		/// <param name="box">The axis aligned bounding box of the object.</param>
		/// <returns>Returns <c>true</c> is the object is potentially visible through this viewport; <c>false</c> otherwise.</returns>
		public abstract bool IsVisible(Bounds box);

		/// <summary>
		/// Returns whether an object with the specified axis aligned bounding box centered at the specified
		/// position could possibly be visible when the world is observed through this viewport with its
		/// current configuration.
		/// </summary>
		/// <param name="position">The position of the object.</param>
		/// <param name="box">The untranslated axis aligned bounding box of the object.</param>
		/// <returns>Returns <c>true</c> is the object is potentially visible through this viewport; <c>false</c> otherwise.</returns>
		public abstract bool IsVisible(Vector3 position, Bounds box);

		#endregion

		/// <summary>
		/// Indicates if this <see cref="IWorldConsumer"/> has been assigned a world.
		/// </summary>
		public bool hasWorld { get { return world != null ; } }

		/// <summary>
		/// Assigns a world to this <see cref="IWorldConsumer"/>.
		/// </summary>
		/// <param name="world">The world to assign.</param>
		public void SetWorld(World world) { this.world = world; }

		protected void Start()
		{
			if (world == null) world = WorldConsumerUtility.FindWorld(this);
		}
	}
}
