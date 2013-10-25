using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Weapon Holder script.
/// This handles the available weapons and firing them.
/// </summary>
public class WeaponScript : MonoBehaviour, ISelfTest {

	public WeaponBase leftWeapon;
	public WeaponBase rightWeapon;
	public List<WeaponBase> allWeapons;
	private RigidPlayerScript playerScript;

	// Use this for initialization
	void Start () {
		playerScript = transform.root.GetComponentInChildren<RigidPlayerScript>(); 
	}
	
	public bool SelfTest()
	{
		bool fail = false;
		/*foreach (GameObject weapon in allWeapons)
		{
			SelfTestUtility.HasComponent<WeaponBase>(ref fail, weapon);
		}*/
		return fail;
	}
	
	private WeaponType AttachWeaponToGameObject<WeaponType>() where WeaponType : WeaponBase
	{
		WeaponType weapon = (WeaponType)this.gameObject.AddComponent(typeof(WeaponType));
		return weapon;
	}
	
	public List<WeaponBase> GetWeaponList()
	{
		return this.allWeapons.Select(w => w.GetComponent<WeaponBase>()).Where(w => w != null).ToList();
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
