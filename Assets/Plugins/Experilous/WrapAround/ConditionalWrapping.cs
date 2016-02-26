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
		public AbstractBounds bounds;
		public float enableBuffer = 0f;
		public float destroyBuffer = 0f;

		public MonoBehaviour[] componentsToEnable;

		public bool hasWorld { get { return world != null ; } }
		public void SetWorld(World world) { this.world = world; }

		protected void Start()
		{
			if (world == null) world = WorldConsumerUtility.FindWorld(this);
			this.DisableAndThrowOnUnassignedReference(world, "The ConditionalWrapping component requires a reference to a World component.");

			if (bounds == null) bounds = GetComponent<AbstractBounds>();
			this.DisableAndThrowOnUnassignedReference(bounds, "The ConditionalWrapping component requires a reference to an AbstractBounds component.");
		}

		protected void Update()
		{
			if (bounds.ContainedBy(world, enableBuffer))
			{
				foreach (var component in componentsToEnable)
				{
					component.enabled = true;
				}
				enabled = false;
			}
			else if (!bounds.Intersects(world, destroyBuffer))
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
