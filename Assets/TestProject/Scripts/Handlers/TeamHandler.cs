using UnityEngine;
using System.Collections;
using System;

public class TeamHandler : MonoBehaviour {
	
	public enum Team
	{
		Player,
		Neutral,
		Enemy
	}
	
	public Team team = Team.Neutral;
	
	public Team GetTeam() {	
		return this.team;
	}

    public bool IsFriendly(Team team)
    {
        return team == this.team;
    }
	
}
