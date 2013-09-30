using UnityEngine;

public class WeaponBase : MonoBehaviour, ISelfTest
{
	RigidPlayerScript playerScript;
	protected GameObject tempMuzzle;
	protected ParticleSystem muzzleParticle;
	
	public Transform muzzlePosition;
	public GameObject muzzlePrefab;
	public GameObject bulletPrefab;
	public float frequency = 10f;
	public float coneAngle = 1.5f;
	public bool firing = false;
	public float damagePerSecond = 20.0f;
	public float forcePerSecond  = 20.0f;
	public bool showInMenu = true;
	public int cost = 100;
	
	public float lastFireTime = -1f;
	
	public virtual void Start()
	{
		this.playerScript = transform.root.GetComponentInChildren<RigidPlayerScript>(); 
		GameObject tempMuzzle = (GameObject)Instantiate(muzzlePrefab, muzzlePosition.position, muzzlePosition.rotation);
		tempMuzzle.transform.parent = this.transform;
		this.muzzleParticle = tempMuzzle.GetComponent<ParticleSystem>();
		
		this.SelfTest();
	}
	
	public bool SelfTest()
	{
		bool fail = false;
		SelfTestUtility.HasComponent<ParticleSystem>(ref fail, this.muzzlePrefab);
		SelfTestUtility.HasComponent<BulletBase>(ref fail, this.bulletPrefab);
		return fail;
	}
	
	public void Fire()
	{
		if (Time.time > ((lastFireTime + 1) / frequency))
		{
			// forward vector
			Vector3 endPoint = playerScript.GetMouseOnPlane();
			Vector3 direction = endPoint - muzzlePosition.position;
			direction.Normalize();	
			
			// apply scatter
			Quaternion tempRot = bulletPrefab.transform.rotation;			
			tempRot.SetFromToRotation(bulletPrefab.transform.forward, direction);
			//tempRot.y = playerScript.transform.rotation.y;	
			//tempRot.y = transform.rotation.y;
			//tempRot.y = Quaternion.FromToRotation(bulletPrefab.transform.position, direction).y;
			//Debug.Log (test.eulerAngles.y);
			
			//tempRot.y = Quaternion.FromToRotation(transform.position, endPoint).y;
			//Quaternion coneRandomRotation = Quaternion.Euler (Random.Range (-coneAngle, coneAngle), Random.Range (-coneAngle, coneAngle), 0);
			//tempRot *= coneRandomRotation;
			
			//Debug.Log (tempRot.ToString ());
			//tempRot.y = transform.rotation.y;

			// Spawn visual bullet	and set values for start					
			GameObject go = (GameObject)Instantiate (bulletPrefab, muzzlePosition.position, tempRot);
			BulletBase bullet = go.GetComponent<BulletBase> ();
			bullet.SetStartValues(playerScript.gameObject, direction);
			
			// show visul muzzle
			if (muzzleParticle) {
				muzzleParticle.Emit(1);									
			}
			
			lastFireTime = Time.time;
		}				
	}
}

