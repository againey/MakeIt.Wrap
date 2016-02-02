using UnityEngine;

namespace Experilous.WrapAround
{
	public class DynamicElementWrapper : MonoBehaviour
	{
		public World world;

		protected void LateUpdate()
		{
			world.Confine(transform);
		}
	}
}
