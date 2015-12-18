using UnityEngine;

namespace Experilous.WrapAround
{
	[RequireComponent(typeof(Element))]
	public abstract class GhostSpawner : MonoBehaviour
	{
		public Viewport viewport;

		protected Element _element;

		protected void Start()
		{
			_element = GetComponent<Element>();
		}
	}
}
