using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
public class GizmoMesh : MonoBehaviour {

	public Color gizmoColor = new Color(1, 0, 0, 0.5F);	
	private MeshFilter myMesh;
	
	void OnStart() {
		myMesh = (MeshFilter)GetComponent<MeshFilter>();		
	}

	void OnDrawGizmos()
	{

	}
}
