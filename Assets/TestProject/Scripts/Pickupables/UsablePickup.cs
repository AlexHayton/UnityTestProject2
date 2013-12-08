using UnityEngine;
using System.Collections;
using TestProject;
using System.Collections.Generic;

[RequireComponent (typeof (Collider))]
[RequireComponent (typeof (UseTarget))]
public abstract class UsablePickup : Pickupable, IUsable {
	
	public override void Start()
	{	
		base.Start();

		Collider collider = this.GetCollider();
		collider.isTrigger = false;

		UseTarget localTarget = this.GetComponent<UseTarget>();
		if (localTarget)
		{
			localTarget.RegisterUsable(this);
		}
		else
		{
			Debug.LogError("UsablePickup has no UseTarget!");
		}
	}

	public override void OnTriggerEnter(Collider collision)
	{
	}
	
	public virtual bool CanBeUsedBy(GameObject user)
	{
		return true;
	}
	
	public void OnUse(GameObject user)
	{
		if (this.CanBePickedUpBy(user)) {
				
			if (this.OnPickUp(user))
			{					
				// EffectUtility sanity checks for us!
				EffectUtility.TryInstantiateEffectPrefab(pickUpEffectPrefab, transform.position, transform.rotation);
					
				Destroy (this.gameObject);
			}
		}
	}
}
