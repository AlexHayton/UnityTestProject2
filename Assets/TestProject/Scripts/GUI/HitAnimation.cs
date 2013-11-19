using UnityEngine;
using System.Collections;

public class HitAnimation : MonoBehaviour {
	
	public float blendInTime = 0.1f;
	public float blendOutTime = 0.1f;
	
	private float enableTime;

	// Use this for initialization
	void Start () {
		this.guiTexture.enabled = false;
		enableTime = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!this.guiTexture.enabled) {
			
			if (Time.realtimeSinceStartup > (enableTime + blendInTime)) {
				this.guiTexture.enabled = true;
			} 
		} else {
			if (Time.realtimeSinceStartup > (enableTime + blendInTime + blendOutTime)) {
				Destroy(this.gameObject);
			}
		}
		
	}
}
