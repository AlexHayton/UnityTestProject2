using UnityEngine;
using System.Collections;
using TestProject;

public class DoorScript : MonoBehaviour {
	
	public enum State {
		OpenStart,
		Open,
		CloseStart,
		Closed,		
		Locked
	}
	
	public float speed = 1.0f;
	public State state = State.Closed;
	public float stayOpenTime = 2.0f;
	public bool opensOnEnter = true;
	
	
	private float openedTime = -1f;
		
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		if (this.openedTime != -1f & Time.time > (this.openedTime + stayOpenTime)) {
			this.Close();	
		}
		
		if (this.state == State.OpenStart) {
			this.Open ();	
		} else if (this.state == State.CloseStart) {
			this.Close ();	
		}
	}
	
	
	public virtual void OnEnter(Collider other) {
		if (this.opensOnEnter && 
		    this.state != State.Locked && 
		    other.tag != "Bullet" &&
		    !other.HasComponent<EnemyDetector>()) {
			this.Open ();	
		}		
	}
	
	public virtual void Open() {
		if (this.state != State.Open) {
			this.state = State.Open;
			animation["DoorOpen"].speed = 1.0f * this.speed;
			animation.Play ("DoorOpen");						
		}		
		this.openedTime = Time.time;
	}
	
	public virtual void Close() {
		if (this.state != State.Closed) {
			this.state = State.Closed;
			// just play the animation backwards		
			animation["DoorOpen"].speed = -1.0f * this.speed;;
			animation["DoorOpen"].normalizedTime = animation["DoorOpen"].length;
			animation.Play ("DoorOpen");
			this.openedTime = -1.0f;
		}
	}
	

}
