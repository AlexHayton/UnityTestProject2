using UnityEngine;
using System.Collections;

public class AIWeapon : MonoBehaviour {
	
	public int damagePerSecond = 1;
	private float lastAttack = 0f;	

	public void Attack(HealthHandler targetHealthHandler) {
		if (Time.time > lastAttack + 1)  {
			targetHealthHandler.DeductHealth(gameObject, damagePerSecond);
			lastAttack = Time.time;
		}
	}
}
