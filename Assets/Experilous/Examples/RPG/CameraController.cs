using UnityEngine;

namespace Experilous.Examples.RPG
{
	public class CameraController : MonoBehaviour
	{
		public PlayerController player;

		protected void LateUpdate()
		{
			transform.position = player.transform.position;
			transform.rotation = player.transform.rotation;
		}
	}
}
