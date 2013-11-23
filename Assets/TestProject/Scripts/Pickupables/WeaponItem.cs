using UnityEngine;
using System.Collections;

public class WeaponItem : Pickupable {
		
	public GameObject weaponPrefab;
	private WeaponHandler _weaponHandler = null;

	public override bool CanBePickedUpBy(GameObject player)
	{
		bool canBePickedUp = false;
		
		_weaponHandler = (WeaponHandler)player.GetComponent<WeaponHandler>();
		if (_weaponHandler && this.weaponPrefab) {
			if (_weaponHandler.HasWeapon(this.weaponPrefab)) {
				canBePickedUp = true;
			}
		}
		
		return canBePickedUp;
	}
		
	public override bool OnPickUp (GameObject player)
	{
		bool success = true;

		if (_weaponHandler && this.weaponPrefab) {
			_weaponHandler.AddWeapon(this.weaponPrefab);
			success = true;
		}
		
		return success;
	}
		
}
