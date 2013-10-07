using UnityEngine;
using System.Collections;

public class SpeedHud : BoxHud {	

	
	public override float GetWidth() {
		CharacterController controller  = player.GetComponent<CharacterController>();
		float tempWidth = width;
		float velocity = GetXZVelocity(controller.velocity);
		if (velocity > 0) {
			tempWidth /= (controller.slopeLimit / (float)GetXZVelocity(controller.velocity));
		} else {
			tempWidth = 6;	
		}
				
		return tempWidth;
	}
	
	public override Color GetColor () {
		return new Color(0,1,0,1);
	}
	
	private float GetXZVelocity(Vector3 velocity) {
		//return Mathf.Floor(Mathf.Sqrt(Mathf.Pow((float)velocity.x, 2), Mathf.Pow((float)velocity.z, 2)));
		return Mathf.Floor(Mathf.Pow (velocity.x, 2) + Mathf.Pow (velocity.z, 2));
		
	}
	

}
