using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TestProject
{
	public static class PlayerUtility
	{
		private static GameObject _localPlayer = null;
	
		public static GameObject GetLocalPlayer()
		{
			if (_localPlayer == null)
			{
				_localPlayer = GameObject.FindWithTag("Player");
			}
			return _localPlayer;
		}

		public static GameObject GetParentPlayer(GameObject obj)
		{
			return GetParentPlayer(obj.transform);
		}

		public static GameObject GetParentPlayer(Component obj)
		{
			return GetParentPlayer(obj.transform);
		}

		public static GameObject GetParentPlayer(Transform trans)
		{
			Transform playerTransform = trans.root.GetComponentsInChildren<Transform>().Where (t => t.tag == "Player").FirstOrDefault();
			if (playerTransform)
			{
				return playerTransform.gameObject;
			}
			else 
			{
				return null;	
			}
		}

		public static GameObject GetNearestPlayer(Vector3 position)
		{
			return GetAllPlayersInClosestOrder(position).FirstOrDefault();
		}

		public static IEnumerable<GameObject> GetAllPlayersInClosestOrder(Vector3 position)
		{
			return GetAllPlayers().OrderBy(player => (position - player.transform.position).sqrMagnitude);
		}
		
		public static GameObject[] GetAllPlayers()
		{
			return GameObject.FindGameObjectsWithTag("Player");
		}
	}
}

