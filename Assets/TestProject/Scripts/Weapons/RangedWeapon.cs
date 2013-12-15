using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TestProject;
using Debug = System.Diagnostics.Debug;
using Random = UnityEngine.Random;
 
public class RangedWeapon : Weapon  {
	public float coneAngle = 1.5f;
	public int bulletsToCreate = 1;
	public float bulletSpeed = 20.0f;
	public GameObject muzzleEffect;
	public float muzzleEffectTime = 0.1f;
	public GameObject projectilePrefab;
	public Transform bulletOrigin;

	protected MuzzleFlashBase muzzleFlash;

	// Lazy-Cache the bullet start values
	protected bool initialisedStartValues = false;
	protected BulletBase.StartValues m_BulletStartValues;
	protected BulletBase.StartValues BulletStartValues
	{
		get
		{
			if (!initialisedStartValues)
			{
				m_BulletStartValues = new BulletBase.StartValues()
				{
					owner = this.owner,
					DamageOnHit = this.damageOnHit,
					ForceOnImpact = this.forceOnImpact
				};
				initialisedStartValues = true;
			}
			return m_BulletStartValues;
		}
	}

	public override void Start() {
		base.Start ();

		if (muzzleEffect) {
			GameObject muzzleFlashObject = Instantiate(muzzleEffect) as GameObject;
			muzzleFlashObject.transform.parent = bulletOrigin;
			muzzleFlashObject.transform.localRotation = muzzleEffect.transform.rotation;
			muzzleFlashObject.transform.localPosition = muzzleEffect.transform.position;
			muzzleFlash = muzzleFlashObject.GetComponent<MuzzleFlashBase>();
		}

	}

	
	public void Update() {
		base.Update ();

		if (enemyDetector) {
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

	}

	public override bool Attack() {
		if (base.Attack ()) {

			// Spawn visual bullet	and set values for start				
			for (int i = 0; i < bulletsToCreate; i++)
			{
				this.SpawnProjectile();
			}
			
			// show visul muzzle
			if (muzzleFlash != null)
			{
				ParticleSystem particleSys = muzzleFlash.GetComponent<ParticleSystem>();
				if (particleSys != null)
				{
					particleSys.Emit(1);
				}
				
				if (muzzleFlash != null)
				{
					muzzleFlash.Fire(this.muzzleEffectTime);
				}
			}

			return true;
		}
		return false;

	}

	public void SpawnProjectile() {

		if (projectilePrefab) {
			// forward vector
			Vector3 direction = transform.forward.normalized;
			
			// apply scatter
			Vector3 dirWithConeRandomization = direction + new Vector3(Random.Range(-coneAngle, coneAngle), 0, Random.Range(-coneAngle, coneAngle));
			float spreadAngle = Vector3.Angle(direction, dirWithConeRandomization);
			Quaternion tempRot = projectilePrefab.transform.rotation;
			tempRot.SetFromToRotation(projectilePrefab.transform.forward, dirWithConeRandomization);
			
			// Set up the new bullet as efficiently as possible
			GameObject go = Instantiate(projectilePrefab, bulletOrigin.position, tempRot) as GameObject;
			BulletBase bullet = go.GetComponent<BulletBase>();
			float actualbulletSpeed = this.bulletSpeed * Mathf.Cos(Mathf.Deg2Rad * spreadAngle);
			// Use cached bullet startValues when possible
			BulletBase.StartValues values = this.BulletStartValues;
			values.forward = dirWithConeRandomization;
			values.Speed = actualbulletSpeed;
			bullet.SetStartValues(values);
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

	

}
