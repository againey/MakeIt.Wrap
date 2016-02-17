using UnityEngine;
using System.Collections.Generic;
using Experilous.Randomization;

namespace Experilous.Examples.Asteroids
{
	public class SceneSetup : MonoBehaviour
	{
		public WrapAround.RhomboidWorld world;
		public WrapAround.Viewport viewport;
		public int randomSeed = 0;

		[Header("Physical Asteroids")]
		public GameObject physicalAsteroidPrefab;
		public int physicalAsteroidCount = 10;
		public float minPhysicalAsteroidScale = 1.0f;
		public float maxPhysicalAsteroidScale = 2.0f;
		public float minPhysicalAsteroidSpeed = 1.0f;
		public float maxPhysicalAsteroidSpeed = 2.0f;
		public float minPhysicalAsteroidRotation = -90.0f;
		public float maxPhysicalAsteroidRotation = +90.0f;
		public Transform physicalAsteroidContainer;

		[Header("Virtual Asteroids")]
		public GameObject virtualAsteroidPrefab;
		public int virtualAsteroidCount = 10;
		public float minVirtualAsteroidScale = 1.0f;
		public float maxVirtualAsteroidScale = 2.0f;
		public float minVirtualAsteroidSpeed = 1.0f;
		public float maxVirtualAsteroidSpeed = 2.0f;
		public float minVirtualAsteroidRotation = -90.0f;
		public float maxVirtualAsteroidRotation = +90.0f;
		public Transform virtualAsteroidContainer;

		private struct AsteroidDefinition
		{
			public Vector3 position;
			public float scale;

			public AsteroidDefinition(Vector3 p, float s)
			{
				position = p;
				scale = s;
			}
		}

