using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CapsuleCollider))]
public abstract class Pickupable : MonoBehaviour {

	public GameObject pickUpEffectPrefab;
	protected GameObject player;
	public bool floatsToPlayer = false;
	public float floatToPlayerRangeSquared = 9.0f;
	public float floatToPlayerSpeed = 2.0f;
	
	void Start()
	{
		player = PlayerUtility.GetPlayer ();
	}
	
	void  OnTriggerEnter (Collider collision) {
		if (collision.tag == "Player") {
			
			if (this.CanBePickedUpBy(collision.gameObject)) {
				
				if (this.OnPickUp(collision.gameObject))
				{					
					if (pickUpEffectPrefab) {
						GameObject test = (GameObject)Instantiate(pickUpEffectPrefab, transform.position, transform.rotation);
						Destroy (test, 0.5f);	
					}
					
					Destroy (this.gameObject);
				}
			}
		}
	}
	
	public abstract bool CanBePickedUpBy(GameObject player);
	
	void Update()
	{
		// Float towards the player if the player is near enough.
		if (this.floatsToPlayer && this.CanBePickedUpBy(this.player) && this.IsNearEnoughToPickup())
		{
			this.FloatTowardsPlayer();
		}
	}
	
	private bool IsNearEnoughToPickup()
	{
		return (transform.position - player.transform.position).sqrMagnitude < floatToPlayerRangeSquared;
	}
	
	private void FloatTowardsPlayer()
	{
    	transform.position = Vector3.MoveTowards(transform.position, player.transform.position, floatToPlayerSpeed*Time.deltaTime);
	}
	
	
	void OnBecameInvisible (){
		Destroy (this.gameObject);
	}
	
	
	public virtual bool OnPickUp(GameObject player) {			
		return true; 
	}
}
