using UnityEngine;

namespace Experilous.WrapAround
{
	[ExecuteInEditMode]
	public class ViewportProvider : MonoBehaviour
	{
		public Viewport viewport;

		protected void Awake()
		{
			if (viewport == null) viewport = GetComponent<Viewport>();
		}
	}
}
