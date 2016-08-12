/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using Experilous.MakeIt.Utilities;

namespace Experilous.MakeIt.Wrap
{
	/// <summary>
	/// Enables a specified list of components only after the element is fully within the bounding volume of a wrap-around world.
	/// </summary>
	/// <remarks>
	/// <para>This component is useful when wrapping behavior needs to start off disabled, but once the element has fully entered
	/// within the world's bounding volume, by whatever means, that wrapping behavior ought to be enabled.  This is particularly
	/// convenient for elements that are spawned offscreen, to avoid the possibility of them visibly popping into existence where
	/// the offscreen area wraps back around to another area of the visible viewport.</para>
	/// 
	/// <para>Depending on the method of moving the element, it is possible that it will never fully entery the world's bounding
	/// volume and have its wrapping behavior enabled.  Therefore, a second condition is available for destroying the game object
	/// completely if it ever is completely out of the world's bounding volume and far enough away that it might never return,
	/// given that no wrap-around behavior is enabled to force it to return.</para>
	/// </remarks>
	/// <seealso cref="ConditionalWrapping"/>
	[AddComponentMenu("Wrap-Around Worlds/Conditional Wrapping (Fixed Update)")]
	public class ConditionalWrappingUsingFixedUpdate : MonoBehaviour, IWorldConsumer
	{
		public World world;
		public BoundedElement boundedElement;
		public float enableBuffer = 0f;
		public float destroyBuffer = 0f;

		public MonoBehaviour[] componentsToEnable;

		public bool hasWorld { get { return world != null ; } }
		public World GetWorld() { return world; }
		public void SetWorld(World world) { this.world = world; }

		protected void Start()
		{
			if (world == null) world = WorldConsumerUtility.FindWorld(this);
			this.DisableAndThrowOnUnassignedReference(world, "The ConditionalWrappingUsingFixedUpdate component requires a reference to a World component.");

			if (boundedElement == null) boundedElement = GetComponent<BoundedElement>();
			this.DisableAndThrowOnUnassignedReference(boundedElement, "The ConditionalWrappingUsingFixedUpdate component requires a reference to an BoundedElement component.");
		}

		protected void FixedUpdate()
		{
			if (boundedElement.bounds.ContainedBy(world, transform, enableBuffer))
			{
				foreach (var component in componentsToEnable)
				{
					component.enabled = true;
				}
				enabled = false;
			}
			else if (!boundedElement.bounds.Intersects(world, transform, destroyBuffer))
			{
				Destroy(gameObject);
			}
		}

		public void ResetConditional()
		{
			if (enabled == false)
			{
				foreach (var component in componentsToEnable)
				{
					component.enabled = false;
				}
				enabled = true;
			}
		}
	}
}
