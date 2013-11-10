using UnityEngine;
using System.Linq.Expressions;
using System.Linq;
using System.Collections.Generic;

public class WeaponBase : MonoBehaviour, ISelfTest
{

    public GameObject FiringEffect;
    public GameObject BulletPrefab;
    public GameObject LaserPointer;
    public GameObject pointerCylinder;

    private GameObject actualLaser;
    private RigidPlayerScript playerScript;
    private EnergyHandler energyHandler;
    private Transform bulletOrigin;
    private Transform laserOrigin;
    private Transform attachPoint;

    public bool primary = true;
    public float Cooldown = .1f;
    public float ConeAngle = 1.5f;
    public int BulletsToCreate = 1;
    public bool showInMenu = true;
    public float EnergyCost = 1;

    private float originaLaserlLength;
    private float originalLaserScale;
    private Material laserMaterial;
    private GameObject laserSpot;
    public float LaserSpotSize;
    public float LaserSpotBrightness;

    private Random rnd;

    public float lastFireTime;

    public virtual void Start()
    {
        lastFireTime = 0;
        rnd = new Random();
        var playerCapsule = GameObject.FindGameObjectWithTag("Player");
        playerScript = playerCapsule.GetComponent<RigidPlayerScript>();
        energyHandler = playerCapsule.GetComponent<EnergyHandler>();
        attachPoint = transform.FindChild("GripPoint");
        bulletOrigin = transform.FindChild("BarrelEnd");
        laserOrigin = transform.FindChild("LaserOrigin");
        actualLaser = (GameObject)Instantiate(LaserPointer, laserOrigin.position, Quaternion.identity);
        actualLaser.transform.parent = laserOrigin;
        laserSpot = actualLaser.transform.GetChild(0).gameObject;
        laserSpot.transform.parent = null;
        LaserSpotSize *= .01f;
        laserSpot.transform.localScale = new Vector3(1, 1, 1) * LaserSpotSize;
        var playerGrip = playerCapsule.transform.FindChild("group1").FindChild("PlayerGrabPoint").position;

        transform.position = transform.position + (playerGrip - attachPoint.position);
        transform.parent = playerCapsule.transform;

        originalLaserScale = actualLaser.transform.localScale.z;
        originaLaserlLength = originalLaserScale * 10;
        actualLaser.transform.position += actualLaser.transform.forward * originaLaserlLength / 2;
        laserMaterial = actualLaser.GetComponent<MeshRenderer>().material;

        while (!SelfTest())
        {
            energyHandler = gameObject.transform.root.GetComponentInChildren<EnergyHandler>();
        }
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

    public void Update()
    {
        ///laser stuff
        Ray ray = new Ray(laserOrigin.position, actualLaser.transform.forward);
        var hits = new List<RaycastHit>(Physics.RaycastAll(ray));
        var nonColliderHits = hits.ToList();
        
        if (nonColliderHits.Any())
        {
            RaycastHit closestHit = nonColliderHits[0];
            foreach (var ncHit in nonColliderHits)
            {
                if (ncHit.distance < closestHit.distance)
                {
                    closestHit = ncHit;
                }
            }
            if (closestHit.distance < originaLaserlLength)
            {
                var newLength = closestHit.distance;
                actualLaser.transform.localScale = new Vector3(1, 1, newLength / originaLaserlLength * originalLaserScale);
                actualLaser.transform.position = laserOrigin.position + actualLaser.transform.forward * newLength / 2;
            }
            print("laser: " + ray.direction);
            print("normal: " + closestHit.normal);
            var cross1 = Vector3.Cross(ray.direction, closestHit.normal);
            print("cross1: " + cross1);
            var cross2 = Vector3.Cross(closestHit.normal, cross1);
            print("cross2: " + cross2);
            laserSpot.transform.rotation = Quaternion.LookRotation(cross2, closestHit.normal);
            var stretchAmount = 1 / Mathf.Abs(Vector3.Dot(ray.direction, closestHit.normal));
            laserSpot.transform.localScale = new Vector3(LaserSpotSize, LaserSpotSize, LaserSpotSize * stretchAmount);
            laserSpot.renderer.material.SetFloat("_Overbright", (LaserSpotBrightness - actualLaser.transform.localScale.z / originalLaserScale));
            laserSpot.transform.position = laserOrigin.position + laserOrigin.forward * closestHit.distance + .001f * laserSpot.transform.up;
        }
        else
        {
            laserSpot.renderer.material.SetFloat("_Overbright", 0);
            actualLaser.transform.localScale = new Vector3(1, 1, originalLaserScale);
            actualLaser.transform.position = laserOrigin.position + actualLaser.transform.forward * originaLaserlLength / 2;
        }

        actualLaser.renderer.material.mainTextureOffset = new Vector2(.5018f, 1 - actualLaser.transform.localScale.z / originalLaserScale);
        actualLaser.renderer.material.mainTextureScale = new Vector2(1, actualLaser.transform.localScale.z / originalLaserScale);


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

                GameObject go = (GameObject)Instantiate(BulletPrefab, bulletOrigin.position, tempRot);
                BulletBase bullet = go.GetComponent<BulletBase>();
                bullet.SetStartValues(playerScript.gameObject, dirWithConeRandomization);
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

