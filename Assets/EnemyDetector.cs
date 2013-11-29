using System.Linq;
using UnityEngine;
using System.Collections;

public class EnemyDetector : MonoBehaviour
{
    private Collider enemyDetectCollider;
    private TeamHandler.Team myTeam;
    private WeaponBase equippedWeapon;
    // Use this for initialization
    void Start()
    {
        var weaponHandler = Resources.FindObjectsOfTypeAll<WeaponHandler>().Single(a => a.transform.root == transform.root);
        if (weaponHandler != null)
        {
            equippedWeapon = weaponHandler.GetSelectedWeapon();
            myTeam = weaponHandler.gameObject.GetComponent<TeamHandler>().GetTeam();
        }
        enemyDetectCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (equippedWeapon == null)
        {
            var weaponHandler = Resources.FindObjectsOfTypeAll<WeaponHandler>().Single(a => a.transform.root == transform.root);
            if (weaponHandler != null)
            {
                equippedWeapon = weaponHandler.GetSelectedWeapon();
                myTeam = weaponHandler.gameObject.GetComponent<TeamHandler>().GetTeam();
            }
        }
        var tilt = Mathf.Cos((transform.eulerAngles.y + 45) * Mathf.Deg2Rad);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, tilt * 45);
    }

    void OnTriggerEnter(Collider col)
    {
        if (equippedWeapon != null && col.CompareTag("Enemy"))
            equippedWeapon.AddEnemyToView(col);
    }

    void OnTriggerExit(Collider col)
    {
        if (equippedWeapon != null && col.CompareTag("Enemy"))
            equippedWeapon.RemoveEnemyFromView(col);
    }
}
