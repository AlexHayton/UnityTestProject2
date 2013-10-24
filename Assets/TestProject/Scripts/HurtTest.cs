using UnityEngine;
using System.Collections;

public class HurtTest : MonoBehaviour {
	
	public int damage = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}	
		
	void OnTriggerEnter(Collider other) {
		HealthHandler handler = other.GetComponent<HealthHandler>();
		if (handler)
		{
			handler.DeductHealth(other.gameObject, damage);
		}
    }
	
}
