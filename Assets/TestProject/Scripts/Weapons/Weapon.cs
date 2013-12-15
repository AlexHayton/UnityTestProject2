using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour  {
	public float range;
	public int damageOnHit;
	public float forceOnImpact;
	public float coolDown;
	public Transform gripPoint;
	public int weight;
	public Texture2D icon;
	public AudioClip[] sounds;
	protected float lastAttack;
	protected GameObject owner;
	protected AudioSource audioObject;

	public GameObject getOwner() {
		return this.owner;
	}
	public void setOwner(GameObject aOwner) {
		this.owner = aOwner;
	}
	public int getDamage() {
		throw new System.Exception("Not implemented");
	}
	public void Start() {
		throw new System.Exception("Not implemented");
	}
	public void Update() {
		throw new System.Exception("Not implemented");
	}
	public bool Attack() {
		throw new System.Exception("Not implemented");
	}
	public void AltAttack() {
		throw new System.Exception("Not implemented");
	}
	protected void PlayAttackSound() {
		throw new System.Exception("Not implemented");
	}


}
