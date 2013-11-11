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
	public int targetSearchRange = 50;
	public ActivationMask activation;
	public LayerMask targetSearchLayer;
		
	private GameObject target;	
	private HealthHandler targetHealthHandler;
	private NavMeshAgent m_Agent;
	private AIWeapon m_Weapon;

	void Start () {
		m_Agent = GetComponent<NavMeshAgent>();
		m_Weapon = GetComponent<AIWeapon>();
		
		if (activation.IsSet (ActivationMask.ActivationType.AfterSpawn)) {
			FindNewTarget();
		}
	}
	
	
	// Update is called once per frame
	void Update () {
			
		switch (myState) {
			
			case State.Idle:
				OnIdle();
				break;
				
			case State.WalkAround:
				OnWalkAround();
				break;
				
			case State.Attack:
				OnAttack ();
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
	
	internal virtual void OnIdle() {	
		// for testing
		myState = State.WalkAround;
	}
	
	internal virtual void OnWalkAround() {		
				
		if (m_Agent.remainingDistance <= m_Agent.stoppingDistance) {	
			FindNewTarget();
			if (!target) {
				m_Agent.SetDestination(NavMeshUtility.GetRandomPoint(this.transform.position, walkAroundDistance));
			}
		}
		
	}
	
	internal virtual void OnAttack() {
		if (target) {
			if (Vector3.Distance(target.transform.position , transform.position) <= m_Agent.stoppingDistance + 0.1) {
				m_Weapon.Attack(targetHealthHandler);
			} else {
				m_Agent.destination =  target.transform.position;
			}
		} else {
			myState = State.Idle;	
		}
	}
	
	
	internal virtual void CheckTarget(GameObject newTarget) {
		if ((myState != State.Attack) | (target & IsTargetCloser(newTarget)) ) {
			targetHealthHandler = newTarget.GetComponent<HealthHandler>();
			if (targetHealthHandler) {
				target = newTarget;
				myState = State.Attack;
				//m_Agent.SetDestination(newTarget.transform.position);
				m_Agent.destination = newTarget.transform.position;
			}
		}
	}
	
	
	internal virtual void OnTakeDamage(GameObject attacker) {
		if ((activation.mask &ActivationMask.ActivationType.GettingAttacked) != 0) {
			CheckTarget(attacker);
		}
	}
	
	internal virtual bool FindNewTarget() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, targetSearchRange, targetSearchLayer.value);
        foreach (Collider collider in hitColliders) {
			// Todo, currently theres no team handler so just search for the player
			// add search for Health + TeamHandler here
        	if (collider.tag == "Player") {
				CheckTarget(collider.gameObject);
			}
			
        }
		
		return false;
	}
	
	internal virtual bool IsTargetCloser(GameObject newTarget) {
		if (target) {
			return Vector3.Distance(transform.position, target.transform.position) >
					Vector3.Distance(transform.position, newTarget.transform.position);			
		} else {
			return true;	
		}
	}
	
	
}
