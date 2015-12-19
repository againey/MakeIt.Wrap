using UnityEngine;

namespace Experilous.WrapAround
{
	public class MechanicalElementWrapper : MonoBehaviour
	{
		public World world;

		protected void FixedUpdate()
		{
			world.Confine(transform);
		}
	}
}
