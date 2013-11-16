﻿using UnityEngine;
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
		
	internal GameObject target;	
	internal HealthHandler targetHealthHandler;
	internal NavMeshAgent m_Agent;
	internal AIWeaponBase m_Weapon;

	public virtual void Start () {
		m_Agent = GetComponent<NavMeshAgent>();
		m_Weapon = GetComponent<AIWeaponBase>();
		
		if (activation.IsSet (ActivationMask.ActivationType.AfterSpawn)) {
			FindNewTarget();
		}
	}
	
	
	// Update is called once per frame
	public virtual void Update () {
			
		switch (myState) {
			
			case State.Idle:
				OnIdleState();
				break;
				
			case State.WalkAround:
				OnWalkAroundState();
				break;
				
			case State.Attack:
				OnAttackState();
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

#region State functions

	internal virtual void OnIdleState() {	
		// for testing
		myState = State.WalkAround;
	}
	
	internal virtual void OnWalkAroundState() {		
				
		if (m_Agent.remainingDistance <= m_Agent.stoppingDistance) {	
			FindNewTarget();
			if (!target) {
				m_Agent.SetDestination(NavMeshUtility.GetRandomPoint(this.transform.position, walkAroundDistance));
			}
		}
		
	}
	
	internal virtual void OnAttackState() {
		if (target) {
			if (Vector3.Distance(target.transform.position , transform.position) <= m_Agent.stoppingDistance + 0.1) {
				Attack ();
			} else {
				m_Agent.destination =  target.transform.position;
			}
		} else {
			myState = State.Idle;	
		}
	}

	
#endregion

	internal virtual bool FindNewTarget() {
		if (activation.IsSet (ActivationMask.ActivationType.AfterSpawn)) {

	        Collider[] hitColliders = Physics.OverlapSphere(transform.position, targetSearchRange, targetSearchLayer.value);
	        foreach (Collider collider in hitColliders) {
				// Todo, currently theres no team handler so just search for the player
				// add search for Health + TeamHandler here
	        	if (collider.tag == "Player") {
					CheckTarget(collider.gameObject);
				}
				
	        }
		}
		
		return false;
	}


	internal virtual void Attack() {
		if (m_Weapon) {
			m_Weapon.Attack(targetHealthHandler);
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
	
	internal virtual bool IsTargetCloser(GameObject newTarget) {
		if (target) {
			return Vector3.Distance(transform.position, target.transform.position) >
					Vector3.Distance(transform.position, newTarget.transform.position);			
		} else {
			return true;	
		}
	}
	
	
#region Signals
	
	internal virtual void OnTakeDamage(GameObject attacker) {
		if ((activation.mask &ActivationMask.ActivationType.GettingAttacked) != 0) {
			CheckTarget(attacker);
		}
	}
#endregion
	
}
