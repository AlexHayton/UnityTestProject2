using UnityEngine;
using System.Collections;

public class EnemySpawnerLogic : Entity {
	
	public GameObject enemyPrefab;
	public int spawnAmount = 4;
	public float spawnTime = 4f;
	
	private float lastSpawn = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isEnabled){
			if (lastSpawn == 0 | Time.time > lastSpawn + spawnTime) {
				if (enemyPrefab) {
					for (int i = 0; i < spawnAmount; i++) {
						Instantiate(enemyPrefab, transform.position, transform.rotation);	
					}
				}
				lastSpawn = Time.time;
			}
		}
	}
}
