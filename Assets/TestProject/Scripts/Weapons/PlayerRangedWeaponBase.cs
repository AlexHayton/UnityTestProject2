using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TestProject;
using Debug = System.Diagnostics.Debug;
using Random = UnityEngine.Random;

public class PlayerRangedWeaponBase : RangedWeaponBase, ISelfTest
{

	public Texture2D Icon;
	private RigidPlayerScript playerScript;
	private EnergyHandler energyHandler;
	private EnemyDetector enemyDetector;


    void Start()
    {
		base.Start();
		var playerCapsule = this.transform.parent;
        playerScript = playerCapsule.GetComponent<RigidPlayerScript>();
        energyHandler = playerCapsule.GetComponent<EnergyHandler>();

		var playerGrip = playerCapsule.transform.FindChildRecursive("PlayerGrabPoint");
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

		Collider closestEnemy = null;
		if (enemyDetector) {
	        var enemiesInFront = enemyDetector.GetEnemiesInView();

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

    public override bool Fire()
    {
        if (energyHandler.GetEnergy() > EnergyCost)
        {
			if (base.Fire ()) {
            	playerScript.gameObject.GetComponent<EnergyHandler>().DeductEnergy(EnergyCost);
				return true;
			}

        }
		return false;

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
