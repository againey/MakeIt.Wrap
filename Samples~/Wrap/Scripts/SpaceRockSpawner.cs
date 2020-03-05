/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using MakeIt.Wrap;

namespace MakeIt.Wrap.Samples
{
	public abstract class SpaceRockSpawner : MonoBehaviour
	{
		[System.Serializable]
		public struct SpaceRockType
		{
			public SpaceRock prefab;
			public float randomSpawnWeight;
			public float minSpeed;
			public float maxSpeed;
			public float minRotationSpeed;
			public float maxRotationSpeed;
		}

		public SpaceRockType[] spaceRockTypes;

		protected abstract void SetPositionAndMovement(SpaceRock rock, float movementSpeed);

		protected SpaceRock SpawnRock(Transform container, float maxSignificance)
		{
			float randomSpawnWeightRemaining = 0f;
			foreach (var spaceRockType in spaceRockTypes)
			{
				randomSpawnWeightRemaining += spaceRockType.randomSpawnWeight;
			}

			int randomIndex;
			for (randomIndex = 0; randomIndex < spaceRockTypes.Length - 1; ++randomIndex)
			{
				if (UnityEngine.Random.Range(0f, randomSpawnWeightRemaining) < spaceRockTypes[randomIndex].randomSpawnWeight && spaceRockTypes[randomIndex].prefab.significance <= maxSignificance)
				{
					break;
				}

				randomSpawnWeightRemaining -= spaceRockTypes[randomIndex].randomSpawnWeight;
			}

			return SpawnRock(container, spaceRockTypes[randomIndex]);
		}

		protected SpaceRock SpawnRock(Transform container, SpaceRockType spaceRockType)
		{
			var rock = Instantiate(spaceRockType.prefab);

			rock.transform.SetParent(container, false);

			var rotationSpeed = UnityEngine.Random.value * (spaceRockType.maxRotationSpeed - spaceRockType.minRotationSpeed) + spaceRockType.minRotationSpeed;
			rotationSpeed *= UnityEngine.Random.Range(0, 2) == 0 ? -1 : +1;

			var movementSpeed = UnityEngine.Random.Range(spaceRockType.minSpeed, spaceRockType.maxSpeed);
			rock.GetComponent<Rigidbody2D>().angularVelocity = rotationSpeed;

			rock.GetComponent<Rigidbody2DElement>().RefreshBounds();
			rock.GetComponent<SpriteElement>().RefreshBounds();

			SetPositionAndMovement(rock, movementSpeed);

			return rock;
		}
	}
}
