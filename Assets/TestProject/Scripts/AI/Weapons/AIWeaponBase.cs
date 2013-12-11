using UnityEngine;
using System.Collections;

public class AIWeaponBase : MonoBehaviour {
	
	public int damagePerSecond = 1;
	public float range = 1f;
	internal float lastAttack = 0f;	

	public virtual void Attack(HealthHandler targetHealthHandler) {
		if (Time.time > lastAttack + 1)  {
			targetHealthHandler.DeductHealth(gameObject, damagePerSecond);
			lastAttack = Time.time;
		}
	}

	public virtual void Fire() {

	}

}
