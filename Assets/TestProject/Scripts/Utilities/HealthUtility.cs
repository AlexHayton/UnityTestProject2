using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public static class HealthUtility {

	public static IEnumerable<HealthHandler>GetHealthHandlersInScene()
	{
		// Function isn't exactly fast so warn when we use it.
		Debug.Log ("Called GetHealthHandlersInScene!");
		return Object.FindObjectsOfType(HealthHandler); 
	}

}
	