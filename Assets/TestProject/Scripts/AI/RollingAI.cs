using UnityEngine;
using System.Collections;

public class RollingAI : AIBase {

	public override void Start() {
		base.Start ();
		m_Agent.updateRotation = false;
		m_Agent.updatePosition = false;
	}

	// Update is called once per frame
	public override void Update () {
		base.Update();
		CheckRotation();
	}


	internal void CheckRotation() {
		if (movementAllowed) {
			//this.transform.rotation.SetLookRotation(m_Agent.nextPosition);
			this.rigidbody.AddForce (m_Agent.velocity);
		}
	}
}
