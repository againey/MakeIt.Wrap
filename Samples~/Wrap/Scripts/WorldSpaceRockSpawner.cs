/******************************************************************************\
* Copyright Andy Gainey                                                        *
*                                                                              *
* Licensed under the Apache License, Version 2.0 (the "License");              *
* you may not use this file except in compliance with the License.             *
* You may obtain a copy of the License at                                      *
*                                                                              *
*     http://www.apache.org/licenses/LICENSE-2.0                               *
*                                                                              *
* Unless required by applicable law or agreed to in writing, software          *
* distributed under the License is distributed on an "AS IS" BASIS,            *
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.     *
* See the License for the specific language governing permissions and          *
* limitations under the License.                                               *
\******************************************************************************/

using UnityEngine;
using MakeIt.Wrap;
using MakeIt.Numerics;

namespace MakeIt.Wrap.Samples
{
	public class WorldSpaceRockSpawner : SpaceRockSpawner
	{
		public CameraWorld world;
		public Transform spaceRockContainer;

		public float targetSignificanceTotal = 1f;
		public float spawnCooldownDuration = 1f;

		private float _spawnCooldown;

		protected void Start()
		{
			_spawnCooldown = spawnCooldownDuration;
		}

		protected void FixedUpdate()
		{
			_spawnCooldown = Mathf.Max(0f, _spawnCooldown - Time.fixedDeltaTime);

			if (_spawnCooldown == 0f)
			{
				float currentSignificanceTotal = 0f;
				var spaceRocks = spaceRockContainer.GetComponentsInChildren<SpaceRock>();
				foreach (var spaceRock in spaceRocks)
				{
					currentSignificanceTotal += spaceRock.significance;
				}

				if (currentSignificanceTotal < targetSignificanceTotal)
				{
					SpawnRock(spaceRockContainer, targetSignificanceTotal);
					_spawnCooldown += spawnCooldownDuration;
				}
			}
		}

		protected override void SetPositionAndMovement(SpaceRock rock, float movementSpeed)
		{
			var element = rock.GetComponent<SpriteElement>();
			var boundingSphere = new Sphere(Vector3.zero, 0f);
			boundingSphere.Encapsulate(((SphereBounds)element.bounds).sphere);
			var radius = boundingSphere.radius * 0.99f;

			var width = world.untransformedAxis0Vector.magnitude;
			var height = world.untransformedAxis1Vector.magnitude;

			var worldSurfaceNormal = Vector3.Cross(world.untransformedAxis1Vector, world.untransformedAxis0Vector).normalized;
			var axis0Normal = Vector3.Cross(world.untransformedAxis0Vector, worldSurfaceNormal).normalized;
			var axis1Normal = Vector3.Cross(worldSurfaceNormal, world.untransformedAxis1Vector).normalized;

			int side = UnityEngine.Random.Range(0, 2) + (UnityEngine.Random.Range(0f, width + height) < width ? 0 : 2);

			float movementAngle;
			switch (side)
			{
				case 0: // From bottom moving up.
					rock.transform.localPosition =
						world.untransformedOrigin
						+ world.untransformedAxis0Vector * UnityEngine.Random.Range(0f, 1f)
						- axis0Normal * radius;
					movementAngle = UnityEngine.Random.Range(Mathf.PI * 0.25f, Mathf.PI * 0.75f);
					break;
				case 1: // From top moving down.
					rock.transform.localPosition =
						world.untransformedOrigin
						+ world.untransformedAxis0Vector * UnityEngine.Random.Range(0f, 1f)
						+ world.untransformedAxis1Vector
						+ axis0Normal * radius;
					movementAngle = UnityEngine.Random.Range(Mathf.PI * -0.75f, Mathf.PI * -0.25f);
					break;
				case 2: // From left moving right.
					rock.transform.localPosition =
						world.untransformedOrigin
						+ world.untransformedAxis1Vector * UnityEngine.Random.Range(0f, 1f)
						- axis1Normal * radius;
					movementAngle = UnityEngine.Random.Range(Mathf.PI * -0.25f, Mathf.PI * 0.25f);
					break;
				case 3:// From right moving left.
					rock.transform.localPosition =
						world.untransformedOrigin
						+ world.untransformedAxis1Vector * UnityEngine.Random.Range(0f, 1f)
						+ world.untransformedAxis0Vector
						+ axis1Normal * radius;
					movementAngle = UnityEngine.Random.Range(Mathf.PI * 0.75f, Mathf.PI * 1.25f);
					break;
				default:
					throw new System.NotImplementedException();
			}

			rock.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(movementAngle), Mathf.Sin(movementAngle)) * movementSpeed;
		}
	}
}
