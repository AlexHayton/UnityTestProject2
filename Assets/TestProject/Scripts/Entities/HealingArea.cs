using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(TeamHandler))]
public class HealingArea : MonoBehaviour {
	
	public float healRate;
	private TeamHandler teamHandler;
	
	void Start()
	{
		teamHandler = this.GetComponent<TeamHandler>();
	}
	
	// Slowly heal any objects in the area.
	void OnTriggerStay(Collider other) {

		GameObject target = other.gameObject;
        if (target)
        {
        	HealthHandler handler = target.GetComponent<HealthHandler>();
        	
        	if (handler)
        	{
        		if (handler.GetCanBeHealed(teamHandler.GetTeam()))
        		{
        			handler.AddHealth(healRate * Time.deltaTime);
        		}
        	}
        }

    }
}
