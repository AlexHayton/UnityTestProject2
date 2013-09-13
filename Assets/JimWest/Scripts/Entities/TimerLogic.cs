using UnityEngine;
using System.Collections;

public class TimerLogic : TriggerBase {
	
	public float time;
	private float endTime;

	void Start () {
	
	}

	void Update () {
		if (isEnabled) {
			
			if (endTime == 0) {
				endTime = Time.realtimeSinceStartup + time;
			}
			
			if (Time.realtimeSinceStartup >= endTime) {				
				// trigger
				base.CallMethods();
				endTime = 0;
				isEnabled = false;
			}
		}
	}

	
}
