using System;
using UnityEngine;

/// <summary>
/// SelfTestUtility
/// Shorthand functions to aid self-testing.
/// </summary>
public static class SelfTestUtility
{	
	public static void NotNull(ref bool fail, string varName, object variable)
	{
		if (variable == null)
		{
			Debug.Log(varName + " must not be null.");
			fail = true;
		}
	}
	
	public static void HasComponent<ComponentType>(ref bool fail, GameObject gameObject) where ComponentType : Component
	{
		ComponentType component = gameObject.GetComponent<ComponentType>();
		if (component == null)
		{
			string varName = gameObject.transform.name;
			Debug.Log(varName + " must have a " + typeof(ComponentType).ToString());
			fail = true;
		}
	}
}
