using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Weapon Holder script.
/// This handles the available weapons and firing them.
/// </summary>
public class WeaponHandler : MonoBehaviour, ISelfTest
{
    [HideInInspector]
    public WeaponBase PrimaryWeapon;
    private int primaryIndex;
    public List<WeaponBase> PrimaryWeapons;
    private RigidPlayerScript playerScript;
    private Vector3 gripPoint;

    // Use this for initialization
    void Start()
    {
        playerScript = gameObject.GetComponent<RigidPlayerScript>();
        primaryIndex = 0;
        PrimaryWeapon = (WeaponBase)Instantiate(PrimaryWeapons[primaryIndex], gripPoint, Quaternion.identity);
        PrimaryWeapon.Start();
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

    private void Equip<WeaponType>() where WeaponType : WeaponBase
    {
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
                PrimaryWeapon.Fire();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                primaryIndex++;
                if (primaryIndex == PrimaryWeapons.Count)
                    primaryIndex = 0;
                PrimaryWeapon = PrimaryWeapons[primaryIndex];
            }
        }
    }

}
