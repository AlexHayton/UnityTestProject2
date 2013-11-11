using UnityEngine;

public class WeaponBase : MonoBehaviour, ISelfTest
{

    public GameObject FiringEffect;
    public GameObject BulletPrefab;
    public GameObject LaserPointer;

    private LaserBase actualLaser;
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

    private Random rnd;

    public float lastFireTime;

    public virtual void Start()
    {
        lastFireTime = 0;
        rnd = new Random();
        var playerCapsule = GameObject.FindGameObjectWithTag("Player");
        playerScript = playerCapsule.GetComponent<RigidPlayerScript>();
        energyHandler = playerCapsule.GetComponent<EnergyHandler>();
        attachPoint = gameObject.FindChild("GripPoint");
        bulletOrigin = gameObject.FindChild("BarrelEnd");
        laserOrigin = gameObject.FindChild("LaserOrigin");
        GameObject laserObject = Instantiate(LaserPointer, laserOrigin.position, Quaternion.identity) as GameObject;		
		actualLaser = laserObject.GetComponent<LaserBase>();
        actualLaser.SetOrigin(laserOrigin.transform);
        actualLaser.transform.parent = laserOrigin;

        var playerGrip = playerCapsule.transform.FindChild("group1").FindChild("PlayerGrabPoint").position;
        transform.parent = playerCapsule.transform;
        transform.rotation = playerCapsule.transform.rotation;
        transform.position = transform.position + (playerGrip - attachPoint.position);
        



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

