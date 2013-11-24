using UnityEngine;
using System.Collections;

public class EnemySpawnerLogic : Entity {
	
	public GameObject enemyPrefab;
	public int spawnAmount = 4;
	public float spawnTime = 4f;
	public int numberOfWaves = 1;
	private int spawnedWaves = 0;
	public bool infiniteWaves = true;
	
	private float lastSpawn = 0f;

	// Use this for initialization
	void Start () {
		InvokeRepeating("CheckWhetherToSpawn", 0, spawnTime);
	}
	
	// Update is called once per frame
	void CheckWhetherToSpawn () 
	{
		if (isEnabled && (infiniteWaves || numberOfWaves < spawnedWaves))
		{
			if (enemyPrefab) 
			{
				for (int i = 0; i < spawnAmount; i++) 
				{
					Instantiate(enemyPrefab, transform.position, transform.rotation);	
				}
			}
			spawnedWaves++;
		}
	}
}
