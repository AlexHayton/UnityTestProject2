using System;
using UnityEngine;

/// <summary>
/// SelfTestUtility
/// Shorthand functions to aid self-testing.
/// </summary>
public static class SelfTestUtility
{	
	private static bool IsNumber(this object value)
	{
	    return value is sbyte
	            || value is byte
	            || value is short
	            || value is ushort
	            || value is int
	            || value is uint
	            || value is long
	            || value is ulong
	            || value is float
	            || value is double
	            || value is decimal;
	}
	
	public static void GreaterThanZero(ref bool fail, string varName, ValueType variable)
	{
		if (IsNumber(variable))
		{
			
			if (variable == null)
			{
				Debug.Log(varName + " must not be null.");
				fail = true;
			}
			
		}
	}
	
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
