using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TestProject;

public class WeaponBase : MonoBehaviour, ISelfTest
{

    public GameObject FiringEffect;
    public GameObject BulletPrefab;
	public List<GameObject> bulletSounds = new List<GameObject>();
    public GameObject LaserPointer;
    public Color LaserColor;
    public Texture2D Icon;

	private MuzzleFlashBase muzzleFlash;
    private LaserBase actualLaser;
    private RigidPlayerScript playerScript;
    private EnergyHandler energyHandler;
    private Transform bulletOrigin;
    [HideInInspector]
    public Transform LaserOrigin;
    protected Transform attachPoint;
    protected GameObject autoAimTarget = null;
	public LayerMask enemyFilterMask;

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
		GameObject muzzleFlashObject = Instantiate(FiringEffect) as GameObject;
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
    	this.ChooseTarget();
	}
   
	protected void ChooseTarget()
	{
		autoAimTarget = TeamUtility.GetClosestEnemyEntity(this.gameObject, enemyFilterMask);
   
        //no enemies found
        if (autoAimTarget == null)
        {
            transform.rotation = Quaternion.LookRotation(transform.parent.forward, transform.parent.up);
        }
        else
        {
            Quaternion targetRotation = Quaternion.LookRotation(autoAimTarget.transform.position - transform.position, transform.parent.up);
			transform.rotation = Quaternion.Euler(autoAimTarget.transform.rotation.eulerAngles.x, transform.parent.eulerAngles.y, transform.parent.eulerAngles.z);

        }

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
			PlayShootSound();

            this.playerScript.gameObject.GetComponent<EnergyHandler>().DeductEnergy(EnergyCost);

            // show visul muzzle
            if (muzzleFlash != null)
            {
                ParticleSystem particleSystem = muzzleFlash.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    particleSystem.Emit(1);
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
    	Vector3 dirWithConeRandomization = direction + new Vector3(Random.Range(-ConeAngle, ConeAngle), 0, Random.Range(-ConeAngle, ConeAngle));
        float spreadAngle = Vector3.Angle(direction, dirWithConeRandomization);
        Quaternion tempRot = BulletPrefab.transform.rotation;
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


	private void PlayShootSound() {
		
		if (bulletSounds.Count > 0) {
			GameObject bulletSound = bulletSounds[UnityEngine.Random.Range(0, bulletSounds.Count - 1)];
			GameObject bulletSoundInstance = Instantiate(bulletSound) as GameObject;
			bulletSoundInstance.transform.parent = playerScript.transform;
			bulletSoundInstance.transform.localPosition = new Vector3(0, 0, 1);
			EffectUtility.DestroyWhenFinished(bulletSoundInstance);
		}
	}


}

