using UnityEngine;

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
    public virtual void SetStartValues(GameObject owner, Vector3 forward)
    {
        this.owner = owner;
        ignoreTag = owner.tag;
        rigidbody.velocity = forward.normalized * Speed * (IsScatter ? Random.Range(.5f,1.5f) : 1);
    }

    //void  OnTriggerEnter (Collider collision) {
    void OnTriggerEnter(Collider enterObj)
    {
        if (enterObj.tag != ignoreTag & enterObj.tag != "Bullet")
        {

            // add force to the object
            if (enterObj.rigidbody)
            {
                enterObj.rigidbody.AddForceAtPosition(transform.forward, enterObj.transform.position, ForceMode.Impulse);
            }

            // TODO
            // aply damage
            var health = enterObj.GetComponentInChildren<HealthHandler>();
            if (health)
            {
                health.DeductHealth(owner, DamageOnHit);
            }
            Destroy(gameObject);
        }
    }

}
