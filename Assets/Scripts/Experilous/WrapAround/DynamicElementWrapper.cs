using UnityEngine;

namespace Experilous.WrapAround
{
	public class DynamicElementWrapper : MonoBehaviour
	{
		public World world;

		protected void Update()
		{
			world.Confine(transform);
		}
	}
}
