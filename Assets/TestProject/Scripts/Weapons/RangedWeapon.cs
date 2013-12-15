using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TestProject;
using Debug = System.Diagnostics.Debug;
using Random = UnityEngine.Random;
 
public class RangedWeapon : Weapon  {
	public float coneAngle = 1.5f;
	public int bulletsToCreate = 1;
	public float bulletSpeed = 20.0f;
	public GameObject muzzleEffect;
	public float muzzleEffectTime = 0.1f;
	public GameObject projectilePrefab;
	public Transform bulletOrigin;

	protected MuzzleFlashBase muzzleFlash;

	// Lazy-Cache the bullet start values
	protected bool initialisedStartValues = false;
	protected BulletBase.StartValues m_BulletStartValues;
	protected BulletBase.StartValues BulletStartValues
	{
		get
		{
			if (!initialisedStartValues)
			{
				m_BulletStartValues = new BulletBase.StartValues()
				{
					owner = this.owner,
					DamageOnHit = this.damageOnHit,
					ForceOnImpact = this.forceOnImpact
				};
				initialisedStartValues = true;
			}
			return m_BulletStartValues;
		}
	}

	public override void Start() {
		base.Start ();

		GameObject muzzleFlashObject = Instantiate(muzzleEffect) as GameObject;
		muzzleFlashObject.transform.parent = bulletOrigin;
		muzzleFlashObject.transform.localRotation = muzzleEffect.transform.rotation;
		muzzleFlashObject.transform.localPosition = muzzleEffect.transform.position;
		muzzleFlash = muzzleFlashObject.GetComponent<MuzzleFlashBase>();
	}

	public override bool Attack() {
		if (base.Attack ()) {

			// Spawn visual bullet	and set values for start				
			for (int i = 0; i < bulletsToCreate; i++)
			{
				this.SpawnProjectile();
			}
			
			// show visul muzzle
			if (muzzleFlash != null)
			{
				ParticleSystem particleSys = muzzleFlash.GetComponent<ParticleSystem>();
				if (particleSys != null)
				{
					particleSys.Emit(1);
				}
				
				if (muzzleFlash != null)
				{
					muzzleFlash.Fire(this.muzzleEffectTime);
				}
			}

			return true;
		}
		return false;

	}

	public void SpawnProjectile() {
		// forward vector
		Vector3 direction = transform.forward.normalized;
		
		// apply scatter
		Vector3 dirWithConeRandomization = direction + new Vector3(Random.Range(-coneAngle, coneAngle), 0, Random.Range(-coneAngle, coneAngle));
		float spreadAngle = Vector3.Angle(direction, dirWithConeRandomization);
		Quaternion tempRot = projectilePrefab.transform.rotation;
		tempRot.SetFromToRotation(projectilePrefab.transform.forward, dirWithConeRandomization);
		
		// Set up the new bullet as efficiently as possible
		GameObject go = Instantiate(projectilePrefab, bulletOrigin.position, tempRot) as GameObject;
		BulletBase bullet = go.GetComponent<BulletBase>();
		float actualbulletSpeed = this.bulletSpeed * Mathf.Cos(Mathf.Deg2Rad * spreadAngle);
		// Use cached bullet startValues when possible
		BulletBase.StartValues values = this.BulletStartValues;
		values.forward = dirWithConeRandomization;
		values.Speed = actualbulletSpeed;
		bullet.SetStartValues(values);
	}

	protected void SetProjectileValues() {
		throw new System.Exception("Not implemented");
	}

	

}
