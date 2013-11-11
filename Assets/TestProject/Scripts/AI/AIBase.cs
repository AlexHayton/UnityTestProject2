using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class AIBase : MonoBehaviour {
	
	public enum State {		
		Idle,		
		Attack,
		Defend,
		Retreat,
		Dead,
		WalkAround,
	}
	
	public State myState = State.Idle;
	public int walkAroundDistance = 40;		
	public ActivationMask activation;
		
	private Transform target;	
	private NavMeshAgent myAgent;
	private float lastAttack = 0f;

	void Start () {
		myAgent = GetComponent<NavMeshAgent>();
		GameObject test = (GameObject)GameObject.FindGameObjectWithTag("Player");
	}
	
	
	// Update is called once per frame
	void Update () {
			
		switch (myState) {
			
			case State.Idle:
				break;
				
			case State.WalkAround:
				OnWalkAround();
				break;
				
			case State.Attack:
				break;
				
			case State.Dead:
				break;
				
			case State.Defend:
				break;
				
			case State.Retreat:
				break;
				
			default:
				break;
			
		}

	}
	
	internal void OnIdle() {		
	}
	
	internal void OnWalkAround() {		
				
		if (myAgent.remainingDistance <= myAgent.stoppingDistance) {					
			myAgent.SetDestination(NavMeshUtility.GetRandomPoint(this.transform.position, walkAroundDistance));
		}
		
	}
	
	
}
