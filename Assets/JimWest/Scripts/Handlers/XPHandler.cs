using UnityEngine;
using System.Collections;

public class XPHandler : MonoBehaviour {
	
	public float xp = 0;

	// Use this for initialization
	void Start () {
		this.xp = 0;
	}
	
	public float GetXp() {	
		return this.xp;
	}
	
	public void AddXp(float xpToAdd)
	{
		this.xp += xpToAdd;
	}

}
