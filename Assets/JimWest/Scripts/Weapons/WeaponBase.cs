using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
	protected GameObject tempMuzzle;
	protected ParticleSystem muzzleParticle;
	
	protected Transform muzzlePosition;
	protected GameObject muzzlePrefab;
	protected GameObject bulletPrefab;
	protected float frequency  = 10f;
	protected float coneAngle = 1.5f;
	protected bool firing = false;
	protected float damagePerSecond = 20.0f;
	protected float forcePerSecond  = 20.0f;
	
	private float lastFireTime = -1f;
	
	public virtual void Start()
	{
		GameObject tempMuzzle = (GameObject)Instantiate(muzzlePrefab, muzzlePosition.position, muzzlePosition.rotation);
		tempMuzzle.transform.parent = this.transform;
		muzzleParticle = tempMuzzle.GetComponent<ParticleSystem>();
	}
	
	public void Fire()
	{
		Quaternion tempRot = transform.parent.transform.rotation;
					
		Quaternion coneRandomRotation = Quaternion.Euler (Random.Range (-coneAngle, coneAngle), Random.Range (-coneAngle, coneAngle), 0);
		GameObject go = (GameObject)Instantiate (bulletPrefab, muzzlePosition.position, tempRot * coneRandomRotation);
		SimpleBullet bullet = go.GetComponent<SimpleBullet> ();
		
		muzzleParticle.Emit(1);
						
		lastFireTime = Time.time;
		
		// Find the object hit by the raycast
		RaycastHit hitInfo = new RaycastHit ();
		Physics.Raycast (transform.position, transform.parent.forward, out hitInfo, 100);

		if (hitInfo.transform) {
			// Get the health component of the target if any
			HealthHandler targetHealth = hitInfo.transform.GetComponent<HealthHandler> ();
			
			if (targetHealth & (hitInfo.transform.root != transform.root)) {
				// Apply damage
				targetHealth.DeductHealth(Mathf.FloorToInt(damagePerSecond / frequency));
			}
			
			// Get the rigidbody if any
			if (hitInfo.rigidbody) {
				// Apply force to the target object at the position of the hit point
				Vector3 force = transform.forward * (forcePerSecond / frequency);
				hitInfo.rigidbody.AddForceAtPosition (force, hitInfo.point, ForceMode.Impulse);
			}					
			bullet.dist = hitInfo.distance;
		}
		else {
			bullet.dist = 1000;
		}
	}
}

