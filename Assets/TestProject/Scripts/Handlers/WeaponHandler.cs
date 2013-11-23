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

    private Vector3 gripPoint;

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
        selectedIndex = index;
        if (SelectedWeapon != null)
        {
            Destroy(SelectedWeapon.gameObject);
        }
        SelectedWeapon = (WeaponBase)Instantiate(Weapons[selectedIndex], gripPoint, Quaternion.identity);

		if (this.SelectedWeaponGUI)
		{
			SelectedWeaponGUI.SetSelectedWeaponIndex(index);
		}
    }

	public bool HasWeapon(GameObject weapon)
	{
		return Weapons.Where (w => w.name == weapon.name).Count() > 0;
	}

	public bool HasWeapon(WeaponBase weapon)
	{
		return this.HasWeapon(weapon.gameObject);
	}

	public void AddWeapon(GameObject weapon)
	{
		if (!this.HasWeapon (weapon)) {
			WeaponBase weaponScript = weapon.GetComponent<WeaponBase>();
			if (weaponScript)
			{
				this.AddWeapon(weaponScript);
			}
		}
	}

	public void AddWeapon(WeaponBase weapon)
	{
		if (!this.HasWeapon (weapon)) {
			Weapons.Add(weapon);
		}
	}
    //public List<WeaponBase> GetWeaponList()
    //{
    //    return this.PrimaryWeapons.Select(w => w.GetComponent<WeaponBase>()).Where(w => w != null).ToList();
    //}

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
			weapon = this.Weapons[slot-1];
		}
		
		return weapon;
	}

	public WeaponBase GetSelectedWeapon(WeaponBase weapon)
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
