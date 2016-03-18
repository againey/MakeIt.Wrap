/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
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
	public class WorldProvider : MonoBehaviour
	{
		public World world;

		protected void Awake()
		{
			if (world == null) world = GetComponent<World>();
		}
	}
}
