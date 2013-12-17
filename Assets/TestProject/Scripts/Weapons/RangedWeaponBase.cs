using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TestProject;
using Debug = System.Diagnostics.Debug;
using Random = UnityEngine.Random;

public class RangedWeaponBase : MonoBehaviour {
	public GameObject PickupPrefab;
	public GameObject FiringEffect;
	public GameObject BulletPrefab;
	public List<AudioClip> BulletSounds;
	public GameObject LaserPointer;
	public Color LaserColor;
	public bool primary = true;
	public float Cooldown = .1f;
	public float ConeAngle = 1.5f;
	public int BulletsToCreate = 1;
	public bool showInMenu = true;
	public float EnergyCost = 1;
	public int DamageOnHit = 10;
	public float BulletSpeed = 20.0f;
	public float ForceOnImpact = 20.0f;
	public float muzzleFlashTime = 0.1f;
	internal bool IsScatter = false;
	
	[HideInInspector]
	public Transform LaserOrigin;
	protected Transform attachPoint;
	protected Transform gripPoint;
	protected Transform bulletOrigin;
	
	protected AudioSource SoundSource;
	protected MuzzleFlashBase muzzleFlash;
	protected LaserBase actualLaser;
	
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
					owner = this.transform.gameObject, 
					DamageOnHit = this.DamageOnHit,
					ForceOnImpact = this.ForceOnImpact
				};
				initialisedStartValues = true;
			}
			return m_BulletStartValues;
		}
	}
	
	protected Random rnd;	
	protected float lastFireTime = -1000;

	protected virtual void Start() {

		attachPoint = transform.FindChild("GripPoint");
		bulletOrigin = transform.FindChild("BarrelEnd");
		LaserOrigin = transform.FindChild("LaserOrigin");

		if (LaserPointer) {
			var laserObject = Instantiate(LaserPointer, LaserOrigin.position, Quaternion.identity) as GameObject;
			actualLaser = laserObject.GetComponent<LaserBase>();
			actualLaser.SetOrigin(LaserOrigin.transform);
			laserObject.transform.parent = LaserOrigin;
			laserObject.renderer.material.color = LaserColor;
		}

		/*
		var playerGrip = playerCapsule.transform.FindChildRecursive("PlayerGrabPoint");
		transform.parent = playerGrip.transform;
		transform.rotation = playerCapsule.transform.rotation;
		transform.position = transform.position + (playerGrip.position - attachPoint.position);
		*/

		var muzzleFlashObject = Instantiate(FiringEffect) as GameObject;
		muzzleFlashObject.transform.parent = bulletOrigin;
		muzzleFlashObject.transform.localRotation = FiringEffect.transform.rotation;
		muzzleFlashObject.transform.localPosition = FiringEffect.transform.position;
		muzzleFlash = muzzleFlashObject.GetComponent<MuzzleFlashBase>();
	}


	public virtual bool Fire()
	{
		if (Time.time - lastFireTime > Cooldown)
		{
			// Spawn visual bullet	and set values for start				
			for (int i = 0; i < BulletsToCreate; i++)
			{
				this.SpawnBullet();
			}
			this.PlayShootSound();

			// show visul muzzle
			if (muzzleFlash != null)
			{
				var particleSys = muzzleFlash.GetComponent<ParticleSystem>();
				if (particleSys != null)
				{
					particleSys.Emit(1);
				}
				
				if (muzzleFlash != null)
				{
					muzzleFlash.Fire(this.muzzleFlashTime);
				}
			}
			
			lastFireTime = Time.time;
			return true;
		}
		return false;
	}
	
	internal virtual void SpawnBullet()
	{
		// forward vector
		Vector3 direction = transform.forward.normalized;
		
		// apply scatter
		var dirWithConeRandomization = direction + new Vector3(Random.Range(-ConeAngle, ConeAngle), 0, Random.Range(-ConeAngle, ConeAngle));
		var spreadAngle = Vector3.Angle(direction, dirWithConeRandomization);
		var tempRot = BulletPrefab.transform.rotation;
		tempRot.SetFromToRotation(BulletPrefab.transform.forward, dirWithConeRandomization);
		
		// Set up the new bullet as efficiently as possible
		GameObject go = Instantiate(BulletPrefab, bulletOrigin.position, tempRot) as GameObject;
		BulletBase bullet = go.GetComponent<BulletBase>();
		float actualBulletSpeed = this.BulletSpeed * Mathf.Cos(Mathf.Deg2Rad * spreadAngle);
		// Use cached bullet startValues when possible
		BulletBase.StartValues values = this.BulletStartValues;
		values.forward = dirWithConeRandomization;
		values.Speed = actualBulletSpeed;
		bullet.SetStartValues(values);
	}
	
	protected void PlayShootSound()
	{
		if (BulletSounds.Any())
		{
			AudioSource.PlayClipAtPoint(BulletSounds[Random.Range(0, BulletSounds.Count - 1)], bulletOrigin.position);
		}
	}

	public GameObject GetPickupPrefab()
	{
		return this.PickupPrefab;
	}

	public void Drop()
	{
		if (this.PickupPrefab)
		{
			Instantiate(this.PickupPrefab, transform.position, transform.rotation);
			Destroy (this);
		}
	}
}
