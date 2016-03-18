/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using Experilous.WrapAround;
using System;

namespace Experilous.Examples.WrapAround
{
	public class SpaceRock : MonoBehaviour, IWorldConsumer
	{
		public CameraWorld world;

		public float significance = 1f;
		public float breakupSignificanceFraction = 1f;

		public bool hasWorld { get { return world != null; } }
		public World GetWorld() { return world; }
		public void SetWorld(World world) { if (world is CameraWorld) this.world = (CameraWorld)world; }

		protected void Start()
		{
			if (world == null) world = WorldConsumerUtility.FindWorld<CameraWorld>(this);
		}

		protected void OnTriggerEnter2D(Collider2D collider)
		{
			var original = collider.gameObject.GetGhostableComponent<Rigidbody2DElement, Rigidbody2DElementGhost>();
			if (original != null)
			{
				var ship = original.GetComponent<ShipController>();
				if (ship != null)
				{
					ship.Die();
					Breakup();
				}
				else
				{
					var laser = original.GetComponent<Laser>();
					if (laser != null)
					{
						Destroy(laser.gameObject);
						Breakup();
					}
				}
			}
		}

		protected void Breakup()
		{
			var spawner = GetComponent<BreakupSpaceRockSpawner>();
			if (spawner != null && spawner.spaceRockTypes.Length > 0)
			{
				spawner.Breakup(significance * breakupSignificanceFraction);
			}

			Destroy(gameObject);
		}
	}
}
