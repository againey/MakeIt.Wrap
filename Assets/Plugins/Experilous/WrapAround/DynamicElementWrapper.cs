using UnityEngine;

namespace Experilous.WrapAround
{
	/// <summary>
	/// Forces the attached dynamic object to always remain confined to the canonical world bounds,
	/// updating once each render frame.
	/// </summary>
	/// <remarks>
	/// Each render frame, this component asks the referenced world to confine its transform to the
	/// world's canonical bounds.  This is appropriate for objects that are frequently moving, but
	/// whose position and orientation only need to be correct for each rendered frame.  If you need
	/// the position and orientation to be correct for each fixed update interval, then see the similar
	/// <see cref="DynamicElementFixedUpdateWrapper"/> class.  Or if this element is a rigidbody,
	/// see the <see cref="RigidbodyElementWrapper"/> class.
	/// </remarks>
	/// <seealso cref="World"/>
	/// <seealso cref="IWorldConsumer"/>
	/// <seealso cref="DynamicElementFixedUpdateWrapper"/>
	/// <seealso cref="RigidbodyElementWrapper"/>
	public class DynamicElementWrapper : MonoBehaviour, IWorldConsumer
	{
		public World world;

		public bool hasWorld { get { return world != null ; } }
		public void SetWorld(World world) { this.world = world; }

		protected void Start()
		{
			if (world == null) world = WorldConsumerUtility.FindWorld(this);
			this.DisableAndThrowOnMissingReference(world, "The DynamicElementWrapper component requires a reference to a World component.");
		}

		protected void LateUpdate()
		{
			world.Confine(transform);
		}
	}
}
