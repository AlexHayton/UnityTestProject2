using System;
using UnityEngine;

public class DetonationWeapon : Weapon  {
	public GameObject iginitionPrefab;
	public GameObject explosionPrefab;
	protected float ignitionTime;
	protected bool ignited;

	public void Iginite() {
		throw new System.Exception("Not implemented");
	}

}
