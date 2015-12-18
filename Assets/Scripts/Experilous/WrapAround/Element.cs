using UnityEngine;

namespace Experilous.WrapAround
{
	public abstract class Element : MonoBehaviour
	{
		public World world;

		public abstract bool IsVisible(Viewport viewport, Vector3 position, Quaternion rotation);
	}
}
