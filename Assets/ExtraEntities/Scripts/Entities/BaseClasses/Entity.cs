using UnityEngine;

public class Entity : MonoBehaviour {
	
	public bool isEnabled = true;
	
	[HideInInspector]
	public static string noFunctionString = "No function";

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[Callable]
	public virtual void Activate() {	
		isEnabled = true;
	}
	
	[Callable]
	public virtual void Deactivate() {
		isEnabled = false;
	}
	
	[Callable]
	public virtual void Toggle() {	
		isEnabled = !isEnabled;
	}

	
}
