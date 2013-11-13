﻿using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class BulletBase : MonoBehaviour
{

    public GameObject DestroyPrefab;
    public int DamageOnHit = 10;
    public float Speed = 20f;
    public float ForceOnImpact = 20.0f;
    public bool IsScatter = false;


    private float dist = 50f;
    private GameObject owner;
    private Transform tr;
    private bool valuesSet = false;
    private string ignoreTag;

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    // set the start values for the bullet
    public virtual void SetStartValues(GameObject owner, Vector3 forward, float speedMult)
    {
        this.owner = owner;
        ignoreTag = owner.tag;
        var thisBulletSpeed = Speed * speedMult * Random.RandomRange(.9f,1.1f);
        rigidbody.velocity = forward.normalized * thisBulletSpeed;
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
                    affectedTransform.rigidbody.AddForceAtPosition(force * this.ForceOnImpact, enterObj.transform.position, ForceMode.Impulse);
                }

                // TODO
                // aply damage
                if (health)
                {
                    health.DeductHealth(owner, DamageOnHit);
                }
            }
            if (health == null || doDamage)
                Destroy(gameObject);
        }
    }

}
