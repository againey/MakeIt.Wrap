/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	/// <summary>
	/// A component that stores a reference to a viewport and can provide it to any other
	/// component or other object that needs a reference to a viewport.
	/// </summary>
	/// <remarks>
	/// This component's inspector provides the ability to apply the referenced viewport to
	/// all descendants that implement the <see cref="IViewportConsumer"/> interface.
	/// 
	/// Classes can also be implemented so that within their Start() method, if they
	/// didn't already have a viewport assigned at edit-time, they can automatically look
	/// up among their ancestors in order to find a <see cref="ViewportProvider"/> and use
	/// its viewport reference.
	/// </remarks>
	/// <seealso cref="Viewport"/>
	/// <seealso cref="IViewportConsumer"/>
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
