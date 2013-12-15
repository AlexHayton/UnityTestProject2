using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TestProject;
using Debug = System.Diagnostics.Debug;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour  {
	public float range = 200f;
	public int damageOnHit = 10;
	public float forceOnImpact = 20f;
	public float coolDown = .1f;
	public int weight = 10;
	public Transform gripPoint;
	public Texture2D icon;
	public List<AudioClip> sounds;
	public GameObject pickupPrefab;

	protected float lastAttack;
	protected GameObject owner;
	protected AudioSource audioObject;

	public virtual void Start() {
		this.owner = this.transform.parent.gameObject;
	}

	public virtual void Update() {
	}

	public virtual bool Attack() {
		if (Time.time - lastAttack > coolDown) {
			this.PlayAttackSound();
			lastAttack = Time.time;
			return true;
		}
		return false;
	}

	public virtual void AltAttack() {
		throw new System.Exception("Not implemented");
	}

	protected virtual void PlayAttackSound() {
		if (sounds.Any())
		{
			AudioSource.PlayClipAtPoint(sounds[Random.Range(0, sounds.Count - 1)], transform.position);
		}
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
			Destroy (this);
		}
	}


}
