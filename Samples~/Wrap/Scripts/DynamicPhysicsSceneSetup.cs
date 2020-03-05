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
using System.Collections.Generic;
using MakeIt.Wrap;
using MakeIt.Core;

namespace MakeIt.Wrap.Samples
{
	public class DynamicPhysicsSceneSetup : MonoBehaviour
	{
		public RhomboidWorld world;
		public Viewport viewport;
		public int randomSeed = 0;

		[Header("Physical Rigidbodies")]
		public GameObject physicalRigidbodyPrefab;
		public int physicalRigidbodyCount = 10;
		public float minPhysicalRigidbodyScale = 1.0f;
		public float maxPhysicalRigidbodyScale = 2.0f;
		public float minPhysicalRigidbodySpeed = 1.0f;
		public float maxPhysicalRigidbodySpeed = 2.0f;
		public float minPhysicalRigidbodyRotation = -90.0f;
		public float maxPhysicalRigidbodyRotation = +90.0f;
		public Transform physicalRigidbodyContainer;

		[Header("Virtual Rigidbodies")]
		public GameObject virtualRigidbodyPrefab;
		public int virtualRigidbodyCount = 10;
		public float minVirtualRigidbodyScale = 1.0f;
		public float maxVirtualRigidbodyScale = 2.0f;
		public float minVirtualRigidbodySpeed = 1.0f;
		public float maxVirtualRigidbodySpeed = 2.0f;
		public float minVirtualRigidbodyRotation = -90.0f;
		public float maxVirtualRigidbodyRotation = +90.0f;
		public Transform virtualRigidbodyContainer;

		private struct RigidbodyDefinition
		{
			public Vector3 position;
			public float scale;

			public RigidbodyDefinition(Vector3 p, float s)
			{
				position = p;
				scale = s;
			}
		}

