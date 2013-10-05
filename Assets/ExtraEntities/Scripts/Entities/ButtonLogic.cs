using UnityEngine;
using System.Collections;

public class ButtonLogic : TriggerBase  {
	
	public void OnPress(Collider other) {
		if (isEnabled) {
			this.Trigger(other);
		}
	}
}
