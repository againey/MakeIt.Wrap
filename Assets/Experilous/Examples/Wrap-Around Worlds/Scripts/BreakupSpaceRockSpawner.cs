/******************************************************************************\
 *  Copyright (C) 2016 Experilous <againey@experilous.com>
 *  
 *  This file is subject to the terms and conditions defined in the file
 *  'Assets/Plugins/Experilous/License.txt', which is a part of this package.
 *
\******************************************************************************/

using UnityEngine;

namespace Experilous.Examples.WrapAround
{
	public class BreakupSpaceRockSpawner : SpaceRockSpawner
	{
		public void Breakup(float totalSignificance)
		{
			float currentSignificance = 0f;
			while (currentSignificance < totalSignificance)
			{
				var rock = SpawnRock(transform.parent, totalSignificance - currentSignificance);
				rock.GetComponent<Experilous.WrapAround.SpriteElement>().enabled = true;
				rock.GetComponent<Experilous.WrapAround.Rigidbody2DElement>().enabled = true;
				rock.GetComponent<Experilous.WrapAround.Rigidbody2DElementWrapper>().enabled = true;
				currentSignificance += rock.significance;
			}
		}

		protected override void SetPositionAndMovement(SpaceRock rock, float movementSpeed)
		{
			var bounds = GetComponent<Experilous.WrapAround.SphereBounds>();

			var movementAngle = Random.Range(0f, Mathf.PI * 2f);
			rock.transform.localPosition = transform.localPosition +
				new Vector3(Mathf.Cos(movementAngle), Mathf.Sin(movementAngle), 0f) * bounds.radius;

			rock.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(movementAngle), Mathf.Sin(movementAngle)) * movementSpeed;
		}
	}
}
