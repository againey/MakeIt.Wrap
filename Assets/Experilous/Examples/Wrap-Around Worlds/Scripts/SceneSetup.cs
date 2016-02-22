using UnityEngine;
using System.Collections.Generic;

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

		protected void Start()
		{
			this.DisableAndThrowOnMissingReference(world, "The Asteroids.SceneSetup component requires a reference to a RhomboidWorld component.");
			this.DisableAndThrowOnMissingReference(viewport, "The Asteroids.SceneSetup component requires a reference to a Viewport component.");
			this.DisableAndThrowOnMissingReference(physicalAsteroidPrefab, "The Asteroids.SceneSetup component requires a reference to a physical asteroid GameObject prefab.");
			this.DisableAndThrowOnMissingReference(virtualAsteroidPrefab, "The Asteroids.SceneSetup component requires a reference to a virtual asteroid GameObject prefab.");
			this.DisableAndThrowOnMissingReference(physicalAsteroidContainer, "The Asteroids.SceneSetup component requires a reference to a physical asteroid Transform root.");
			this.DisableAndThrowOnMissingReference(virtualAsteroidContainer, "The Asteroids.SceneSetup component requires a reference to a virtual asteroid Transform root.");

			Physics.gravity = new Vector3(0f, 0f, 0f);
			Time.timeScale = 0f;

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

					var x = Random.Range(0, 1000000) / 1000000f;
					var y = Random.Range(0, 1000000) / 1000000f;
					var p = world.transform.TransformPoint(world.axis0 * x + world.axis1 * y + world.origin);
					var s = Random.Range(minPhysicalAsteroidScale, maxPhysicalAsteroidScale);
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
					var direction = Random.Range(0, 1000000) / 1000000f * Mathf.PI * 2f;
					var speed = Random.Range(minPhysicalAsteroidSpeed, maxPhysicalAsteroidSpeed);
					rigidBody.velocity = new Vector3(Mathf.Cos(direction) * speed, 0f, Mathf.Sin(direction) * speed);
					rigidBody.angularVelocity = new Vector3(
						Random.Range(minPhysicalAsteroidRotation, maxPhysicalAsteroidRotation),
						Random.Range(minPhysicalAsteroidRotation, maxPhysicalAsteroidRotation),
						Random.Range(minPhysicalAsteroidRotation, maxPhysicalAsteroidRotation));

					var meshRenderer = asteroid.GetComponent<MeshRenderer>();
					meshRenderer.material.color = new Color(
						Random.Range(0f, 1f),
						Random.Range(0f, 1f),
						Random.Range(0f, 1f));

					var collider = asteroid.GetComponent<Collider>();
					if (collider != null) collider.isTrigger = false;
				}
			}

			if (virtualAsteroidPrefab != null)
			{
				for (int i = 0; i < virtualAsteroidCount; ++i)
				{
					var x = Random.Range(0, 1000000) / 1000000f;
					var y = Random.Range(0, 1000000) / 1000000f;
					var p = world.transform.TransformPoint(world.axis0 * x + world.axis1 * y + world.origin);
					var s = Random.Range(minVirtualAsteroidScale, maxVirtualAsteroidScale);
					var definition = new AsteroidDefinition(p, s);

					var asteroid = Instantiate(virtualAsteroidPrefab);
					asteroid.name = string.Format("Virtual Asteroid ({0})", i);
					asteroid.transform.position = definition.position;
					asteroid.transform.localScale = new Vector3(definition.scale, definition.scale, definition.scale);
					asteroid.transform.SetParent(virtualAsteroidContainer, false);

					asteroid.GetComponent<WrapAround.DynamicElementWrapper>().world = world;
					asteroid.GetComponent<WrapAround.RenderableElement>().viewport = viewport;

					var rigidBody = asteroid.GetComponent<Rigidbody>();
					var direction = Random.Range(0, 1000000) / 1000000f * Mathf.PI * 2f;
					var speed = Random.Range(minVirtualAsteroidSpeed, maxVirtualAsteroidSpeed);
					rigidBody.velocity = new Vector3(Mathf.Cos(direction) * speed, 0f, Mathf.Sin(direction) * speed);
					rigidBody.angularVelocity = new Vector3(
						Random.Range(minVirtualAsteroidRotation, maxVirtualAsteroidRotation),
						Random.Range(minVirtualAsteroidRotation, maxVirtualAsteroidRotation),
						Random.Range(minVirtualAsteroidRotation, maxVirtualAsteroidRotation));

					var meshRenderer = asteroid.GetComponent<MeshRenderer>();
					meshRenderer.material.color = new Color(
						Random.Range(0f, 1f),
						Random.Range(0f, 1f),
						Random.Range(0f, 1f));

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
