using UnityEngine;
using System.Collections;
using TestProject;
using System.Collections.Generic;

[RequireComponent (typeof (CapsuleCollider))]
public abstract class Pickupable : MonoBehaviour {

	public GameObject pickUpEffectPrefab;
	protected GameObject targetPlayer;
	public bool floatsToPlayer = false;
	public float floatToPlayerRangeSquared = 9.0f;
	public float floatToPlayerSpeed = 2.0f;
	private const float PLAYER_CHECK_INTERVAL = 0.5f;
	private float nextValidityCheckTime;
	
	void Start()
	{
		CapsuleCollider _capsule = GetComponent<CapsuleCollider>();
		_capsule.isTrigger = true;
	}
	
	void  OnTriggerEnter (Collider collision) {
		if (collision.tag == "Player") {
			
			if (this.CanBePickedUpBy(collision.gameObject)) {
				
				if (this.OnPickUp(collision.gameObject))
				{					
					// EffectUtility sanity checks for us!
					EffectUtility.TryInstantiateEffectPrefab(pickUpEffectPrefab, transform.position, transform.rotation);
					
					Destroy (this.gameObject);
				}
			}
		}
	}
	
	public abstract bool CanBePickedUpBy(GameObject player);
	
	void Update()
	{
		if (Time.time > this.nextValidityCheckTime)
		{
			IEnumerable<GameObject> players = PlayerUtility.GetAllPlayersInClosestOrder(this.transform.position);
			IEnumerator<GameObject> enumerator = players.GetEnumerator();

			targetPlayer = null;
			while (targetPlayer == null && enumerator.MoveNext())
			{
				GameObject thisPlayer = enumerator.Current;
				
				// if we fail a distance check we'll never find a close enough player
				if (!this.IsNearEnoughToPickup(thisPlayer))
				{
					break;
				}

				if (this.CanBePickedUpBy(thisPlayer))
				{
					targetPlayer = thisPlayer;
				}
			}
			
			this.nextValidityCheckTime = Time.time + PLAYER_CHECK_INTERVAL;
		}
		
		// Float towards the player if the player is near enough.
		if (targetPlayer != null && this.floatsToPlayer)
		{
			this.FloatTowardsPlayer(targetPlayer);
		}
	}
	
	private bool IsNearEnoughToPickup(GameObject player)
	{
		return (transform.position - player.transform.position).sqrMagnitude < floatToPlayerRangeSquared;
	}
	
	private void FloatTowardsPlayer(GameObject player)
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
