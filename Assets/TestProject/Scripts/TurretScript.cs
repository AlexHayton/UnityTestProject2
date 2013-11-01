using UnityEngine;
using System.Collections;

public class TurretScript : MonoBehaviour {
	
	public Transform rotateTransform;
	public WeaponBase weapon;

	// Use this for initialization
	void Start () {
		this.weapon = (WeaponBase)GetComponentInChildren<WeaponBase>();
	}
	
	// Update is called once per frame
	void Update () {
		if (this.weapon) {
			this.weapon.Fire();
		}
	}
}
