using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TestProject;
using Debug = System.Diagnostics.Debug;
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
    private EnemyDetector enemyDetector;

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
    
    // Lazy-Cache the bullet start values
	private bool initialisedStartValues = false;
    private BulletBase.StartValues m_BulletStartValues;
    private BulletBase.StartValues BulletStartValues
    {
    	get
    	{
			if (!initialisedStartValues)
    		{
				m_BulletStartValues = new BulletBase.StartValues()
				{
					owner = playerScript.gameObject, 
					DamageOnHit = this.DamageOnHit,
					ForceOnImpact = this.ForceOnImpact
				};
				initialisedStartValues = true;
			}
			return m_BulletStartValues;
		}
    }

    private Random rnd;

    private float lastFireTime = -1000;



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
        enemyDetector = playerGrip.GetComponentInChildren<EnemyDetector>();

        var muzzleFlashObject = Instantiate(FiringEffect) as GameObject;
        muzzleFlashObject.transform.parent = bulletOrigin;
        muzzleFlashObject.transform.localRotation = FiringEffect.transform.rotation;
        muzzleFlashObject.transform.localPosition = FiringEffect.transform.position;
        muzzleFlash = muzzleFlashObject.GetComponent<MuzzleFlashBase>();
        Update();
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
        //foreach (var enemy in enemiesInFront)
        //{
        //    var dirToEnemy = (enemy.transform.position - bulletOrigin.position).normalized;
        //    var forwardDir = transform.parent.forward.normalized;
        //    var castDir = new Vector3(forwardDir.x, dirToEnemy.y, forwardDir.z);

        //    var allHits = Physics.RaycastAll(bulletOrigin.position, castDir).Where(a => !a.collider.gameObject.CompareTag("Bullet") && !a.collider.gameObject.CompareTag("NoCollide")).ToArray();
        //    if (allHits.Length == 0)
        //        continue;

        //    var minDist = allHits.Min(a => a.distance);
        //    var closestHit = allHits.First(a => a.distance == minDist);

        //    if (closestHit.collider.Equals(enemy) && (distToClostest == 0.0f || closestHit.distance < distToClostest))
        //    {
        //        chosenEnemy = enemy;
        //        distToClostest = closestHit.distance;
        //    }
        //}

        var enemy = GetClosestEnemyInLOS();
        if (enemy == null)
        {
            transform.rotation = Quaternion.LookRotation(transform.parent.forward, Vector3.up);
        }
        else
        {
            var targetRotation = Quaternion.LookRotation(enemy.transform.position - transform.position, Vector3.up);
            //transform.rotation = Quaternion.Euler(targetRotation.eulerAngles.x, transform.parent.eulerAngles.y, transform.parent.eulerAngles.z);
            transform.rotation = Quaternion.Euler(targetRotation.eulerAngles);
        }
    }

    public T FindClosestByBinary<T>(Vector3 pos, ref IEnumerable<T> objects) where T : Collider
    {
        var splits = 0;
        float currentThreshold = (pos - objects.First().transform.position).sqrMagnitude;
        var lastThreshold = 0f;
        var enemiesInBound = objects.Count(a => (pos - a.transform.position).sqrMagnitude < currentThreshold);
        while (enemiesInBound != 1 && splits < 10)
        {
            ++splits;
            if (enemiesInBound > 1)
            {
                var tempThreshold = currentThreshold;
                currentThreshold -= Mathf.Abs(currentThreshold - lastThreshold) / 2f;
                lastThreshold = tempThreshold;
            }
            else
            {
                var tempThreshold = currentThreshold;
                currentThreshold += Mathf.Abs(currentThreshold - lastThreshold) / 2f;
                lastThreshold = tempThreshold;
            }

            enemiesInBound = objects.Count(a => (pos - a.transform.position).sqrMagnitude < currentThreshold);
        }
        return objects.First(a => (pos - a.transform.position).sqrMagnitude < currentThreshold);
    }

    private Collider GetClosestEnemyInLOS()
    {
        var inFrontNoY = transform.parent.forward;
        inFrontNoY.y = 0;
        inFrontNoY.Normalize();

        //var enemiesInFront = (from enemy in Resources.FindObjectsOfTypeAll<TeamHandler>().Where(a => !a.IsFriendly(teamHandler.GetTeam()))
        //               let vectorToEnemy = enemy.transform.position - transform.parent.position
        //               let dirToEnemyWithNoY = new Vector3(vectorToEnemy.x, 0, vectorToEnemy.z).normalized
        //               where Vector3.Dot(inFrontNoY, dirToEnemyWithNoY) > .999f
        //               select enemy.gameObject.collider);

        var enemiesInFront = enemyDetector.GetEnemiesInView();

        Collider closestEnemy = null;
        var closestEnemyDist = 0.0f;
        foreach (var enemyCol in enemiesInFront.Where(a=> a != null))
        {
            var thisDist = (enemyCol.transform.position - transform.position).sqrMagnitude;
            if (thisDist < closestEnemyDist || closestEnemy == null)
            {
                closestEnemy = enemyCol;
                closestEnemyDist = thisDist;
            }
        }

        if (closestEnemy == null)
            return null;

        var dirToEnemy = (closestEnemy.transform.position - bulletOrigin.position).normalized;
        var forwardDir = transform.parent.forward.normalized;
        var castDir = new Vector3(forwardDir.x, dirToEnemy.y, forwardDir.z);

        var allHits = Physics.RaycastAll(bulletOrigin.position, dirToEnemy).Where(a => !a.collider.gameObject.CompareTag("Bullet") && !a.collider.gameObject.CompareTag("NoCollide"));

        var closestHit = new RaycastHit();
        foreach (var raycastHit in allHits)
        {
            if (raycastHit.distance < closestHit.distance || closestHit.Equals(default(RaycastHit)))
            {
                closestHit = raycastHit;
            }
        }

        return closestHit.collider.GetComponent<TeamHandler>() != null ? closestHit.collider : null;
    }

    public void Fire()
    {
        if (Time.time - lastFireTime > Cooldown && energyHandler.GetEnergy() > EnergyCost)
        {
            // Spawn visual bullet	and set values for start				
            for (int i = 0; i < BulletsToCreate; i++)
            {
                this.SpawnBullet();
            }
            this.PlayShootSound();

            playerScript.gameObject.GetComponent<EnergyHandler>().DeductEnergy(EnergyCost);

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
    
    private void SpawnBullet()
    {
		// forward vector
		Vector3 direction = transform.forward.normalized;

    	// apply scatter
        var dirWithConeRandomization = direction + new Vector3(Random.Range(-ConeAngle, ConeAngle), 0, Random.Range(-ConeAngle, ConeAngle));
        var spreadAngle = Vector3.Angle(direction, dirWithConeRandomization);
        var tempRot = BulletPrefab.transform.rotation;
        tempRot.SetFromToRotation(BulletPrefab.transform.forward, dirWithConeRandomization);

		// Set up the new bullet as efficiently as possible
        GameObject go = Instantiate(BulletPrefab, bulletOrigin.position, tempRot) as GameObject;
        BulletBase bullet = go.GetComponent<BulletBase>();
        float actualBulletSpeed = this.BulletSpeed * Mathf.Cos(Mathf.Deg2Rad * spreadAngle);
        // Use cached bullet startValues when possible
        BulletBase.StartValues values = this.BulletStartValues;
        values.forward = dirWithConeRandomization;
        values.Speed = actualBulletSpeed;
        bullet.SetStartValues(values);
    }


    private void PlayShootSound()
    {
        if (BulletSounds.Any())
        {
            AudioSource.PlayClipAtPoint(BulletSounds[Random.Range(0, BulletSounds.Count - 1)], bulletOrigin.position);
        }
    }


}