/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.MakeItWrap
{
	/// <summary>
	/// Forces a recalculation of the attached viewport's visible ghost regions every render frame.
	/// </summary>
	/// <remarks>
	/// This is created as a separate component primarily to allow the <c>LateUpdate()</c> method
	/// to be given a specific script execution order which will guarantee that it executes after
	/// any scripts that might affect the viewport's configuration (such as a camera controller),
	/// but before all other scripts that need to operate relative to the viewport's latest
	/// configuration (such as a mesh element or light element).
	/// </remarks>
	[RequireComponent(typeof(Viewport))]
	[AddComponentMenu("Make It Wrap/Dynamic Viewport")]
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
