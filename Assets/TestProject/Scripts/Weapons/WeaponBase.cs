using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TestProject;
using Random = UnityEngine.Random;

public class WeaponBase : MonoBehaviour, ISelfTest
{

    public GameObject FiringEffect;
    public GameObject BulletPrefab;
    public List<AudioClip> BulletSounds;
    public GameObject LaserPointer;
    public Color LaserColor;
    public Texture2D Icon;
	
	private AudioSource SoundSource;
    private MuzzleFlashBase muzzleFlash;
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
    public float muzzleFlashTime = 0.1f;
    private bool IsScatter = false;
    private float lastFireTime;

    public virtual void Start()
    {
        var playerCapsule = GameObject.FindGameObjectWithTag("Player");
        playerScript = playerCapsule.GetComponent<RigidPlayerScript>();
        energyHandler = playerCapsule.GetComponent<EnergyHandler>();

        attachPoint = transform.FindChild("GripPoint");
        bulletOrigin = transform.FindChild("BarrelEnd");
        LaserOrigin = transform.FindChild("LaserOrigin");

        var laserObject = Instantiate(LaserPointer, LaserOrigin.position, Quaternion.identity) as GameObject;
        actualLaser = laserObject.GetComponent<LaserBase>();
        actualLaser.SetOrigin(LaserOrigin.transform);
        laserObject.transform.parent = LaserOrigin;
        laserObject.renderer.material.color = LaserColor;

        var playerGrip = playerCapsule.transform.FindChild("group1").FindChild("PlayerGrabPoint");
        transform.parent = playerGrip.transform;
        transform.rotation = playerCapsule.transform.rotation;
        transform.position = transform.position + (playerGrip.position - attachPoint.position);

        var muzzleFlashObject = Instantiate(FiringEffect) as GameObject;
        muzzleFlashObject.transform.parent = bulletOrigin;
        muzzleFlashObject.transform.localRotation = FiringEffect.transform.rotation;
        muzzleFlashObject.transform.localPosition = FiringEffect.transform.position;
        muzzleFlash = muzzleFlashObject.GetComponent<MuzzleFlashBase>();
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

        for (var i = -30; i <= 30; i += 5)
        {
            var thisRay = new Ray(bulletOrigin.position, Quaternion.AngleAxis(i, -transform.parent.right) * transform.parent.forward);
            arrayOfRays.Add(thisRay);

            var hitsOnThisRay = Physics.RaycastAll(thisRay).Where(a => !a.transform.gameObject.CompareTag("Bullet")).ToList();
            if (!hitsOnThisRay.Any())
                continue;
            arrayOfManyRayHits.Add(hitsOnThisRay);
            var closestHitOnThisRay = hitsOnThisRay.First(a => Math.Abs(a.distance - hitsOnThisRay.Min(b => b.distance)) <= Mathf.Epsilon);
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
            PlayShootSound();

            // forward vector
            var direction = transform.forward.normalized;

            // Spawn visual bullet	and set values for start				
            for (int i = 0; i < BulletsToCreate; i++)
            {

                // apply scatter
                var dirWithConeRandomization = direction + new Vector3(Random.Range(-ConeAngle, ConeAngle), 0, Random.Range(-ConeAngle, ConeAngle));
                var spreadAngle = Vector3.Angle(direction, dirWithConeRandomization);
                var tempRot = BulletPrefab.transform.rotation;
                tempRot.SetFromToRotation(BulletPrefab.transform.forward, dirWithConeRandomization);

                var go = Instantiate(BulletPrefab, bulletOrigin.position, tempRot) as GameObject;
                var bullet = go.GetComponent<BulletBase>();
                var actualBulletSpeed = BulletSpeed * Mathf.Cos(Mathf.Deg2Rad * spreadAngle);
                var values = new BulletBase.StartValues()
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
            if (muzzleFlash != null)
            {
                var particleSys = muzzleFlash.GetComponent<ParticleSystem>();
                if (particleSys != null)
                {
                    particleSys.Emit(1);
                }

                if (muzzleFlash != null)
                {
                    muzzleFlash.Fire(this.muzzleFlashTime);
                }
            }

            lastFireTime = Time.time;
        }
    }


    private void PlayShootSound()
    {
        if (BulletSounds.Any())
        {
            AudioSource.PlayClipAtPoint(BulletSounds[Random.Range(0, BulletSounds.Count - 1)], bulletOrigin.position);
        }
    }


}

