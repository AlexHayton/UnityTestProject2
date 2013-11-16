using UnityEngine;
using System.Collections;

public class ExplodeWeapon : AIWeaponBase {

	public float explodeTime = 1.0f;
	public float explosionRadius = 5f;
	public float explosionForce = 50f;
	public GameObject glowPrefab;
	public GameObject explodePrefab;

	private bool ignited = false;
	private GameObject explodeParticle;
	private AIBase myAi;

	void Start() {
		myAi = GetComponent<AIBase>();
	}

	void Update() {
		if (ignited && Time.time >= lastAttack + explodeTime) {

			Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
			foreach (Collider collider in hitColliders) {
				// Todo, currently theres no team handler so just search for the player
				// add search for Health + TeamHandler here
				if (collider.tag == "Player") {
					collider.GetComponent<HealthHandler>().DeductHealth(this.gameObject, this.damagePerSecond);
				}

				if (collider.rigidbody) {
					collider.rigidbody.AddExplosionForce(explosionForce, this.transform.position, explosionRadius, 3.0F);
				}
				
			}

			
			if (explodePrefab) {
				GameObject tempExplosion = (GameObject)Instantiate(explodePrefab, transform.position, transform.rotation);	
				if (tempExplosion.particleSystem) {
					tempExplosion.particleSystem.startSize += explosionRadius;
				}

				Destroy (tempExplosion, 1f);
			}

			Destroy (explodeParticle);
			Destroy(this.gameObject);
		}
	}

	public override void Attack(HealthHandler targetHealthHandler) {
		if (!ignited && Time.time > lastAttack + 1)  {
			ignited = true;
			lastAttack = Time.time;

			if (glowPrefab) {
				explodeParticle = (GameObject)Instantiate(glowPrefab, transform.position, transform.rotation);	
				explodeParticle.transform.parent = this.transform;
			}

			if (myAi) {
				myAi.movementAllowed = false;
			}

		}
	}
}
