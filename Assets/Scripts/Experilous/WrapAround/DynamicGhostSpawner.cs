using UnityEngine;

namespace Experilous.WrapAround
{
	[RequireComponent(typeof(Element))]
	public class DynamicGhostSpawner : GhostSpawner
	{
		public DynamicElementGhost ghostPrefab;

		protected new void Start()
		{
			base.Start();
			if (ghostPrefab == null)
			{
				var elementPrefab = Instantiate(_element);
				ghostPrefab = elementPrefab.gameObject.AddComponent<DynamicElementGhost>();
				Destroy(elementPrefab);
			}
		}

		protected void LateUpdate()
		{
			foreach (var ghostRegion in viewport.visibleGhostRegions)
			{
				if (!ghostRegion.HasGhost(_element))
				{
					Vector3 position = transform.position;
					Quaternion rotation = transform.rotation;
					ghostRegion.Transform(ref position, ref rotation);

					if (_element.IsVisible(viewport, position, rotation))
					{
						var ghost = Instantiate(ghostPrefab);
						ghost.transform.SetParent(_element.transform.parent, false);
						ghost.original = _element;
						ghost.region = ghostRegion;
						//ghost.UpdateFromOriginal();
						ghostRegion.AddElement(_element);
					}
				}
			}
		}
	}
}
