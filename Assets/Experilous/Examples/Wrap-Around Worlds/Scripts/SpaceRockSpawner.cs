/******************************************************************************\
 *  Copyright (C) 2016 Experilous <againey@experilous.com>
 *  
 *  This file is subject to the terms and conditions defined in the file
 *  'Assets/Plugins/Experilous/License.txt', which is a part of this package.
 *
\******************************************************************************/

using UnityEngine;
using Experilous.WrapAround;

namespace Experilous.Examples.WrapAround
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
				if (Random.Range(0f, randomSpawnWeightRemaining) < spaceRockTypes[randomIndex].randomSpawnWeight && spaceRockTypes[randomIndex].prefab.significance <= maxSignificance)
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

			var rotationSpeed = Random.value * (spaceRockType.maxRotationSpeed - spaceRockType.minRotationSpeed) + spaceRockType.minRotationSpeed;
			rotationSpeed *= Random.Range(0, 2) == 0 ? -1 : +1;

			var movementSpeed = Random.Range(spaceRockType.minSpeed, spaceRockType.maxSpeed);
			rock.GetComponent<Rigidbody2D>().angularVelocity = rotationSpeed;

			rock.GetComponent<Rigidbody2DElement>().RefreshBounds();
			rock.GetComponent<SpriteElement>().RefreshBounds();

			SetPositionAndMovement(rock, movementSpeed);

			return rock;
		}
	}
}
