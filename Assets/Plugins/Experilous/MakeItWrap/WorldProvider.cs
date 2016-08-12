/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.MakeIt.Wrap
{
	/// <summary>
	/// A component that stores a reference to a world and can provide it to any other
	/// component or other object that needs a reference to a world.
	/// </summary>
	/// <remarks>
	/// This component's inspector provides the ability to apply the referenced world to
	/// all descendants that implement the <see cref="IWorldConsumer"/> interface.
	/// 
	/// Classes can also be implemented so that within their Start() method, if they
	/// didn't already have a world assigned at edit-time, they can automatically look
	/// up among their ancestors in order to find a <see cref="WorldProvider"/> and use
	/// its world reference.
	/// </remarks>
	/// <seealso cref="World"/>
	/// <seealso cref="IWorldConsumer"/>
	[ExecuteInEditMode]
	[AddComponentMenu("Wrap-Around Worlds/World Provider")]
	public class WorldProvider : MonoBehaviour
	{
		public World world;

		protected void Awake()
		{
			if (world == null) world = GetComponent<World>();
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
				var consumers = recursive ? GetComponentsInChildren<IWorldConsumer>() : GetComponents<IWorldConsumer>();
				var changedConsumers = new System.Collections.Generic.List<Object>();
				foreach (var consumer in consumers)
				{
					if (consumer.GetWorld() == null)
					{
						changedConsumers.Add((Object)consumer);
					}
				}

				if (changedConsumers.Count > 0)
				{
					UnityEditor.Undo.RecordObjects(changedConsumers.ToArray(), recursive ? "Apply World to Components (Recursive)" : "Apply World to Components");

					foreach (var consumer in changedConsumers)
					{
						((IWorldConsumer)consumer).SetWorld(world);
						UnityEditor.EditorUtility.SetDirty(consumer);
					}
				}
			}
			else
			{
#endif
				var consumers = recursive ? GetComponentsInChildren<IWorldConsumer>() : GetComponents<IWorldConsumer>();
				foreach (var consumer in consumers)
				{
					if (consumer.GetWorld() == null)
					{
						consumer.SetWorld(world);
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
				var consumers = recursive ? GetComponentsInChildren<IWorldConsumer>() : GetComponents<IWorldConsumer>();
				var changedConsumers = new System.Collections.Generic.List<Object>();
				foreach (var consumer in consumers)
				{
					if (!ReferenceEquals(consumer.GetWorld(), world))
					{
						changedConsumers.Add((Object)consumer);
					}
				}

				if (changedConsumers.Count > 0)
				{
					UnityEditor.Undo.RecordObjects(changedConsumers.ToArray(), recursive ? "Apply World to Components (Recursive)" : "Apply World to Components");

					foreach (var consumer in changedConsumers)
					{
						((IWorldConsumer)consumer).SetWorld(world);
						UnityEditor.EditorUtility.SetDirty(consumer);
					}
				}
			}
			else
			{
#endif
				var consumers = recursive ? GetComponentsInChildren<IWorldConsumer>() : GetComponents<IWorldConsumer>();
				foreach (var consumer in consumers)
				{
					consumer.SetWorld(world);
				}
#if UNITY_EDITOR
			}
#endif
		}
	}
}
