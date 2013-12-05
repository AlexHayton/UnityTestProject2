using UnityEngine;
using System.Collections;
using TestProject;

[RequireComponent (typeof (CapsuleCollider))]
[RequireComponent (typeof (UseTarget))]
public class WeaponItem : UsablePickup {
		
	public GameObject weaponPrefab;
	private WeaponHandler _weaponHandler = null;

	public override bool CanBePickedUpBy(GameObject player)
	{
		bool canBePickedUp = false;
		
		_weaponHandler = (WeaponHandler)player.GetComponent<WeaponHandler>();
		if (_weaponHandler && this.weaponPrefab) {
			if (!(_weaponHandler.HasWeapon(this.weaponPrefab))) {
				canBePickedUp = true;
			}
		}
		
		return canBePickedUp;
	}
		
	public override bool OnPickUp (GameObject player)
	{
		bool success = true;

		if (_weaponHandler && this.weaponPrefab) {

			// Create a short-lived weapon...
			WeaponBase newWeaponScript = this.weaponPrefab.GetComponent<WeaponBase>();
			success = _weaponHandler.AddWeapon(newWeaponScript);
		}
		
		return success;
	}
		
}
