using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RigidPlayerScript))]
public class CameraShakeHandler : MonoBehaviour {

	public float shakeAmountOnTakeDamage = 0.5f;
	public float waitTimeBetweenShakes = 0.2f;

	private RigidPlayerScript playerScript;
	private Camera mainCamera;
	private float lastShakeTime = -1f;

	void Start () {	
		playerScript = this.gameObject.GetComponent<RigidPlayerScript>();
		if (playerScript)
		{
		    mainCamera = Camera.main;
		}
	}

	
	void Update () {	
	}

	public void ShakeCamera(float shakeAmount) {
		// change x should be ok for the start
		// camera will automatically focus player again, so just shake it
		if (mainCamera & Time.time >= lastShakeTime + waitTimeBetweenShakes)  {
			Vector3 tempPos = mainCamera.transform.position;
			tempPos.x += Random.Range (-shakeAmount,shakeAmount);		
			mainCamera.transform.position = tempPos;
			lastShakeTime = Time.time;
		}
	}

	public void EarthquakeShake() {

	}

	public void OnTakeDamage(GameObject attacker) {
		ShakeCamera(shakeAmountOnTakeDamage);
	}
}
