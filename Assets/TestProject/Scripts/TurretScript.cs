using UnityEngine;
using System.Collections;

public class TurretScript : MonoBehaviour {
	
	public Transform rotateTransform;
	public PlayerRangedWeaponBase weapon;

	// Use this for initialization
	void Start () {
		this.weapon = (PlayerRangedWeaponBase)GetComponentInChildren<PlayerRangedWeaponBase>();
	}
	
	// Update is called once per frame
	void Update () {
		if (this.weapon) {
			this.weapon.Fire();
		}
	}
}
