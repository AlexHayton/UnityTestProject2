using UnityEngine;
using System.Collections;
using System;
using TestProject;

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
	
	public IEnumerable<GameObject>GetEnemiesInRadius(float maxDistance)
	{
		using System.Collections;
		IEnumerable<GameObject> candidateObjects = TeamUtility.GetTeamHandlersInScene().Select(h => h.gameObject);
		return candidateObjects.Where(go => go.HasComponent<HealthHandler>());
	}

	public GameObject GetEnemyInFront(GameObject gameObject, LayerMask enemyFilterMask)
	{
		// Get a list of enemy entities
		IEnumerable<GameObject> enemies = GetTargetableGameObjectsInScene();
		return enemies.GetClosestTo(gameObject);
	}

	public GameObject GetClosestFriend(GameObject gameObject, LayerMask enemyFilterMask)
	{
		// Get a list of enemy entities
		IEnumerable<GameObject> enemies = GetTargetableGameObjectsInScene();
		return enemies.GetClosestTo(gameObject);
	}
	
}
