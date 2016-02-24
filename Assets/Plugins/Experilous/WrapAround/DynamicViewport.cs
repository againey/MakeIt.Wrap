/******************************************************************************\
 *  Copyright (C) 2016 Experilous <againey@experilous.com>
 *  
 *  This file is subject to the terms and conditions defined in the file
 *  'Assets/Plugins/Experilous/License.txt', which is a part of this package.
 *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	/// <summary>
	/// Forces a recalculation of the attached viewport's visible ghost regions every render frame.
	/// </summary>
	/// <remarks>
	/// This is created as a separate component primarily to allow the <c>LateUpdate()</c> method
	/// to be given a specific script execution order which will guarantee that it executes after
	/// any scripts that might affect the viewport's configuration (such as a camera controller),
	/// but before all other scripts that need to operate relative to the viewport's latest
	/// configuration (such as a renderable element or light element).
	/// </remarks>
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
