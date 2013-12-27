using UnityEngine;
using System.Collections;

// this is just a temporary weapon until we have real models
public class AIWeaponBase : Weapon {
	
	public override bool OnPrimaryAttack() {
		// do diddly squat
		return true;
	}

	public virtual void DoDamageTo(HealthHandler enemyHealthHandler) {
		enemyHealthHandler.DeductHealth(this.owner.gameObject, this.damageOnHit);
	}


}
