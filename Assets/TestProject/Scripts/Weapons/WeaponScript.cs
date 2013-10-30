using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Weapon Holder script.
/// This handles the available weapons and firing them.
/// </summary>
public class WeaponScript : MonoBehaviour, ISelfTest
{

    private WeaponBase primaryWeapon;
    private WeaponBase secondaryWeapon;
    private int primaryIndex;
    private int secondaryIndex;
    public List<WeaponBase> PrimaryWeapons;
    public List<WeaponBase> SecondaryWeapons;
    private RigidPlayerScript playerScript;

    // Use this for initialization
    void Start()
    {
        playerScript = transform.root.GetComponentInChildren<RigidPlayerScript>();
        primaryIndex = 0;
        secondaryIndex = 0;
        primaryWeapon = PrimaryWeapons[0];
        secondaryWeapon = SecondaryWeapons[0];
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
                primaryWeapon.Fire();
            }
            else if (Input.GetButtonDown("Fire2") || Input.GetButton("Fire2"))
            {
                secondaryWeapon.Fire();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                primaryIndex++;
                if (primaryIndex == PrimaryWeapons.Count)
                    primaryIndex = 0;
                primaryWeapon = PrimaryWeapons[primaryIndex];
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                secondaryIndex++;
                if (secondaryIndex == SecondaryWeapons.Count)
                    secondaryIndex = 0;
                secondaryWeapon = SecondaryWeapons[secondaryIndex];
            }
        }
    }

}
