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
        Open ();
		Close ();
    }
	
	
	public virtual void Open() {
		if (this.state != State.Open) {
			this.state = State.Open;
			animation.Play ("DoorOpen");
		}
	}
	
	public virtual void Close() {
		if (this.state != State.Closed) {
			this.state = State.Closed;
			animation.Play ("DoorClose");
		}
	}
}
