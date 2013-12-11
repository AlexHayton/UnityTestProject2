using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TestProject;

/// <summary>
/// Weapon Holder script.
/// This handles the available weapons and firing them.
/// </summary>
public class WeaponHandler : MonoBehaviour, ISelfTest
{
    private PlayerRangedWeaponBase SelectedWeapon;

    [HideInInspector]
	// SelectedIndex starts at 0;
	// SelectedSlot starts at 1;
    public int SelectedIndex;
    public List<PlayerRangedWeaponBase> Weapons;
    private RigidPlayerScript playerScript;
	private GUISelectedWeapon _selectedWeaponGUI = null;

    private GUISelectedWeapon SelectedWeaponGUI
    {
        get
        {
            if (_selectedWeaponGUI == null)
            {
                _selectedWeaponGUI = transform.root.GetComponentInChildren<GUISelectedWeapon>();
            }
            return _selectedWeaponGUI;
        }
    }

    // Use this for initialization
    void Start()
    {
        this.playerScript = gameObject.GetComponent<RigidPlayerScript>();
        if(Weapons.Count > 0)
            Equip(0);
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

    private void Equip(int index)
    {
        if(SelectedIndex == index && SelectedWeapon != null)
            return;
        SelectedIndex = index;
        if (SelectedWeapon != null)
        {
            Destroy(SelectedWeapon.gameObject);
        }

		Transform gripLocation = transform.FindChildRecursive("PlayerGrabPoint");
		if (gripLocation)
		{
			Vector3 gripPoint = gripLocation.position;
			SelectedWeapon = (PlayerRangedWeaponBase)Instantiate(Weapons[SelectedIndex], gripPoint, Quaternion.identity);
			SelectedWeapon.transform.parent = this.gameObject.transform;
		}
		else
		{
			Debug.LogError("This entity (" + name + ") doesn't have a PlayerGrabPoint. Cannot attach weapons!");
		}

        if (SelectedWeaponGUI)
		{
			SelectedWeaponGUI.SetSelectedWeaponIndex(index);
		}
    }

	public bool HasWeapon(GameObject weapon)
	{
		return Weapons.Any(w => w.name == weapon.name);
	}

	public bool HasWeapon(PlayerRangedWeaponBase weapon)
	{
		return HasWeapon(weapon.gameObject);
	}

	public bool AddWeapon(GameObject weapon)
	{
		bool success = false;
		if (!HasWeapon (weapon)) 
		{
			var weaponScript = weapon.GetComponent<PlayerRangedWeaponBase>();
			if (weaponScript)
			{
				success = AddWeapon(weaponScript);
			}
		}
		return success;
	}

	public bool AddWeapon(PlayerRangedWeaponBase weapon)
	{
		bool success = false;
		if (weapon != null && !HasWeapon (weapon)) 
		{
			// Destroy the old weapon
			RangedWeaponBase oldWeapon = SelectedWeapon;
			this.DropWeapon(SelectedIndex, false);

			// Equip the new one.
			Weapons[SelectedIndex] = weapon;
			SelectedIndex = 0;
			SelectedWeapon = null;
			Equip(SelectedIndex);

			// Destroy the old weapon
			success = true;
		}
		return success;
	}

	public void DropWeapon()
	{
		this.DropWeapon(SelectedIndex, true);
	}

	private void DropWeapon(int index, bool equipNext)
	{
		if (SelectedWeapon)
		{
			GameObject pickupPrefab = SelectedWeapon.GetPickupPrefab();
			if (pickupPrefab)
			{
				Instantiate(pickupPrefab, transform.position, transform.rotation);
			}
			Weapons[SelectedIndex] = null;
			SelectedIndex = 0;
			Destroy (SelectedWeapon);
			SelectedWeapon = null;

			if (equipNext)
			{
				PlayerRangedWeaponBase nextWeapon = this.Weapons.FirstOrDefault();
				if (nextWeapon)
				{
					int newIndex = this.Weapons.IndexOf(nextWeapon);
					Equip(newIndex);
				}
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
        if (playerScript.LaserTransform == null)
            playerScript.LaserTransform = SelectedWeapon.LaserOrigin;
        if (playerScript)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButton("Fire1"))
            {
                SelectedWeapon.Fire();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (Weapons.Count > 0)
                {
                    Equip(0);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (Weapons.Count > 1)
                {
                    Equip(1);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (Weapons.Count > 2)
                {
                    Equip(2);
                }
            }
        }
    }
	
	public PlayerRangedWeaponBase GetWeaponInSlot(int slot)
	{
		PlayerRangedWeaponBase weapon = null;
		
		if (Weapons.Count >= slot)
		{
			weapon = Weapons[slot-1];
		}
		
		return weapon;
	}

	public PlayerRangedWeaponBase GetSelectedWeapon()
	{
		return SelectedWeapon;
	}

	public int GetSelectedSlot()
	{
		return SelectedIndex + 1;
	}

    public void PickUpWeapon(ref PlayerRangedWeaponBase weapon)
    {
        //in theory this should add the "reference (not as in ref, just used that so I had access to the weapon to destroy it)" to the weapon, then destroy the actual object
        Weapons.Add(weapon);
        Destroy(weapon);
    }

}
