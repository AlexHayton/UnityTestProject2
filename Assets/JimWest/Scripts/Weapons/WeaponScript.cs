using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {
	
	public Transform muzzlePosition;
	public GameObject muzzlePrefab;
	public GameObject bulletPrefab;
	public float frequency  = 10f;
	public float coneAngle = 1.5f;
	
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
					
					// forward vector
					Vector3 endPoint = playerScript.GetMouseOnPlane();
					Vector3 direction = endPoint - muzzlePosition.position;
					direction.Normalize();	
					
					// apply scatter
					Quaternion tempRot = bulletPrefab.transform.rotation;
					Quaternion test = Quaternion.FromToRotation(bulletPrefab.transform.position, direction);
					Debug.Log (test.eulerAngles.y);
					
					//tempRot.y = Quaternion.FromToRotation(transform.position, endPoint).y;
					//Quaternion coneRandomRotation = Quaternion.Euler (Random.Range (-coneAngle, coneAngle), Random.Range (-coneAngle, coneAngle), 0);
					//tempRot *= coneRandomRotation;
					
					//Debug.Log (tempRot.ToString ());
					//tempRot.y = transform.rotation.y;

					// Spawn visual bullet	and set values for start					
					GameObject go = (GameObject)Instantiate (bulletPrefab, muzzlePosition.position, bulletPrefab.transform.rotation);
					BulletBase bullet = go.GetComponent<BulletBase> ();
					go.transform.RotateAround(bullet.transform.position, Vector3.up, test.eulerAngles.y);
					bullet.SetStartValues(playerScript.gameObject, direction);
					
					// show visul muzzle
					muzzleParticle.Emit(1);									
					lastFireTime = Time.time;
				}				
			}
			
		}
	}	

}
