﻿/******************************************************************************\
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
	public class Laser : MonoBehaviour
	{
		public float duration = 1f;

		private float _durationConsumed = 0f;

		protected void FixedUpdate()
		{
			_durationConsumed += Time.fixedDeltaTime;
			if (_durationConsumed >= duration)
			{
				Destroy(gameObject);
			}
		}
	}
}
