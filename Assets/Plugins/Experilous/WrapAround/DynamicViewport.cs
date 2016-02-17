using UnityEngine;

namespace Experilous.WrapAround
{
	[RequireComponent(typeof(Viewport))]
	public class DynamicViewport : MonoBehaviour
	{
		private Viewport _viewport;

		protected void Start()
		{
			_viewport = GetComponent<Viewport>();
		}

		protected void LateUpdate()
		{
			_viewport.RecalculateVisibleGhostRegions();
		}
	}
}
