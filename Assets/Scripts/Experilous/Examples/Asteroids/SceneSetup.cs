using UnityEngine;
using System.Collections.Generic;

namespace Experilous.Examples.Asteroids
{
	public class SceneSetup : MonoBehaviour
	{
		public WrapAround.AxisAlignedWrapXY2DWorld World;
		public Asteroid AsteroidPrefab;
		public int AsteroidCount = 10;
		public float MinimumAsteroidScale = 1.0f;
		public float MaximumAsteroidScale = 2.0f;
		public float MinimumAsteroidSpeed = 1.0f;
		public float MaximumAsteroidSpeed = 2.0f;
		public Transform AsteroidContainer;

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

			var randomEngine = new NativeRandomEngine(1);
			List<AsteroidDefinition> asteroids = new List<AsteroidDefinition>();

			var prefabRadius = AsteroidPrefab.GetComponent<SphereCollider>().radius;

			int tries = 0;
			while (asteroids.Count < AsteroidCount && tries < AsteroidCount * 10)
			{
				++tries;

				var x = Random.HalfOpenRange(World.minX, World.maxX, randomEngine);
				var y = Random.HalfOpenRange(World.minY, World.maxY, randomEngine);
				var s = Random.ClosedRange(MinimumAsteroidScale, MaximumAsteroidScale, randomEngine);
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
				var asteroid = Instantiate(AsteroidPrefab);
				asteroid.name = string.Format("Asteroid ({0})", i);
				asteroid.transform.position = new Vector3(definition.position.x, definition.position.y, 0f);
				asteroid.transform.localScale = new Vector3(definition.scale, definition.scale, definition.scale);
				asteroid.transform.SetParent(AsteroidContainer, false);
				asteroid.World = World;

				var rigidBody = asteroid.GetComponent<Rigidbody>();
				var direction = Random.UnitVector2(randomEngine);
				var speed = Random.ClosedRange(MinimumAsteroidSpeed, MaximumAsteroidSpeed, randomEngine);
				rigidBody.velocity = new Vector3(direction.x * speed, direction.y * speed, 0f);

				var meshRenderer = asteroid.GetComponent<MeshRenderer>();
				meshRenderer.material.color = new Color(
					Random.ClosedFloatUnit(randomEngine),
					Random.ClosedFloatUnit(randomEngine),
					Random.ClosedFloatUnit(randomEngine));
			}
		}
	}
}
