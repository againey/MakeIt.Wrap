/******************************************************************************\
* Copyright Andy Gainey                                                        *
\******************************************************************************/

using UnityEngine;

namespace Experilous.WrapAround
{
	public class WrapAroundWorld : MonoBehaviour
	{
		private VisibleElementPool<ScalableSphereBounds, RhomboidWorld> _elementPool;

		protected void Awake()
		{
#if UNITY_EDITOR
			if (UnityEditor.EditorApplication.isPlaying)
			{
#endif
				_elementPool = gameObject.AddComponent<VisibleElementPool<ScalableSphereBounds, RhomboidWorld>>();
#if UNITY_EDITOR
			}
#endif
		}
	}
}
