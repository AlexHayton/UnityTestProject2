using UnityEngine;
using System.Collections;

namespace TestProject
{
	public class TeamHandler : MonoBehaviour {
	
		GameObject UseTarget;

		public bool CanUse()
    	{
        	return team == this.team;
    	}
    	
    }
}