using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class GrenadeBase : BulletBase
{
    public float VelocityImpactThreshold;
    public float TimeDelay;
    public float DamageRadius;
    private float damageRadiusSquared;

    private float creationTime;

    protected override void Start()
    {
        base.Start();
        VelocityImpactThreshold *= VelocityImpactThreshold;
        creationTime = Time.time;
        damageRadiusSquared = DamageRadius*DamageRadius;
    }

    void Update()
    {
        if (Time.time - creationTime > TimeDelay)
            Explode();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy") && collision.relativeVelocity.sqrMagnitude > VelocityImpactThreshold)
            Explode();

    }

    void Explode()
    {
        Instantiate(DestroyPrefab, transform.position, Quaternion.identity);
        var allColliders = Physics.OverlapSphere(transform.position, DamageRadius);

        foreach (var allCollider in allColliders)
        {
            if (allCollider && allCollider.rigidbody)
            {
                allCollider.rigidbody.AddExplosionForce(values.DamageOnHit * 10, transform.position, DamageRadius);

                if (allCollider.CompareTag("Enemy"))
                {
                    var enemy = allCollider;
                    var damageToDo = values.DamageOnHit * (1 - (enemy.transform.position - transform.position).sqrMagnitude / damageRadiusSquared);
                    if (damageToDo < 0)
                        continue;
                    var health = enemy.GetComponentInChildren<HealthHandler>();
                    if (health != null)
                    {
                        var teamHandler = GetComponent<TeamHandler>();
                        var doDamage = (teamHandler == null)
                            ? health.GetCanTakeDamage()
                            : health.GetCanTakeDamage(teamHandler.GetTeam());

                        if (doDamage)
                        {
                            // add force to the object or the collider's parent.
                            var affectedTransform = enemy.transform;
                            var affectedHitHandler = enemy.GetComponent<HitHandler>();
                            var forceDir = (enemy.transform.position - transform.position).normalized;
                            var force = forceDir * values.ForceOnImpact;

                            if (affectedTransform == null)
                            {
                                affectedTransform = enemy.transform.parent;
                            }

                            if (affectedHitHandler)
                            {
                                affectedHitHandler.OnHit(gameObject, enemy.transform.position, force, ForceMode.Impulse);
                            }

                            else if (affectedTransform.rigidbody)
                            {
                                affectedTransform.rigidbody.AddForceAtPosition(force, enemy.transform.position,
                                    ForceMode.Impulse);
                            }

                            // TODO
                            // aply damage
                            if (health)
                            {
                                health.DeductHealth(values.owner, (int)damageToDo);
                            }
                        }
                    }
                }
            }
        }
        Destroy(gameObject);
    }
}
