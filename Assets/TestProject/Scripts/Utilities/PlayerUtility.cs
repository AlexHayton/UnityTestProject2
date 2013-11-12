using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		
		public static IList<GameObject> GetAllPlayers()
		{
			return new List<GameObject>();
		}
	}
}

