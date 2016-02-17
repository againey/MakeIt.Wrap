using UnityEngine;

namespace Experilous.WrapAround
{
	[ExecuteInEditMode]
	public class WorldProvider : MonoBehaviour
	{
		public World world;

		protected void Awake()
		{
			if (world == null) world = GetComponent<World>();
		}
	}
}
