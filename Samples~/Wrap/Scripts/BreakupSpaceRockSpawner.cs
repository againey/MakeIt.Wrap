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
	public class BreakupSpaceRockSpawner : SpaceRockSpawner
	{
		public void Breakup(float totalSignificance)
		{
			float currentSignificance = 0f;
			while (currentSignificance < totalSignificance)
			{
				var rock = SpawnRock(transform.parent, totalSignificance - currentSignificance);
				rock.GetComponent<SpriteElement>().enabled = true;
				rock.GetComponent<Rigidbody2DElement>().enabled = true;
				rock.GetComponent<Rigidbody2DElementWrapper>().enabled = true;
				currentSignificance += rock.significance;
			}
		}

		protected override void SetPositionAndMovement(SpaceRock rock, float movementSpeed)
		{
			var movementAngle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
			var boundingSphere = new Sphere(Vector3.zero, 0f);
			var element = GetComponent<SpriteElement>();
			boundingSphere.Encapsulate(((SphereBounds)element.bounds).sphere);
			rock.transform.localPosition = transform.localPosition +
				new Vector3(Mathf.Cos(movementAngle), Mathf.Sin(movementAngle), 0f) * boundingSphere.radius * 0.5f;

			rock.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(movementAngle), Mathf.Sin(movementAngle)) * movementSpeed;
		}
	}
}