		protected void Awake()
		{
			Physics.gravity = new Vector3(0f, 0f, 0f);
			Time.timeScale = 0f;

			var randomEngine = NativeRandomEngine.Create(randomSeed);
			List<AsteroidDefinition> asteroids = new List<AsteroidDefinition>();

			if (physicalAsteroidPrefab != null)
			{
				float physicalPrefabRadius = 0;
				var sphereCollider = physicalAsteroidPrefab.GetComponent<SphereCollider>();
				if (sphereCollider != null)
				{
					physicalPrefabRadius = sphereCollider.radius;
				}
				else
				{
					var boxCollider = physicalAsteroidPrefab.GetComponent<BoxCollider>();
					if (boxCollider != null)
					{
						physicalPrefabRadius = boxCollider.size.magnitude * 0.5f;
					}
				}

				world.maxPhysicsObjectRadius = physicalPrefabRadius * maxPhysicalAsteroidScale;

				float virtualPrefabRadius = 0;
				sphereCollider = virtualAsteroidPrefab.GetComponent<SphereCollider>();
				if (sphereCollider != null)
				{
					virtualPrefabRadius = sphereCollider.radius;
				}
				else
				{
					var boxCollider = virtualAsteroidPrefab.GetComponent<BoxCollider>();
					if (boxCollider != null)
					{
						virtualPrefabRadius = boxCollider.size.magnitude * 0.5f;
					}
				}

				world.maxVisibleObjectRadius = Mathf.Max(world.maxPhysicsObjectRadius, virtualPrefabRadius * maxVirtualAsteroidScale);

				int tries = 0;
				while (asteroids.Count < physicalAsteroidCount && tries < physicalAsteroidCount * 10)
				{
					++tries;

					var x = RandomUtility.HalfOpenRange(0f, 1f, randomEngine);
					var y = RandomUtility.HalfOpenRange(0f, 1f, randomEngine);
					var p = world.transform.TransformPoint(world.axis0 * x + world.axis1 * y + world.origin);
					var s = RandomUtility.ClosedRange(minPhysicalAsteroidScale, maxPhysicalAsteroidScale, randomEngine);
					var asteroid = new AsteroidDefinition(p, s);

					bool collisionFound = false;
					foreach (var existingAsteroid in asteroids)
					{
						var distance = (asteroid.position - existingAsteroid.position).magnitude;
						var combinedRadius = physicalPrefabRadius * (asteroid.scale + existingAsteroid.scale) + 0.01f;
						if (distance < combinedRadius)
						{
							collisionFound = true;
							break;
						}
					}

					if (collisionFound) continue;

					asteroids.Add(asteroid);
				}

				for (int i = 0; i < asteroids.Count; ++i)
				{
					var definition = asteroids[i];
					var asteroid = Instantiate(physicalAsteroidPrefab);
					asteroid.name = string.Format("Physical Asteroid ({0})", i);
					asteroid.transform.position = definition.position;
					asteroid.transform.localScale = new Vector3(definition.scale, definition.scale, definition.scale);
					asteroid.transform.SetParent(physicalAsteroidContainer, false);

					asteroid.GetComponent<WrapAround.RigidbodyElementWrapper>().world = world;
					asteroid.GetComponent<WrapAround.RigidbodyElement>().world = world;
					asteroid.GetComponent<WrapAround.RenderableElement>().viewport = viewport;

					var rigidBody = asteroid.GetComponent<Rigidbody>();
					var direction = RandomUtility.UnitVector2(randomEngine);
					var speed = RandomUtility.ClosedRange(minPhysicalAsteroidSpeed, maxPhysicalAsteroidSpeed, randomEngine);
					rigidBody.velocity = new Vector3(direction.x * speed, direction.y * speed, 0f);
					rigidBody.angularVelocity = new Vector3(
						RandomUtility.ClosedRange(minPhysicalAsteroidRotation, maxPhysicalAsteroidRotation, randomEngine),
						RandomUtility.ClosedRange(minPhysicalAsteroidRotation, maxPhysicalAsteroidRotation, randomEngine),
						RandomUtility.ClosedRange(minPhysicalAsteroidRotation, maxPhysicalAsteroidRotation, randomEngine));

					var meshRenderer = asteroid.GetComponent<MeshRenderer>();
					meshRenderer.material.color = new Color(
						RandomUtility.ClosedFloatUnit(randomEngine),
						RandomUtility.ClosedFloatUnit(randomEngine),
						RandomUtility.ClosedFloatUnit(randomEngine));

					var collider = asteroid.GetComponent<Collider>();
					if (collider != null) collider.isTrigger = false;
				}
			}

			if (virtualAsteroidPrefab != null)
			{
				for (int i = 0; i < virtualAsteroidCount; ++i)
				{
					var x = RandomUtility.HalfOpenRange(0f, 1f, randomEngine);
					var y = RandomUtility.HalfOpenRange(0f, 1f, randomEngine);
					var p = world.transform.TransformPoint(world.axis0 * x + world.axis1 * y + world.origin);
					var s = RandomUtility.ClosedRange(minVirtualAsteroidScale, maxVirtualAsteroidScale, randomEngine);
					var definition = new AsteroidDefinition(p, s);

					var asteroid = Instantiate(virtualAsteroidPrefab);
					asteroid.name = string.Format("Virtual Asteroid ({0})", i);
					asteroid.transform.position = definition.position;
					asteroid.transform.localScale = new Vector3(definition.scale, definition.scale, definition.scale);
					asteroid.transform.SetParent(virtualAsteroidContainer, false);

					asteroid.GetComponent<WrapAround.DynamicElementWrapper>().world = world;
					asteroid.GetComponent<WrapAround.RenderableElement>().viewport = viewport;

					var rigidBody = asteroid.GetComponent<Rigidbody>();
					var direction = RandomUtility.UnitVector2(randomEngine);
					var speed = RandomUtility.ClosedRange(minVirtualAsteroidSpeed, maxVirtualAsteroidSpeed, randomEngine);
					rigidBody.velocity = new Vector3(direction.x * speed, direction.y * speed, 0f);
					rigidBody.angularVelocity = new Vector3(
						RandomUtility.ClosedRange(minVirtualAsteroidRotation, maxVirtualAsteroidRotation, randomEngine),
						RandomUtility.ClosedRange(minVirtualAsteroidRotation, maxVirtualAsteroidRotation, randomEngine),
						RandomUtility.ClosedRange(minVirtualAsteroidRotation, maxVirtualAsteroidRotation, randomEngine));

					var meshRenderer = asteroid.GetComponent<MeshRenderer>();
					meshRenderer.material.color = new Color(
						RandomUtility.ClosedFloatUnit(randomEngine),
						RandomUtility.ClosedFloatUnit(randomEngine),
						RandomUtility.ClosedFloatUnit(randomEngine));

					var collider = asteroid.GetComponent<Collider>();
					if (collider != null) collider.isTrigger = true;
				}
			}
		}

		protected void Update()
		{
			if (Time.timeScale == 0f && Time.realtimeSinceStartup > 2f)
			{
				Time.timeScale = 1f;
			}
		}
	}
}
