using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Weapon Holder script.
/// This handles the available weapons and firing them.
/// </summary>
public class WeaponScript : MonoBehaviour {

	private WeaponBase leftWeapon;
	private WeaponBase rightWeapon;
	private RigidPlayerScript playerScript;

	// Use this for initialization
	void Start () {
		playerScript = transform.root.GetComponentInChildren<RigidPlayerScript>(); 
		this.leftWeapon = this.AttachWeaponToGameObject<Rifle>();
		this.rightWeapon = this.AttachWeaponToGameObject<Rifle>();
	}
	
	private IList<WeaponBase> m_availableWeapons;
	private IList<WeaponBase> GetAvailableWeapons()
	{
		// Gets a collection of all weapons and puts it in a static var.
		return CacheUtility.CacheVariable<WeaponScript, IList<WeaponBase>>(this, 
			ref m_availableWeapons, 
			delegate(WeaponScript self)
			{
				return ClassUtility.GetInstances<WeaponBase>();
			});
	}
	
	private WeaponType AttachWeaponToGameObject<WeaponType>() where WeaponType : WeaponBase
	{
		WeaponType weapon = (WeaponType)this.gameObject.AddComponent(typeof(WeaponType));
		return weapon;
	}
	
	// Update is called once per frame
	void Update () {
		// left mouse button click
		if (playerScript) {
			if (Input.GetButtonDown("Fire1") | Input.GetButton("Fire1")) {				
				this.leftWeapon.Fire ();
			}
			else if (Input.GetButtonDown("Fire2") | Input.GetButton("Fire2")) {			
				this.rightWeapon.Fire ();
			}
		}
	}

}
