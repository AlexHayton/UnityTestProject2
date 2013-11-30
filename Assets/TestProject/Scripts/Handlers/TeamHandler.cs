using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TestProject;
using System.Linq;

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
	
	public IEnumerable<GameObject>GetEnemiesInRadius(float maxDistance)
	{
		IEnumerable<GameObject> candidateObjects = TeamUtility.GetTeamHandlersInScene().Select(h => h.gameObject);
		return candidateObjects.Where(go => go.HasComponent<HealthHandler>());
	}

	public GameObject GetEnemyInFront(LayerMask enemyFilterMask)
	{
		// Get a list of enemy entities
		IEnumerable<GameObject> enemies = TestProject.EntityUtility.GetTargetableGameObjectsInScene();
		return enemies.GetClosestEntityTo(gameObject);
	}

	public GameObject GetClosestFriend(LayerMask enemyFilterMask)
	{
		// Get a list of enemy entities
		IEnumerable<GameObject> enemies = TestProject.EntityUtility.GetTargetableGameObjectsInScene();
		return enemies.GetClosestEntityTo(gameObject);
	}
	
}
