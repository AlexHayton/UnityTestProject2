using System;
using UnityEngine;

public class DetonationWeapon: Weapon {
	public GameObject iginitionPrefab;
	public GameObject explosionPrefab;

	protected float ignitionTime = 1.0f;
	protected bool ignited = false;
	protected GameObject ignitionParticle;

	public override bool OnPrimaryAttack() {
		if (!ignited) {
			this.Iginite();
			return true;
		}
		return false;
	}

	public override void Update() {
		if (ignited && Time.time >= lastAttack + ignitionTime) {
			
			Collider[] hitColliders = Physics.OverlapSphere(transform.position, this.range);
			foreach (Collider collider in hitColliders) {
				// Todo, currently theres no team handler so just search for the player
				// add search for Health + TeamHandler here
				if (collider.tag == "Player") {
					collider.GetComponent<HealthHandler>().DeductHealth(this.gameObject, this.damageOnHit);
				}
				
				if (collider.rigidbody) {
					collider.rigidbody.AddExplosionForce(this.forceOnImpact, this.transform.position, this.range, 3.0F, ForceMode.Impulse);
				}
				
			}
			
			
			if (explosionPrefab) {
				GameObject tempExplosion = (GameObject)Instantiate(explosionPrefab, transform.position, transform.rotation);	
				if (tempExplosion.particleSystem) {
					tempExplosion.particleSystem.startSize += this.range;
				}
				
				Destroy (tempExplosion, 1f);
			}
			
			Destroy (ignitionParticle);
			Destroy(this.gameObject);
		}
	}


	public void Iginite() {
		ignited = true;
		if (iginitionPrefab) {
			ignitionParticle = (GameObject)Instantiate(iginitionPrefab, transform.position, transform.rotation);	
			ignitionParticle.transform.parent = this.transform;
		};
	}

}
