using UnityEngine;
using System.Collections;

public class AIScript : MonoBehaviour {	
	
	public int damagePerSecond = 1;
	
	private Transform target;	
	private NavMeshAgent myAgent;
	private HealthHandler healtScript;
	private float lastAttack = 0f;

	void Start () {
		target = GameObject.FindWithTag ("Player").transform;
		healtScript = (HealthHandler)target.GetComponent<HealthHandler>();
		myAgent = GetComponent<NavMeshAgent>();
	}
	
	void Update () {
		if (target) {
			myAgent.destination = target.position;
			
			if (healtScript) {
				if (Vector3.Distance(target.transform.position , transform.position) <= myAgent.stoppingDistance + 0.1 && Time.time > lastAttack + 1) {
					healtScript.DeductHealth(damagePerSecond);
					lastAttack = Time.time;
				}
			}
			
		}
	}
}
