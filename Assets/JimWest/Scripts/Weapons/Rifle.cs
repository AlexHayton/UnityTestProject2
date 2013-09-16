using UnityEngine;
using System.Collections;
using UnityEditor;

public class Rifle : WeaponBase {
	
	// Define prefabs etc. here
	public override void Start()
	{
		bulletPrefab = Instantiate(Resources.Load("RifleBullet"));
		muzzlePrefab = Instantiate(Resources.Load("RifleMuzzle"));
		base.Start();
	}
}