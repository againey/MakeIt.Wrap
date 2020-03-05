/******************************************************************************\
* Copyright Andy Gainey                                                        *
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
