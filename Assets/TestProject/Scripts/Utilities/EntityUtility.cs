using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

// You can use these functions to instantiate and manage effects.
namespace TestProject
{
	public static class EntityUtility
	{
		public static bool HasComponent<ComponentType>(this GameObject gameObject) where ComponentType : Component
		{
			ComponentType component = gameObject.GetComponent<ComponentType>();
			// Return false if the component is null
			return component != null;
		}

		public static IEnumerable<GameObject>GetTargetableGameObjectsInScene()
		{
			IEnumerable<GameObject> candidateObjects = TeamUtility.GetTeamHandlersInScene().Select(h => h.gameObject);
			return candidateObjects.Where(go => go.HasComponent<HealthHandler>());
		}

		public static IEnumerable<GameObject>GetClosestEntitiesTo(this IEnumerable<GameObject> objects, GameObject gameObject)
		{
			return objects.GetClosestEntitiesTo(gameObject.transform);
		}

		public static GameObject GetClosestEntityTo(this IEnumerable<GameObject> objects, GameObject gameObject)
		{
			return objects.GetClosestEntityTo(gameObject.transform);
		}

		public static IEnumerable<GameObject>GetClosestEntitiesTo(this IEnumerable<GameObject> objects, Transform transform)
		{
			return objects.GetClosestEntitiesTo(transform.position);
		}

		public static GameObject GetClosestEntityTo(this IEnumerable<GameObject> objects, Transform transform)
		{
			return objects.GetClosestEntityTo(transform.position);
		}

		public static IEnumerable<GameObject>GetClosestEntitiesTo(this IEnumerable<GameObject> objects, Vector3 point)
		{
			return objects.OrderBy(o => o.transform.position);
		}

		public static GameObject GetClosestEntityTo(this IEnumerable<GameObject> objects, Vector3 point)
		{
			return objects.GetClosestEntitiesTo(point).FirstOrDefault();
		}
	}
}