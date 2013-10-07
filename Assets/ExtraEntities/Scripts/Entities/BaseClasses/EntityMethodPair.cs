using UnityEngine;

[System.Serializable]
public class EntityMethodPair {
	
	public Entity target;
	public string method = "no";
	
	[HideInInspector]
	public int index;	
}

