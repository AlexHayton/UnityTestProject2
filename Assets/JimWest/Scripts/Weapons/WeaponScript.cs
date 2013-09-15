using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Weapon script.
/// This handles the available weapons and firing them.
/// </summary>
public class WeaponScript : MonoBehaviour {

	private Weapon currentWeapon;
	
	private Weapon leftWeapon;
	private Weapon rightWeapon;

	// Use this for initialization
	void Start () {
		this.leftWeapon = new Rifle();
		this.leftWeapon.Start();
		this.rightWeapon = new Rifle();
		this.rightWeapon.Start();
	}
	
	private IList<Weapon> m_allWeapons;
	private IList<Weapon> m_availableWeapons;
	private IList<Weapon> GetAllWeapons()
	{
		
	}
	private IList<Weapon> GetAvailableWeapons()
	{
		
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
