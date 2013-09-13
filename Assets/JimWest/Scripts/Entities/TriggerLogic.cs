using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Linq;
using System;

public class TriggerLogic : TriggerBase {	
	
	public bool onlyOnce;
	public bool onTriggerEnter;
	public bool onTriggerExit;
	public bool onTriggerStay;	
	
	internal bool triggered;	
	
	[HideInInspector]
	public string onlyOnTag;
	
	[HideInInspector]
	public int onlyOnTagIndex = 0;

	
	void Start () {
	}
	
	void Update () {
	}
	
	void OnTriggerEnter(Collider other) {
		if (onTriggerEnter) {
			Trigger(other);
		}		
    }
	
	void OnTriggerExit(Collider other) {
		if (onTriggerExit) {
			Trigger(other);
		}	
	}
		
	void OnTriggerStay(Collider other) {
		if (onTriggerStay) {
			Trigger(other);
		}	
	}
	
	internal override void Trigger (Collider other)
	{
		// check for tags
		if (!String.Equals(onlyOnTag, "All tags")) {
			if (other.CompareTag(onlyOnTag)) {
				if (!onlyOnce | onlyOnce & !triggered){
					base.Trigger (other);
					triggered = true;
				}
			}			
		} else {
			// no checks needed here
			base.Trigger (other);
		}
	}
	
	
}
