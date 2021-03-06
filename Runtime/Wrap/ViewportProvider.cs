﻿/******************************************************************************\
* Copyright Andy Gainey                                                        *
*                                                                              *
* Licensed under the Apache License, Version 2.0 (the "License");              *
* you may not use this file except in compliance with the License.             *
* You may obtain a copy of the License at                                      *
*                                                                              *
*     http://www.apache.org/licenses/LICENSE-2.0                               *
*                                                                              *
* Unless required by applicable law or agreed to in writing, software          *
* distributed under the License is distributed on an "AS IS" BASIS,            *
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.     *
* See the License for the specific language governing permissions and          *
* limitations under the License.                                               *
\******************************************************************************/

using UnityEngine;

namespace MakeIt.Wrap
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
	[AddComponentMenu("MakeIt.Wrap/Viewport Provider")]
	public class ViewportProvider : MonoBehaviour
	{
		public Viewport viewport;

		protected void Awake()
		{
			if (viewport == null) viewport = GetComponent<Viewport>();
		}

		public void ApplyToUnsetConsumers(Component obj, bool recursive = false)
		{
			ApplyToUnsetConsumers(obj.gameObject, recursive);
		}

		public void ApplyToUnsetConsumers(GameObject obj, bool recursive = false)
		{
#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
			{
				var consumers = recursive ? GetComponentsInChildren<IViewportConsumer>() : GetComponents<IViewportConsumer>();
				var changedConsumers = new System.Collections.Generic.List<Object>();
				foreach (var consumer in consumers)
				{
					if (consumer.GetViewport() == null)
					{
						changedConsumers.Add((Object)consumer);
					}
				}

				if (changedConsumers.Count > 0)
				{
					UnityEditor.Undo.RecordObjects(changedConsumers.ToArray(), recursive ? "Apply Viewport to Components (Recursive)" : "Apply Viewport to Components");

					foreach (var consumer in changedConsumers)
					{
						((IViewportConsumer)consumer).SetViewport(viewport);
						UnityEditor.EditorUtility.SetDirty(consumer);
					}
				}
			}
			else
			{
#endif
				var consumers = recursive ? GetComponentsInChildren<IViewportConsumer>() : GetComponents<IViewportConsumer>();
				foreach (var consumer in consumers)
				{
					if (consumer.GetViewport() == null)
					{
						consumer.SetViewport(viewport);
					}
				}
#if UNITY_EDITOR
			}
#endif
		}

		public void ApplyToAllConsumers(Component obj, bool recursive = false)
		{
			ApplyToAllConsumers(obj.gameObject, recursive);
		}

		public void ApplyToAllConsumers(GameObject obj, bool recursive = false)
		{
#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
			{
				var consumers = recursive ? GetComponentsInChildren<IViewportConsumer>() : GetComponents<IViewportConsumer>();
				var changedConsumers = new System.Collections.Generic.List<Object>();
				foreach (var consumer in consumers)
				{
					if (!ReferenceEquals(consumer.GetViewport(), viewport))
					{
						changedConsumers.Add((Object)consumer);
					}
				}

				if (changedConsumers.Count > 0)
				{
					UnityEditor.Undo.RecordObjects(changedConsumers.ToArray(), recursive ? "Apply Viewport to Components (Recursive)" : "Apply Viewport to Components");

					foreach (var consumer in changedConsumers)
					{
						((IViewportConsumer)consumer).SetViewport(viewport);
						UnityEditor.EditorUtility.SetDirty(consumer);
					}
				}
			}
			else
			{
#endif
				var consumers = recursive ? GetComponentsInChildren<IViewportConsumer>() : GetComponents<IViewportConsumer>();
				foreach (var consumer in consumers)
				{
					consumer.SetViewport(viewport);
				}
#if UNITY_EDITOR
			}
#endif
		}
	}
}
