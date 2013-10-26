using UnityEngine;

public class WeaponBase : MonoBehaviour, ISelfTest
{
	RigidPlayerScript playerScript;
	EnergyHandler energyHandler;
	protected GameObject tempMuzzle;
	protected ParticleSystem muzzleParticle;
	
	public Transform muzzlePosition;
	public GameObject muzzlePrefab;
	public GameObject bulletPrefab;
	public float frequency = 10f;
	public float coneAngle = 1.5f;
	public bool firing = false;
	public int bulletsToCreate = 1;
	public float damagePerSecond = 20.0f;
	public float forcePerSecond  = 20.0f;
	public bool showInMenu = true;
	public float energyCost = 1;
	public int cost = 100;
	
	public float lastFireTime = -1f;
	
	public virtual void Start()
	{
		this.playerScript = transform.root.GetComponentInChildren<RigidPlayerScript>(); 
		GameObject tempMuzzle = (GameObject)Instantiate(muzzlePrefab, muzzlePosition.position, muzzlePosition.rotation);
		tempMuzzle.transform.parent = this.transform;
		this.muzzleParticle = tempMuzzle.GetComponent<ParticleSystem>();
		
		if(this.SelfTest())
			energyHandler = transform.root.GetComponentInChildren<EnergyHandler>();
	}
	
	public bool SelfTest()
	{
		bool fail = false;
		// Check that we have a particleSystem and bulletBase in our prefabs
		SelfTestUtility.HasComponent<ParticleSystem>(ref fail, this.muzzlePrefab);
		SelfTestUtility.HasComponent<BulletBase>(ref fail, this.bulletPrefab);
		
		// Check that we have the right components on the player script.
		SelfTestUtility.NotNull(ref fail, this, "playerScript");
		SelfTestUtility.HasComponent<XPHandler>(ref fail, this.playerScript.gameObject);
		SelfTestUtility.HasComponent<EnergyHandler>(ref fail, this.playerScript.gameObject);
		return fail;
	}
	
	public void Fire()
	{
		if (Time.time > lastFireTime + (1.0f / frequency) && energyHandler.GetEnergy() > energyCost)
		{
			// forward vector
			Vector3 endPoint = playerScript.GetMouseOnPlane();
			Vector3 direction = endPoint - muzzlePosition.position;
			direction.y = 0;
			direction.Normalize();

			// Spawn visual bullet	and set values for start				
			for (int i = 0; i < this.bulletsToCreate; i++)
			{
				// apply scatter
				var dirWithConeRandomization = direction + new Vector3(Random.Range (-coneAngle, coneAngle), 0, Random.Range (-coneAngle, coneAngle));
				
				Quaternion tempRot = bulletPrefab.transform.rotation;			
				tempRot.SetFromToRotation(bulletPrefab.transform.forward, dirWithConeRandomization);
				
				
				//tempRot.y = playerScript.transform.rotation.y;	
				//tempRot.y = transform.rotation.y;
				//tempRot.y = Quaternion.FromToRotation(bulletPrefab.transform.position, direction).y;
				//Debug.Log (test.eulerAngles.y);
				
				//tempRot.y = Quaternion.FromToRotation(transform.position, endPoint).y;
				//Quaternion coneRandomRotation = Quaternion.Euler (Random.Range (-coneAngle, coneAngle), Random.Range (-coneAngle, coneAngle), 0);
				//tempRot *= coneRandomRotation;
				
				//Debug.Log (tempRot.ToString ());
				//tempRot.y = transform.rotation.y;
				
				GameObject go = (GameObject)Instantiate (bulletPrefab, muzzlePosition.position, tempRot);
				BulletBase bullet = go.GetComponent<BulletBase> ();
				bullet.SetStartValues(playerScript.gameObject, dirWithConeRandomization);
			}
			
			this.playerScript.gameObject.GetComponent<EnergyHandler>().DeductEnergy(this.energyCost);
			
			// show visul muzzle
			if (muzzleParticle) {
				muzzleParticle.Emit(1);									
			}
			
			lastFireTime = Time.time;
		}				
	}
}

