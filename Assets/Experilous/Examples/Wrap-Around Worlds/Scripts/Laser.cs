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
