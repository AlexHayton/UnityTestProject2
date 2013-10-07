using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

public class TriggerBase : Entity {	
		
	[SerializeField]
	public List<EntityMethodPair> targets = new List<EntityMethodPair>();	
	
	void Start () {
	}
	
	void Update () {
	}
	
	internal virtual void Trigger(Collider other){
		if (isEnabled) {		
			CallMethods();
		}	
	}
	
	internal virtual void CallMethods() {	
		// call the method via reflections invoke
		foreach (EntityMethodPair pair in targets) {
					// get the function from the string
			if (pair.method != "No Method") {	
				
				pair.target.GetType ()
    				.GetMethod(pair.method, BindingFlags.Instance |BindingFlags.NonPublic | BindingFlags.Public)
    				.Invoke(pair.target, new object[0]);
			}
		}	
	}
	
}
