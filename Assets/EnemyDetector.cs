using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    private WeaponBase equippedWeapon;
    private List<Collider> enemiesInView;

    // Use this for initialization
    void Start()
    {
        enemiesInView = new List<Collider>();
        var weaponHandler = Resources.FindObjectsOfTypeAll<WeaponHandler>().Single(a => a.transform.root == transform.root);
        if (weaponHandler != null)
        {
            equippedWeapon = weaponHandler.GetSelectedWeapon();
        }
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
            }
        }
        enemiesInView.RemoveAll(a => a == null);
        var tilt = Mathf.Cos((transform.eulerAngles.y + 45) * Mathf.Deg2Rad);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, tilt * 45);
    }

    public List<Collider> GetEnemiesInView()
    {
        return enemiesInView;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemy"))
            enemiesInView.Add(col);
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Enemy"))
            enemiesInView.Remove(col);
    }
}
