using UnityEngine;
using System.Collections;

// this is just a temporary weapon until we have real models
public class AIWeaponBase : Weapon {
	
	public override bool Attack() {
		return base.Attack ();
	}

	public virtual void DoDamageTo(HealthHandler enemyHealthHandler) {
		enemyHealthHandler.DeductHealth(this.owner.gameObject, this.damageOnHit);
	}


}
