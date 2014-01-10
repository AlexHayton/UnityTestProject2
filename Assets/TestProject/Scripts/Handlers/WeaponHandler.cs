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
    private Weapon selectedWeapon;

    [HideInInspector] // SelectedIndex starts at 0;
    // SelectedSlot starts at 1;
    public int SelectedIndex;
    public int NumberOfSlots = 3;

    public string GrabPoint;
    public List<Weapon> Weapons;


    private GUISelectedWeapon selectedWeaponGui;

    private GUISelectedWeapon SelectedWeaponGui
    {
        get
        {
            if (selectedWeaponGui == null)
            {
                selectedWeaponGui = transform.root.GetComponentInChildren<GUISelectedWeapon>();
            }
            return selectedWeaponGui;
        }
    }

    // Use this for initialization
    void Start()
    {
        if (Weapons.Count > 0)
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
        if (SelectedIndex == index && selectedWeapon != null)
            return;
        SelectedIndex = index;
        if (selectedWeapon != null)
        {
            Destroy(selectedWeapon.gameObject);
        }

        Transform gripLocation = transform.FindChildRecursive(GrabPoint);
        if (gripLocation)
        {
            Vector3 gripPoint = gripLocation.position;
            var selectedWeaponObject = Instantiate(Weapons[SelectedIndex].gameObject, gripPoint, Quaternion.identity) as GameObject;
            selectedWeapon = selectedWeaponObject.GetComponent<Weapon>();
            selectedWeapon.transform.parent = gripLocation;
        }
        else
        {
            Debug.LogError("This entity (" + name + ") doesn't have a PlayerGrabPoint. Cannot attach weapons!");
        }

        if (SelectedWeaponGui)
        {
            SelectedWeaponGui.SetSelectedWeaponIndex(index);
        }
    }

    public bool HasWeapon(GameObject weapon)
    {
        return Weapons.Any(w => w.name == weapon.name);
    }

    public bool HasWeapon(Weapon weapon)
    {
        return HasWeapon(weapon.gameObject);
    }

    public bool AddWeapon(GameObject weapon)
    {
        bool success = false;
        if (!HasWeapon(weapon))
        {
            var weaponScript = weapon.GetComponent<Weapon>();
            if (weaponScript)
            {
                success = AddWeapon(weaponScript);
            }
        }
        return success;
    }

    public bool AddWeapon(Weapon weapon)
    {
        bool success = false;
        if (weapon != null && !HasWeapon(weapon))
        {
            int equipIndex = SelectedIndex;
            if (Weapons.Count >= NumberOfSlots)
            {
                // Destroy the old weapon in the current slot
                Weapon oldWeapon = selectedWeapon;
                this.DropWeapon(equipIndex, false);
                // Add the weapon in the current slot
                Weapons[equipIndex] = weapon;
            }
            else
            {
                equipIndex = Weapons.Count;
                Weapons[equipIndex] = weapon;
            }

            SelectedIndex = 0;
            selectedWeapon = null;
            Equip(equipIndex);

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
        if (selectedWeapon)
        {
            GameObject pickupPrefab = selectedWeapon.getPickupPrefab();
            if (pickupPrefab)
            {
                Instantiate(pickupPrefab, transform.position, transform.rotation);
            }
            Destroy(selectedWeapon.gameObject);
            Weapons[SelectedIndex] = null;
            SelectedIndex = 0;
            selectedWeapon = null;

            if (equipNext)
            {
                Weapon nextWeapon = this.Weapons.FirstOrDefault();
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
        if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire1")) & selectedWeapon)
        {
            selectedWeapon.Attack();
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

    public Weapon GetWeaponInSlot(int slot)
    {
        Weapon weapon = null;

        if (Weapons.Count >= slot)
        {
            weapon = Weapons[slot - 1];
        }

        return weapon;
    }

    public Weapon GetSelectedWeapon()
    {
        return selectedWeapon;
    }

    public int GetSelectedSlot()
    {
        return SelectedIndex + 1;
    }

}
