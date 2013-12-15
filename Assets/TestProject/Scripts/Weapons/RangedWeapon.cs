using System;
using UnityEngine;
 
public class RangedWeapon : Weapon  {
	public GameObject muzzleEffect;
	public float muzzleEffectTime;
	public GameObject bulletPrefab;
	public float coneAngle;
	public int bulletsToCreate;
	public float bulletSpeed;
	protected String muzzleEffectObject;

	public void SpawnProjectile() {
		throw new System.Exception("Not implemented");
	}
	protected void SetProjectileValues() {
		throw new System.Exception("Not implemented");
	}

}
