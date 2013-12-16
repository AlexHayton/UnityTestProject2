using UnityEngine;
using System.Collections;

public class TurretScript : MonoBehaviour {
	
	public Transform rotateTransform;
	public Weapon weapon;

	// Use this for initialization
	void Start () {
		this.weapon = (Weapon)GetComponentInChildren<Weapon>();
	}
	
	// Update is called once per frame
	void Update () {
		if (this.weapon) {
			this.weapon.Attack();
		}
	}
}
