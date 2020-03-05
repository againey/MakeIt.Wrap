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

namespace MakeIt.Wrap.Samples
{
	[RequireComponent(typeof(Rigidbody))]
	public class SimpleCharacterController : MonoBehaviour
	{
		public float walkSpeed = 1f;

		protected Rigidbody _rigidbody;

		protected void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		protected void Update()
		{
			var horizontal = Input.GetAxis("Horizontal");
			var vertical = Input.GetAxis("Vertical");

			_rigidbody.velocity = new Vector3(horizontal * walkSpeed, 0f, vertical * walkSpeed);
		}
	}
}
