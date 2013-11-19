using UnityEngine;
using System.Collections;
using System;
using TestProject;
	
[ExecuteInEditMode()]
public class GUIWeaponSlot : GUIContentHolder { 

	public int weaponSlot;
	
	private WeaponBase GetWeapon()
	{
		WeaponBase currentWeapon = null;
		owner = PlayerUtility.GetParentPlayer(this.gameObject);
		WeaponHandler handler = owner.GetComponent<WeaponHandler>();
		if (handler)
		{
			return handler.GetWeaponInSlot(weaponSlot);
		}
		
		return currentWeapon;
	}

	public override string GetText()
	{
		return this.weaponSlot.ToString();
	}
	
	public override Texture2D GetImage()
	{
		WeaponBase weapon = this.GetWeapon();
		if (weapon)
		{
			return weapon.GetIcon();
		}
		else
		{
			return null;
		}
	}
}

