﻿/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;
using Experilous.MakeItWrap;
using Experilous.Numerics;

namespace Experilous.Examples.MakeItWrap
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
			var movementAngle = Random.Range(0f, Mathf.PI * 2f);
			var boundingSphere = new Sphere(Vector3.zero, 0f);
			var element = GetComponent<SpriteElement>();
			boundingSphere.Encapsulate(((SphereBounds)element.bounds).sphere);
			rock.transform.localPosition = transform.localPosition +
				new Vector3(Mathf.Cos(movementAngle), Mathf.Sin(movementAngle), 0f) * boundingSphere.radius * 0.5f;

			rock.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(movementAngle), Mathf.Sin(movementAngle)) * movementSpeed;
		}
	}
}
