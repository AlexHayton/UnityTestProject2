using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestProject
{
	public static class TeamUtility
	{
		public static IEnumerable<TeamHandler>GetTeamHandlersInScene()
		{
			// Function isn't exactly fast so warn when we use it.
			Debug.Log ("Called GetTeamHandlersInScene!");
			return Object.FindObjectsOfType(TeamHandler); 
		}
	}
}
