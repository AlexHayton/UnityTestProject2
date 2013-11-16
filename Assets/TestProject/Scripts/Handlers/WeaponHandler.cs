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
    public int selectedIndex;
    public List<WeaponBase> Weapons;
    private RigidPlayerScript playerScript;

    private Vector3 gripPoint;

    // Use this for initialization
    void Start()
    {
        playerScript = gameObject.GetComponent<RigidPlayerScript>();
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
    }
    //public List<WeaponBase> GetWeaponList()
    //{
    //    return this.PrimaryWeapons.Select(w => w.GetComponent<WeaponBase>()).Where(w => w != null).ToList();
    //}

    // Update is called once per frame
    
    void Update()
    {

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
		
		if (Weapons.Count > slot)
		{
			weapon = Weapons[slot];
		}
		
		return weapon;
	}

    public void PickUpWeapon(ref WeaponBase weapon)
    {
        //in theory this should add the "reference (not as in ref, just used that so I had access to the weapon to destroy it)" to the weapon, then destroy the actual object
        Weapons.Add(weapon);
        Destroy(weapon);
    }

}
