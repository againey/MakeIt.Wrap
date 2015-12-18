using UnityEngine;

namespace Experilous.WrapAround
{
	[RequireComponent(typeof(Element))]
	public class MechanicalElementWrapper : MonoBehaviour
	{
		protected Element _element;

		protected void Start()
		{
			_element = GetComponent<Element>();
		}

		protected void FixedUpdate()
		{
			_element.world.Confine(_element.transform);
		}
	}
}