		protected void Start()
		{
			this.DisableAndThrowOnUnassignedReference(world, "The Rigidbodies.SceneSetup component requires a reference to a RhomboidWorld component.");
			this.DisableAndThrowOnUnassignedReference(viewport, "The Rigidbodies.SceneSetup component requires a reference to a Viewport component.");
			this.DisableAndThrowOnUnassignedReference(physicalRigidbodyPrefab, "The Rigidbodies.SceneSetup component requires a reference to a physical rigidbody GameObject prefab.");
			this.DisableAndThrowOnUnassignedReference(virtualRigidbodyPrefab, "The Rigidbodies.SceneSetup component requires a reference to a virtual rigidbody GameObject prefab.");
			this.DisableAndThrowOnUnassignedReference(physicalRigidbodyContainer, "The Rigidbodies.SceneSetup component requires a reference to a physical rigidbody Transform root.");
			this.DisableAndThrowOnUnassignedReference(virtualRigidbodyContainer, "The Rigidbodies.SceneSetup component requires a reference to a virtual rigidbody Transform root.");

			Physics.gravity = new Vector3(0f, 0f, 0f);
			Time.timeScale = 0f;

			List<RigidbodyDefinition> rigidbodies = new List<RigidbodyDefinition>();

			if (physicalRigidbodyPrefab != null)
			{
				float physicalPrefabRadius = 0;
				var sphereCollider = physicalRigidbodyPrefab.GetComponent<SphereCollider>();
				if (sphereCollider != null)
				{
					physicalPrefabRadius = sphereCollider.radius;
				}
				else
				{
					var boxCollider = physicalRigidbodyPrefab.GetComponent<BoxCollider>();
					if (boxCollider != null)
					{
						physicalPrefabRadius = boxCollider.size.magnitude * 0.5f;
					}
				}

				world.maxPhysicsObjectRadius = physicalPrefabRadius * maxPhysicalRigidbodyScale;

				float virtualPrefabRadius = 0;
				sphereCollider = virtualRigidbodyPrefab.GetComponent<SphereCollider>();
				if (sphereCollider != null)
				{
					virtualPrefabRadius = sphereCollider.radius;
				}
				else
				{
					var boxCollider = virtualRigidbodyPrefab.GetComponent<BoxCollider>();
					if (boxCollider != null)
					{
						virtualPrefabRadius = boxCollider.size.magnitude * 0.5f;
					}
				}

				world.maxVisibleObjectRadius = Mathf.Max(world.maxPhysicsObjectRadius, virtualPrefabRadius * maxVirtualRigidbodyScale);

				int tries = 0;
				while (rigidbodies.Count < physicalRigidbodyCount && tries < physicalRigidbodyCount * 10)
				{
					++tries;

					var x = UnityEngine.Random.Range(0, 1000000) / 1000000f;
					var y = UnityEngine.Random.Range(0, 1000000) / 1000000f;
					var p = world.transform.TransformPoint(world.axis0 * x + world.axis1 * y + world.origin);
					var s = UnityEngine.Random.Range(minPhysicalRigidbodyScale, maxPhysicalRigidbodyScale);
					var rigidbody = new RigidbodyDefinition(p, s);

					bool collisionFound = false;
					foreach (var existingRigidbody in rigidbodies)
					{
						var distance = (rigidbody.position - existingRigidbody.position).magnitude;
						var combinedRadius = physicalPrefabRadius * (rigidbody.scale + existingRigidbody.scale) + 0.01f;
						if (distance < combinedRadius)
						{
							collisionFound = true;
							break;
						}
					}

					if (collisionFound) continue;

					rigidbodies.Add(rigidbody);
				}

				for (int i = 0; i < rigidbodies.Count; ++i)
				{
					var definition = rigidbodies[i];
					var rigidbody = Instantiate(physicalRigidbodyPrefab);
					rigidbody.name = string.Format("Physical Rigidbody ({0})", i);
					rigidbody.transform.position = definition.position;
					rigidbody.transform.localScale = new Vector3(definition.scale, definition.scale, definition.scale);
					rigidbody.transform.SetParent(physicalRigidbodyContainer, false);

					rigidbody.GetComponent<RigidbodyElementWrapper>().world = world;
					rigidbody.GetComponent<RigidbodyElement>().world = world;
					rigidbody.GetComponent<MeshElement>().viewport = viewport;

					var rigidBody = rigidbody.GetComponent<Rigidbody>();
					var direction = UnityEngine.Random.Range(0, 1000000) / 1000000f * Mathf.PI * 2f;
					var speed = UnityEngine.Random.Range(minPhysicalRigidbodySpeed, maxPhysicalRigidbodySpeed);
					rigidBody.velocity = new Vector3(Mathf.Cos(direction) * speed, 0f, Mathf.Sin(direction) * speed);
					rigidBody.angularVelocity = new Vector3(
						UnityEngine.Random.Range(minPhysicalRigidbodyRotation, maxPhysicalRigidbodyRotation),
						UnityEngine.Random.Range(minPhysicalRigidbodyRotation, maxPhysicalRigidbodyRotation),
						UnityEngine.Random.Range(minPhysicalRigidbodyRotation, maxPhysicalRigidbodyRotation));

					var meshRenderer = rigidbody.GetComponent<MeshRenderer>();
					meshRenderer.material.color = new Color(
						UnityEngine.Random.Range(0f, 1f),
						UnityEngine.Random.Range(0f, 1f),
						UnityEngine.Random.Range(0f, 1f));

					var collider = rigidbody.GetComponent<Collider>();
					if (collider != null) collider.isTrigger = false;
				}
			}

			if (virtualRigidbodyPrefab != null)
			{
				for (int i = 0; i < virtualRigidbodyCount; ++i)
				{
					var x = UnityEngine.Random.Range(0, 1000000) / 1000000f;
					var y = UnityEngine.Random.Range(0, 1000000) / 1000000f;
					var p = world.transform.TransformPoint(world.axis0 * x + world.axis1 * y + world.origin);
					var s = UnityEngine.Random.Range(minVirtualRigidbodyScale, maxVirtualRigidbodyScale);
					var definition = new RigidbodyDefinition(p, s);

					var rigidbody = Instantiate(virtualRigidbodyPrefab);
					rigidbody.name = string.Format("Virtual Rigidbody ({0})", i);
					rigidbody.transform.position = definition.position;
					rigidbody.transform.localScale = new Vector3(definition.scale, definition.scale, definition.scale);
					rigidbody.transform.SetParent(virtualRigidbodyContainer, false);

					rigidbody.GetComponent<DynamicElementWrapper>().world = world;
					rigidbody.GetComponent<MeshElement>().viewport = viewport;

					var rigidBody = rigidbody.GetComponent<Rigidbody>();
					var direction = UnityEngine.Random.Range(0, 1000000) / 1000000f * Mathf.PI * 2f;
					var speed = UnityEngine.Random.Range(minVirtualRigidbodySpeed, maxVirtualRigidbodySpeed);
					rigidBody.velocity = new Vector3(Mathf.Cos(direction) * speed, 0f, Mathf.Sin(direction) * speed);
					rigidBody.angularVelocity = new Vector3(
						UnityEngine.Random.Range(minVirtualRigidbodyRotation, maxVirtualRigidbodyRotation),
						UnityEngine.Random.Range(minVirtualRigidbodyRotation, maxVirtualRigidbodyRotation),
						UnityEngine.Random.Range(minVirtualRigidbodyRotation, maxVirtualRigidbodyRotation));

					var meshRenderer = rigidbody.GetComponent<MeshRenderer>();
					meshRenderer.material.color = new Color(
						UnityEngine.Random.Range(0f, 1f),
						UnityEngine.Random.Range(0f, 1f),
						UnityEngine.Random.Range(0f, 1f));

					var collider = rigidbody.GetComponent<Collider>();
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
