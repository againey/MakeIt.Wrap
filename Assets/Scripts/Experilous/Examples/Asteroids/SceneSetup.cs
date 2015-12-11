using UnityEngine;
using System.Collections.Generic;

namespace Experilous.Examples.Asteroids
{
	public class SceneSetup : MonoBehaviour
	{
		public WrapAround.AxisAlignedWrapXY2DWorld World;
		public int RandomSeed = 0;

		[Header("Physical Asteroids")]
		public Asteroid PhysicalAsteroidPrefab;
		public int PhysicalAsteroidCount = 10;
		public float MinimumPhysicalAsteroidScale = 1.0f;
		public float MaximumPhysicalAsteroidScale = 2.0f;
		public float MinimumPhysicalAsteroidSpeed = 1.0f;
		public float MaximumPhysicalAsteroidSpeed = 2.0f;
		public float MinimumPhysicalAsteroidRotation = -90.0f;
		public float MaximumPhysicalAsteroidRotation = +90.0f;
		public Transform PhysicalAsteroidContainer;

		[Header("Virtual Asteroids")]
		public Asteroid VirtualAsteroidPrefab;
		public int VirtualAsteroidCount = 10;
		public float MinimumVirtualAsteroidScale = 1.0f;
		public float MaximumVirtualAsteroidScale = 2.0f;
		public float MinimumVirtualAsteroidSpeed = 1.0f;
		public float MaximumVirtualAsteroidSpeed = 2.0f;
		public float MinimumVirtualAsteroidRotation = -90.0f;
		public float MaximumVirtualAsteroidRotation = +90.0f;
		public Transform VirtualAsteroidContainer;

		private struct AsteroidDefinition
		{
			public Vector2 position;
			public float scale;

			public AsteroidDefinition(Vector2 p, float s)
			{
				position = p;
				scale = s;
			}
		}

		protected void Awake()
		{
			Physics.gravity = new Vector3(0f, 0f, 0f);
			Time.timeScale = 0f;

			var randomEngine = new NativeRandomEngine(RandomSeed);
			List<AsteroidDefinition> asteroids = new List<AsteroidDefinition>();

			float prefabRadius = 0;
			var sphereCollider = PhysicalAsteroidPrefab.GetComponent<SphereCollider>();
			if (sphereCollider != null)
			{
				prefabRadius = sphereCollider.radius;
			}
			else
			{
				var boxCollider = PhysicalAsteroidPrefab.GetComponent<BoxCollider>();
				if (boxCollider != null)
				{
					prefabRadius = boxCollider.size.magnitude * 0.5f;
				}
			}

			int tries = 0;
			while (asteroids.Count < PhysicalAsteroidCount && tries < PhysicalAsteroidCount * 10)
			{
				++tries;

				var x = Random.HalfOpenRange(World.minX, World.maxX, randomEngine);
				var y = Random.HalfOpenRange(World.minY, World.maxY, randomEngine);
				var s = Random.ClosedRange(MinimumPhysicalAsteroidScale, MaximumPhysicalAsteroidScale, randomEngine);
				var asteroid = new AsteroidDefinition(new Vector2(x, y), s);

				bool collisionFound = false;
				foreach (var existingAsteroid in asteroids)
				{
					var distance = (asteroid.position - existingAsteroid.position).magnitude;
					var combinedRadius = prefabRadius * (asteroid.scale + existingAsteroid.scale) + 0.01f;
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
				var asteroid = Instantiate(PhysicalAsteroidPrefab);
				asteroid.name = string.Format("Physical Asteroid ({0})", i);
				asteroid.transform.position = new Vector3(definition.position.x, definition.position.y, 0f);
				asteroid.transform.localScale = new Vector3(definition.scale, definition.scale, definition.scale);
				asteroid.transform.SetParent(PhysicalAsteroidContainer, false);
				asteroid.World = World;
				asteroid.InteractsAcrossEdges = true;

				var rigidBody = asteroid.GetComponent<Rigidbody>();
				var direction = Random.UnitVector2(randomEngine);
				var speed = Random.ClosedRange(MinimumPhysicalAsteroidSpeed, MaximumPhysicalAsteroidSpeed, randomEngine);
				rigidBody.velocity = new Vector3(direction.x * speed, direction.y * speed, 0f);
				rigidBody.angularVelocity = new Vector3(
					Random.ClosedRange(MinimumPhysicalAsteroidRotation, MaximumPhysicalAsteroidRotation, randomEngine),
					Random.ClosedRange(MinimumPhysicalAsteroidRotation, MaximumPhysicalAsteroidRotation, randomEngine),
					Random.ClosedRange(MinimumPhysicalAsteroidRotation, MaximumPhysicalAsteroidRotation, randomEngine));

				var meshRenderer = asteroid.GetComponent<MeshRenderer>();
				meshRenderer.material.color = new Color(
					Random.ClosedFloatUnit(randomEngine),
					Random.ClosedFloatUnit(randomEngine),
					Random.ClosedFloatUnit(randomEngine));

				var collider = asteroid.GetComponent<Collider>();
				if (collider != null) collider.isTrigger = false;
			}

			for (int i = 0; i < VirtualAsteroidCount; ++i)
			{
				var x = Random.HalfOpenRange(World.minX, World.maxX, randomEngine);
				var y = Random.HalfOpenRange(World.minY, World.maxY, randomEngine);
				var s = Random.ClosedRange(MinimumVirtualAsteroidScale, MaximumVirtualAsteroidScale, randomEngine);
				var definition = new AsteroidDefinition(new Vector2(x, y), s);

				var asteroid = Instantiate(VirtualAsteroidPrefab);
				asteroid.name = string.Format("Virtual Asteroid ({0})", i);
				asteroid.transform.position = new Vector3(definition.position.x, definition.position.y, 0f);
				asteroid.transform.localScale = new Vector3(definition.scale, definition.scale, definition.scale);
				asteroid.transform.SetParent(VirtualAsteroidContainer, false);
				asteroid.World = World;
				asteroid.InteractsAcrossEdges = false;

				var rigidBody = asteroid.GetComponent<Rigidbody>();
				var direction = Random.UnitVector2(randomEngine);
				var speed = Random.ClosedRange(MinimumVirtualAsteroidSpeed, MaximumVirtualAsteroidSpeed, randomEngine);
				rigidBody.velocity = new Vector3(direction.x * speed, direction.y * speed, 0f);
				rigidBody.angularVelocity = new Vector3(
					Random.ClosedRange(MinimumVirtualAsteroidRotation, MaximumVirtualAsteroidRotation, randomEngine),
					Random.ClosedRange(MinimumVirtualAsteroidRotation, MaximumVirtualAsteroidRotation, randomEngine),
					Random.ClosedRange(MinimumVirtualAsteroidRotation, MaximumVirtualAsteroidRotation, randomEngine));

				var meshRenderer = asteroid.GetComponent<MeshRenderer>();
				meshRenderer.material.color = new Color(
					Random.ClosedFloatUnit(randomEngine),
					Random.ClosedFloatUnit(randomEngine),
					Random.ClosedFloatUnit(randomEngine));

				var collider = asteroid.GetComponent<Collider>();
				if (collider != null) collider.isTrigger = true;
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
