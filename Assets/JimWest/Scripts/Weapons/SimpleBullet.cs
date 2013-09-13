using UnityEngine;
using System.Collections;

public class SimpleBullet : MonoBehaviour {

	public float speed = 20f;
	public float lifeTime = 4f;
	public float dist = 10000f;
	
	private float spawnTime = 0.0f;
	private Transform tr;
	
	void OnEnable () {
		tr = transform;
		spawnTime = Time.time;
	}
	
	void Update () {
		tr.position += tr.forward * speed * Time.deltaTime;
		dist -= speed * Time.deltaTime;
		if (Time.time > spawnTime + lifeTime || dist < 0) {
			Destroy(this.gameObject);			
		}
	}
	
	void  OnTriggerEnter (Collider collision) {
	}
	
}
