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
	public class ConditionalWrapping : MonoBehaviour, IWorldConsumer
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
			this.DisableAndThrowOnUnassignedReference(world, "The ConditionalWrapping component requires a reference to a World component.");

			if (boundedElement == null) boundedElement = GetComponent<BoundedElement>();
			this.DisableAndThrowOnUnassignedReference(boundedElement, "The ConditionalWrapping component requires a reference to an BoundedElement component.");
		}

		protected void Update()
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
