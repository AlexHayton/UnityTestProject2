using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

// You can use these functions to instantiate and manage effects.
namespace TestProject
{
	public static class EntityUtility
	{
		public static bool HasComponent<ComponentType>(this Collider collider) where ComponentType : Component
		{
			return collider.gameObject.HasComponent<ComponentType>();
		}

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

		public static Transform FindChildRecursive(this Transform transformForSearch, string childName)
		{
			Transform foundChild = null;

			for (int i = 0 ; i < transformForSearch.childCount; i++)
			{
				Transform child = transformForSearch.GetChild(i);
				if (child.name == childName)
				{
					foundChild = child;
				}
				else if (child.childCount > 0)
				{
					foundChild = child.FindChildRecursive(childName);
				}

				if (foundChild != null)
				{
					break;
				}
			}

			return foundChild;
		}

		public static void DestroyGameObject(GameObject objToDestroy)
		{
			DestroyGameObject(objToDestroy, 0.0f);
		}

		public static void DestroyGameObject(GameObject objToDestroy, float destroyAfterTime)
		{
			// Handles fading out etc.
			FadeOutHandler fadeOut = objToDestroy.GetComponent<FadeOutHandler>();
			if (fadeOut)
			{
				fadeOut.QueueDestroy(destroyAfterTime);
			}
			else
			{
				GameObject.Destroy(objToDestroy, destroyAfterTime);
			}
		}
	}
}