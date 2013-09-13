using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {
	
	public Transform muzzlePosition;
	public GameObject muzzlePrefab;
	public GameObject bulletPrefab;
	public float frequency  = 10f;
	public float coneAngle = 1.5f;
	public bool firing = false;
	public float damagePerSecond = 20.0f;
	public float forcePerSecond  = 20.0f;
	
	private float lastFireTime = -1f;
	private RigidPlayerScript playerScript;
	private GameObject tempMuzzle;
	private ParticleSystem muzzleParticle;

	// Use this for initialization
	void Start () {
		playerScript = transform.root.GetComponentInChildren<RigidPlayerScript>(); 
		GameObject tempMuzzle = (GameObject)Instantiate(muzzlePrefab, muzzlePosition.position, muzzlePosition.rotation);
		tempMuzzle.transform.parent = this.transform;
		muzzleParticle = tempMuzzle.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		// left mouse button click
		if (playerScript) {
			if (Input.GetButtonDown("Fire1") | Input.GetButton("Fire1")) {				
				
				if (Time.time > lastFireTime + 1 / frequency) {
					// Spawn visual bullet	
					
					//Quaternion tempRot =  Quaternion.LookRotation(playerScript.GetMouseOnPlane() - muzzlePosition.position , Vector3.forward);
					//tempRot.z = 0f;
					//tempRot.w = 0.5f;
					
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
			
		}
	}

}
