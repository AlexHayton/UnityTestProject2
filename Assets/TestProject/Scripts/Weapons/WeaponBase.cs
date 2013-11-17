using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponBase : MonoBehaviour, ISelfTest
{

    public GameObject FiringEffect;
    public GameObject BulletPrefab;
    public GameObject LaserPointer;
    public Color LaserColor;
    public Texture2D Icon;

    private LaserBase actualLaser;
    private RigidPlayerScript playerScript;
    private EnergyHandler energyHandler;
    private Transform bulletOrigin;
    [HideInInspector]
    public Transform LaserOrigin;
    private Transform attachPoint;

    public bool primary = true;
    public float Cooldown = .1f;
    public float ConeAngle = 1.5f;
    public int BulletsToCreate = 1;
    public bool showInMenu = true;
    public float EnergyCost = 1;
    public int DamageOnHit = 10;
    public float BulletSpeed = 20.0f;
    public float ForceOnImpact = 20.0f;
    private bool IsScatter = false;

    private Random rnd;

    private float lastFireTime = 0.0f;

    public virtual void Start()
    {
        lastFireTime = 0.0f;
        rnd = new Random();
        var playerCapsule = GameObject.FindGameObjectWithTag("Player");
        playerScript = playerCapsule.GetComponent<RigidPlayerScript>();
        energyHandler = playerCapsule.GetComponent<EnergyHandler>();
        attachPoint = transform.FindChild("GripPoint");
        bulletOrigin = transform.FindChild("BarrelEnd");
        LaserOrigin = transform.FindChild("LaserOrigin");
        GameObject laserObject = Instantiate(LaserPointer, LaserOrigin.position, Quaternion.identity) as GameObject;
        actualLaser = laserObject.GetComponent<LaserBase>();
        actualLaser.SetOrigin(LaserOrigin.transform);
        laserObject.transform.parent = LaserOrigin;
        var playerGrip = playerCapsule.transform.FindChild("group1").FindChild("PlayerGrabPoint");
        transform.parent = playerGrip.transform;
        transform.rotation = playerCapsule.transform.rotation;
        transform.position = transform.position + (playerGrip.position - attachPoint.position);
        energyHandler = gameObject.transform.root.GetComponentInChildren<EnergyHandler>();
        laserObject.renderer.material.color = LaserColor;
    }

    public bool SelfTest()
    {
        bool fail = false;
        // Check that we have a particleSystem and bulletBase in our prefabs
        SelfTestUtility.HasComponent<BulletBase>(ref fail, this.BulletPrefab);
        // Check that we have the right components on the player script.
        SelfTestUtility.NotNull(ref fail, this, "playerScript");
        SelfTestUtility.HasComponent<XPHandler>(ref fail, playerScript.gameObject);
        SelfTestUtility.HasComponent<EnergyHandler>(ref fail, playerScript.gameObject);
        return fail;
    }

    public Texture2D GetIcon()
    {
        return this.Icon;
    }

    public void Update()
    {
        var arrayOfRays = new List<Ray>();
        var arrayOfManyRayHits = new List<List<RaycastHit>>();
        var chosenRay = new Ray();
        var closestEnemyHit = new RaycastHit();

        for (var i = -20; i <= 20; i += 5)
        {
            var thisRay = new Ray(bulletOrigin.position, Quaternion.AngleAxis(i, -transform.parent.right) * transform.parent.forward);
            arrayOfRays.Add(thisRay);

            var hitsOnThisRay = Physics.RaycastAll(thisRay).Where(a => !a.transform.gameObject.CompareTag("Bullet")).ToList();
            if (!hitsOnThisRay.Any())
                continue;
            arrayOfManyRayHits.Add(hitsOnThisRay);
            var closestHitOnThisRay = hitsOnThisRay.First(a => a.distance == hitsOnThisRay.Min(b => b.distance));
            if (closestHitOnThisRay.transform.gameObject.GetComponent<AIBase>() != null &&
                (closestHitOnThisRay.distance < closestEnemyHit.distance || closestEnemyHit.Equals(default(RaycastHit))))
            {
                closestEnemyHit = closestHitOnThisRay;
                chosenRay = new Ray(bulletOrigin.position, closestEnemyHit.transform.position - bulletOrigin.position);
                Debug.DrawRay(chosenRay.origin, chosenRay.direction, Color.red);
            }
        }

        //no enemies found
        if (closestEnemyHit.Equals(default(RaycastHit)))
        {
            transform.rotation = Quaternion.LookRotation(transform.parent.forward, transform.parent.up);
        }
        else
        {
            var targetRotation = Quaternion.LookRotation(closestEnemyHit.transform.position - transform.position, transform.parent.up);
            transform.rotation = Quaternion.Euler(targetRotation.eulerAngles.x, transform.parent.eulerAngles.y, transform.parent.eulerAngles.z);

        }

    }

    public void Fire()
    {
        if (Time.time - lastFireTime > Cooldown && energyHandler.GetEnergy() > EnergyCost)
        {
            // forward vector
            var direction = transform.forward.normalized;

            // Spawn visual bullet	and set values for start				
            for (int i = 0; i < BulletsToCreate; i++)
            {
                // apply scatter
                var dirWithConeRandomization = direction + new Vector3(Random.Range(-ConeAngle, ConeAngle), 0, Random.Range(-ConeAngle, ConeAngle));
                float spreadAngle = Vector3.Angle(direction, dirWithConeRandomization);
                Quaternion tempRot = BulletPrefab.transform.rotation;
                tempRot.SetFromToRotation(BulletPrefab.transform.forward, dirWithConeRandomization);


                //tempRot.y = playerScript.transform.rotation.y;	
                //tempRot.y = transform.rotation.y;
                //tempRot.y = Quaternion.FromToRotation(bulletPrefab.transform.position, direction).y;
                //Debug.Log (test.eulerAngles.y);

                //tempRot.y = Quaternion.FromToRotation(transform.position, endPoint).y;
                //Quaternion coneRandomRotation = Quaternion.Euler (Random.Range (-coneAngle, coneAngle), Random.Range (-coneAngle, coneAngle), 0);
                //tempRot *= coneRandomRotation;

                //Debug.Log (tempRot.ToString ());
                //tempRot.y = transform.rotation.y;

                GameObject go = Instantiate(BulletPrefab, bulletOrigin.position, tempRot) as GameObject;
                BulletBase bullet = go.GetComponent<BulletBase>();
                float actualBulletSpeed = this.BulletSpeed * Mathf.Cos(Mathf.Deg2Rad * spreadAngle);
                BulletBase.StartValues values = new BulletBase.StartValues()
                {
                    owner = playerScript.gameObject,
                    forward = dirWithConeRandomization,
                    DamageOnHit = this.DamageOnHit,
                    Speed = actualBulletSpeed,
                    ForceOnImpact = this.ForceOnImpact
                };
                bullet.SetStartValues(values);
            }

            this.playerScript.gameObject.GetComponent<EnergyHandler>().DeductEnergy(EnergyCost);

            // show visul muzzle
            if (FiringEffect != null)
            {
                ParticleSystem particleSystem = FiringEffect.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    particleSystem.Emit(1);
                }

                if (FiringEffect.animation != null)
                {
                    FiringEffect.animation.Play();
                }
            }

            lastFireTime = Time.time;
        }
    }


}

