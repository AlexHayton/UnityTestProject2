using UnityEngine;
using System.Collections;

public static class NavMeshUtility
{
	public static Vector3 GetRandomPoint(Vector3 position, int walkDistance) {
	    Vector3 randomDirection = Random.insideUnitSphere * walkDistance;
	    randomDirection += position;
	    NavMeshHit hit;
	    NavMesh.SamplePosition(randomDirection, out hit, walkDistance, 1);
	    return hit.position;
	}

}


