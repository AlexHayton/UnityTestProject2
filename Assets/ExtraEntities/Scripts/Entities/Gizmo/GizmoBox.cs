using UnityEngine;
using System.Collections;

public class GizmoBox : MonoBehaviour {
	
	public Color gizmoColor = new Color(1, 0, 0, 0.5F);

	void OnDrawGizmos() {
		// apply rotation
		Gizmos.matrix = transform.localToWorldMatrix;
		
		// Draw a semitransparent blue cube at the transforms position
		Gizmos.color = gizmoColor;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
		
	}

}
