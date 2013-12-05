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
    private WeaponBase SelectedWeapon;

    [HideInInspector]
	// SelectedIndex starts at 0;
	// SelectedSlot starts at 1;
    public int selectedIndex;
    public List<WeaponBase> Weapons;
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
        if(selectedIndex == index && SelectedWeapon != null)
            return;
        selectedIndex = index;
        if (SelectedWeapon != null)
        {
            Destroy(SelectedWeapon.gameObject);
        }
        SelectedWeapon = (WeaponBase)Instantiate(Weapons[selectedIndex]);
        if (SelectedWeaponGUI)
		{
			SelectedWeaponGUI.SetSelectedWeaponIndex(index);
		}
    }

	public bool HasWeapon(GameObject weapon)
	{
		return Weapons.Any(w => w.name == weapon.name);
	}

	public bool HasWeapon(WeaponBase weapon)
	{
		return HasWeapon(weapon.gameObject);
	}

	public bool AddWeapon(WeaponBase weapon)
	{
		bool success = false;
		if (weapon != null && !HasWeapon (weapon)) {
			// Store the old weapon
			WeaponBase oldWeapon = SelectedWeapon;

			// Equip the new one.
			Weapons[selectedIndex] = weapon;
			SelectedWeapon = null;
			Equip(selectedIndex);

			// Destroy the old weapon
			oldWeapon.Drop();
			success = true;
		}
		return success;
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
	
	public WeaponBase GetWeaponInSlot(int slot)
	{
		WeaponBase weapon = null;
		
		if (Weapons.Count >= slot)
		{
			weapon = Weapons[slot-1];
		}
		
		return weapon;
	}

	public WeaponBase GetSelectedWeapon()
	{
		return SelectedWeapon;
	}

	public int GetSelectedSlot()
	{
		return selectedIndex + 1;
	}

    public void PickUpWeapon(ref WeaponBase weapon)
    {
        //in theory this should add the "reference (not as in ref, just used that so I had access to the weapon to destroy it)" to the weapon, then destroy the actual object
        Weapons.Add(weapon);
        Destroy(weapon);
    }

}
