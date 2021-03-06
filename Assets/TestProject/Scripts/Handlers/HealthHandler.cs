using UnityEngine;
using System.Collections;
using TestProject;

[RequireComponent(typeof(Collider))]

public class HealthHandler : MonoBehaviour
{

    public float maxHealth = 100;
    public float health = 100;
    public bool dead = false;

    public int regenHealth = 0;
    public float regenSpeed = 1f;
    private float nextRegen = 0.0f;

    public bool invincible = false;
    public GameObject destroyPrefab;

    void Start()
    {
		if (regenHealth > 0)
		{
			StartCoroutine(RegenerateHealth());
		}
    }

    void Update()
    {
    }

	public IEnumerator RegenerateHealth()
	{
		while (true)
		{
			if (health < maxHealth)
			{
				// regen health
				this.health = Mathf.Min(this.health + regenHealth, maxHealth);
			}
			yield return new WaitForSeconds(regenSpeed);
		}
	}
    
    public bool GetCanBeHealed()
    {
        return !dead;
    }
    
    public bool GetCanBeHealed(TeamHandler.Team healerTeam)
    {
        bool canBeHealed = this.GetCanBeHealed();

        // Check the teams are different
        TeamHandler teamHandler = this.GetComponent<TeamHandler>();
        if (teamHandler != null)
        {
            canBeHealed = (canBeHealed && teamHandler.GetTeam() != healerTeam);
        }

        return canBeHealed;
    }

    public bool GetCanTakeDamage()
    {
        return !dead;
    }

    public bool GetCanTakeDamage(TeamHandler.Team attackerTeam)
    {
        bool takeDamage = this.GetCanTakeDamage();

        // Check the teams are different
        TeamHandler teamHandler = this.GetComponent<TeamHandler>();
        if (teamHandler != null)
        {
            takeDamage = (takeDamage && teamHandler.GetTeam() != attackerTeam);
        }

        return takeDamage;
    }

    public float GetHealth()
    {
        return this.health;
    }

    public float GetMaxHealth()
    {
        return this.maxHealth;
    }

	public float GetHealthPercentage()
	{
		return this.health / this.maxHealth * 100.0f;
	}

    public void AddHealth(int health)
    {
		this.AddHealth((float)health);
    }

	public void AddHealth(float health)
	{
		// not greater than maxHealth
		this.health = Mathf.Min(this.health + health, maxHealth);
	}

    public bool DeductHealth(GameObject doer, int damage)
    {
        if (!invincible)
        {
            // no negative health
            this.health = Mathf.Max(this.health - damage, 0);
        }

        // send OnAttackMessage to every script so we can use it for AI etc. to every component on the gameobject
        SendMessage("OnTakeDamage", doer, SendMessageOptions.DontRequireReceiver);

        this.dead = this.health <= 0;

        if (dead & gameObject.tag != "Player")
        {

            if (doer != null)
            {
                XPHandler xphandler = doer.GetComponent<XPHandler>();
                if (xphandler != null)
                {
                    xphandler.AddXp(this.GetMaxHealth());
                }
            }

            // Spawn a pickup if the manager wants to.
            PickupDropManager pickupDropper = gameObject.GetComponent<PickupDropManager>();
            EntityUtility.DestroyGameObject(gameObject);
            if (pickupDropper)
            {
                pickupDropper.SpawnPickups();
            }

            if (destroyPrefab)
            {
                GameObject test = (GameObject)Instantiate(destroyPrefab, transform.position, transform.rotation);
                Destroy(test, 0.5f);
            }
        }

        // returns true if died
        return this.dead;
    }

    public bool GetIsAlive()
    {
        return !this.dead;
    }

}
