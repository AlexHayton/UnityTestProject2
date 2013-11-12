using UnityEngine;
using System.Collections;

public class PlayerUtility {
	
	private static GameObject _localPlayer = null;
	
	public static GameObject GetLocalPlayer()
	{
		if (_localPlayer == null)
		{
			_localPlayer = GameObject.FindWithTag("Player");
		}
		return _localPlayer;
	}
	
}