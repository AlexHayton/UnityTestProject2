using UnityEngine;
using System.Collections;

public class GizmoDirection : MonoBehaviour {
	
	void OnDrawGizmosSelected() {
		
		float fov = 20;
		float max = 2;
		float min = 0.1f;
		
		Matrix4x4 temp = Gizmos.matrix;
		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
		Gizmos.DrawFrustum(Vector3.zero, fov, max, min, 1);
		Gizmos.matrix = temp;
	}
}
