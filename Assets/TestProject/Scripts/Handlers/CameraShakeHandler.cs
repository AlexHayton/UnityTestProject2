using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RigidPlayerScript))]
public class CameraShakeHandler : MonoBehaviour {

	public float shakeAmountOnTakeDamage = 0.5f;

	private RigidPlayerScript playerScript;
	private Camera mainCamera;

	void Start () {	
		playerScript = this.gameObject.GetComponent<RigidPlayerScript>();
		if (playerScript) {
			mainCamera = playerScript.GetPlayerCamera();
		}
	}

	
	void Update () {	
	}

	public virtual void ShakeCamera(float shakeAmount) {
		// change x should be ok for the start
		// camera will automatically focus player again, so just shake it
		if (mainCamera)  {
			Vector3 tempPos = mainCamera.transform.position;
			tempPos.x += Random.Range (-shakeAmount,shakeAmount);		
			mainCamera.transform.position = tempPos;
		}
	}

	public void OnTakeDamage(GameObject attacker) {
		ShakeCamera(shakeAmountOnTakeDamage);
	}
}
