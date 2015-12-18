using UnityEngine;

namespace Experilous.WrapAround
{
	[RequireComponent(typeof(Element))]
	public class DynamicElementWrapper : MonoBehaviour
	{
		protected Element _element;

		protected void Start()
		{
			_element = GetComponent<Element>();
		}

		protected void Update()
		{
			_element.world.Confine(_element.transform);
		}
	}
}
