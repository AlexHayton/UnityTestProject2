using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class BulletBase : MonoBehaviour
{

    public GameObject DestroyPrefab;
    protected StartValues values;
    private GameObject owner;
    private Transform tr;
    private bool valuesSet = false;
    private string ignoreTag;
    private bool alreadyHit = false;

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    protected virtual void Start()
    {
        
    }

    public struct StartValues
    {
        public GameObject owner;
        public Vector3 forward;
        public int DamageOnHit;
        public float Speed;
        public float ForceOnImpact;
    }

    // set the start values for the bullet
    public virtual void SetStartValues(StartValues values)
    {
        this.values = values;
        ignoreTag = values.owner.tag;
        float thisBulletSpeed = values.Speed * Random.Range(.9f, 1.1f);
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
                enterObj.tag != "NoCollide" && !alreadyHit)
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
                HitHandler affectedHitHandler = enterObj.GetComponent<HitHandler>();
                Vector3 force = transform.forward;
                force.Normalize();
                force = force * values.ForceOnImpact;

                if (affectedTransform == null)
                {
                    affectedTransform = enterObj.transform.parent;
                }

                if (affectedHitHandler)
                {
                    affectedHitHandler.OnHit(this.gameObject, enterObj.transform.position, force, ForceMode.Impulse);
                }
                else if (affectedTransform.rigidbody)
                {
                    affectedTransform.rigidbody.AddForceAtPosition(force, enterObj.transform.position, ForceMode.Impulse);
                }

                // TODO
                // aply damage
                if (health)
                {
                    health.DeductHealth(values.owner, values.DamageOnHit);
                }
            }
            if (health == null || doDamage)
            {
                Destroy(gameObject);
                alreadyHit = true;
            }
        }
    }

}
