using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TestProject
{
	public static class GUIUtility
	{
		private static GUIHandler _handler = null;
		private static GameObject _cachedPlayer = null;

		public static GUIHandler GetLocalGUIHandler()
		{
			GameObject player = PlayerUtility.GetLocalPlayer();
			if (_cachedPlayer == null || _cachedPlayer != player)
			{
				_handler = player.GetComponent<GUIHandler>();
			}

			return _handler;
		}

		public static GUIHandler GetParentGUIHandler(GameObject obj)
		{
			return GetParentGUIHandler(obj.transform);
		}

		public static GUIHandler GetParentGUIHandler(Component obj)
		{
			return GetParentGUIHandler(obj.transform);
		}
		
		public static GUIHandler GetParentGUIHandler(Transform trans)
		{
			return trans.root.GetComponentInChildren<GUIHandler>();
		}
	}
}

