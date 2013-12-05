using UnityEngine;
using System.Collections;
using TestProject;
using System.Collections.Generic;

[RequireComponent (typeof (CapsuleCollider))]
[RequireComponent (typeof (UseTarget))]
public abstract class UsablePickup : Pickupable, IUsable {
	
	void Start()
	{
		base.Start();
		
		UseTarget localTarget = this.GetComponent<UseTarget>();
		localTarget.RegisterUsable(this);
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
