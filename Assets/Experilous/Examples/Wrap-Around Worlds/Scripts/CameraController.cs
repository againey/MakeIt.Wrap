using UnityEngine;

namespace Experilous.Examples.RPG
{
	public class CameraController : MonoBehaviour
	{
		public PlayerController player;

		protected void Start()
		{
			this.DisableAndThrowOnMissingReference(player, "The RPG.CameraController component requires a reference to a PlayerController component.");
		}

		protected void LateUpdate()
		{
			transform.position = player.transform.position;
			transform.rotation = player.transform.rotation;
		}
	}
}
