using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {
	
	public enum State {
		OpenStart,
		Open,
		CloseStart,
		Closed,		
	}
	
	public State state = State.Closed;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (state == State.OpenStart) {
			Open ();	
		} else if (state == State.CloseStart) {
			Close ();	
		}
	}
	
	void OnCollisionEnter(Collision collision) {
    }
	
	
	public virtual void Open() {
		if (this.state != State.Open) {
			this.state = State.Open;
			animation["DoorOpen"].speed = 1.0f;
			animation.Play ("DoorOpen");
		}
	}
	
	public virtual void Close() {
		if (this.state != State.Closed) {
			this.state = State.Closed;
			// just play the animation backwards		
			animation["DoorOpen"].speed = -1.0f;
			animation["DoorOpen"].normalizedTime = animation["DoorOpen"].length;
			animation.Play ("DoorOpen");
		}
	}
}
