using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TestProject
{
	public class PlayerUtility
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

