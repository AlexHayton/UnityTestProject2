using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TestProject;
using Debug = System.Diagnostics.Debug;
using Random = UnityEngine.Random;

public abstract class Weapon : MonoBehaviour  {

	public float range = 200f;
	public int damageOnHit = 10;
	public float forceOnImpact = 20f;
	public float coolDown = .1f;
	public int weight = 10;
	public Transform gripPoint;
	public string attachPointName = "PlayerGrabPoint";
	public Texture2D icon;
	public List<AudioClip> sounds;
	public GameObject pickupPrefab;

	protected float lastAttack;
	protected GameObject owner;
	protected AudioSource audioObject;
	protected EnemyDetector enemyDetector;
	protected bool isEquipped = false;
	public bool hasSecondary { get; protected set; }

	public virtual void Start() {
		if (this.transform.parent) {
			this.owner = this.transform.parent.gameObject;
		} else {
			this.owner = this.gameObject;
		}

		gripPoint = transform.root.FindChildRecursive("PlayerGrabPoint");
		Transform attachPoint = owner.transform.FindChildRecursive(attachPointName);
		
		if (attachPoint) {
			transform.parent = attachPoint.transform;
			transform.rotation = owner.transform.rotation;
			transform.position = transform.position + (attachPoint.position - gripPoint.position);
			enemyDetector = owner.GetComponentInChildren<EnemyDetector>();
		}
	}

	public virtual void Update() {
	}

	public virtual bool IsInCooldown()
	{
		return Time.time - lastAttack < coolDown;
	}
	
	public bool Attack() {
		if (!IsInCooldown()) {
			this.PlayAttackSound();
			lastAttack = Time.time;
			return OnPrimaryAttack();
		}
		return false;
	}

	public abstract bool OnPrimaryAttack();

	protected virtual void PlayAttackSound() {
		if (sounds.Any())
		{
			AudioSource.PlayClipAtPoint(sounds[Random.Range(0, sounds.Count - 1)], transform.position);
		}
	}

	public virtual void AltAttack() {
		throw new System.Exception("Not implemented");
	}

	public Texture2D getIcon()
	{
		return this.icon;
	}
		
	public virtual GameObject getOwner() {
		return this.owner;
	}

	public virtual void setOwner(GameObject aOwner) {
		this.owner = aOwner;
	}

	public virtual GameObject getPickupPrefab()
	{
		return this.pickupPrefab;
	}

	public virtual void Drop()
	{
		if (this.pickupPrefab)
		{
			Instantiate(this.pickupPrefab, transform.position, transform.rotation);
			Destroy (this.gameObject);
		}
	}


}
