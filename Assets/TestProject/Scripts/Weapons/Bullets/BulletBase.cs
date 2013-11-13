using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class BulletBase : MonoBehaviour
{

    public GameObject DestroyPrefab;
    private StartValues values;

    private float dist = 50f;
    private GameObject owner;
    private Transform tr;
    private bool valuesSet = false;
    private string ignoreTag;

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
	
	public struct StartValues
	{
		GameObject owner;
		Vector3 forward;
		int DamageOnHit;
		float Speed;
		float ForceOnImpact;
	}

    // set the start values for the bullet
    public virtual void SetStartValues(StartValues values)
    {
        this.owner = values.owner;
		this.values = values;
        ignoreTag = values.owner.tag;
        float thisBulletSpeed = values.Speed * Random.RandomRange(.9f,1.1f);
        rigidbody.velocity = values.forward.normalized * thisBulletSpeed;
        //var stretch = thisBulletSpeed*.001f;
        //transform.localScale = new Vector3(.1f, .1f, stretch);
    }

    //void  OnTriggerEnter (Collider collision) {
    void OnTriggerEnter(Collider enterObj)
    {
        if (enterObj.tag != ignoreTag &&
            enterObj.tag != "Bullet" &&
            enterObj.tag != "Player" &&
            enterObj.tag != "NoCollide")
        {
            bool doDamage = true;
            var health = enterObj.GetComponentInChildren<HealthHandler>();
            if (health != null)
            {
                TeamHandler teamHandler = this.GetComponent<TeamHandler>();

                if (teamHandler != null)
                {
                    doDamage = health.GetCanTakeDamage(teamHandler.GetTeam());
                }
                else
                {
                    doDamage = health.GetCanTakeDamage();
                }
            }

            if (doDamage)
            {
                // add force to the object or the collider's parent.
                Transform affectedTransform = enterObj.transform;
                if (affectedTransform == null)
                {
                    affectedTransform = enterObj.transform.parent;
                }

                if (affectedTransform.rigidbody)
                {
                    Vector3 force = transform.forward;
                    force.Normalize();
                    affectedTransform.rigidbody.AddForceAtPosition(force * values.ForceOnImpact, enterObj.transform.position, ForceMode.Impulse);
                }

                // TODO
                // aply damage
                if (health)
                {
                    health.DeductHealth(owner, values.DamageOnHit);
                }
            }
            if (health == null || doDamage)
                Destroy(gameObject);
        }
    }

}
