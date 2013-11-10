using UnityEngine;
using System.Collections;

public class AIBase : MonoBehaviour {
	
	internal enum State{		
		Idle,		
		Attack,
		Defend,
		Retreat,
		Dead		
	}
	
	internal State myState = State.Idle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
