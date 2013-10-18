using System;
using UnityEngine;
using System.Reflection;

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
	
	private static object GetPropertyFromComponent(Component component, string varName)
	{
		if (component == null)
			return null;
		
		FieldInfo prop = component.GetType().GetField(varName);
		if (prop == null)
		{
			Debug.Log(component.gameObject.name + "." + component.name + "." + varName + ": Could not find this property!");
			return null;
		}
		
		object variable = prop.GetValue(component);
		return variable;
	}
	
	public static void GreaterThanZero(ref bool fail, Component component, string varName)
	{
		NotNull(ref fail, component, varName);
		
		if (!fail)
		{
			object variable = GetPropertyFromComponent(component, varName);
			if (IsNumber(variable))
			{
				if ((double)variable <= 0)
				{
					Debug.Log(component.gameObject.name + "." + component.name + "." + varName + ": must be greater than 0.");
					fail = true;
				}
			}
			else if (variable is Vector2)
			{
				Vector2 v2Variable = (Vector2)variable;
				if (v2Variable.x <= 0 || v2Variable.y <= 0)
				{
					Debug.Log(component.gameObject.name + "." + component.name + "." + varName + ": must be greater than 0.");
					fail = true;
				}
			}
		}
	}
	
	public static void NotNull(ref bool fail, Component component, string varName)
	{
		object variable = GetPropertyFromComponent(component, varName);
		if (variable == null)
		{
			Debug.Log(component.gameObject.name + "." + component.name + "." + varName + ": must not be null.");
			fail = true;
		}
	}
	
	public static void HasComponent<ComponentType>(ref bool fail, GameObject gameObject) where ComponentType : Component
	{
		ComponentType component = gameObject.GetComponent<ComponentType>();
		if (component == null)
		{
			string varName = gameObject.transform.name;
			Debug.Log(component.gameObject.name + "." + component.name + "." + varName + ": must have a " + typeof(ComponentType).ToString() + " component");
			fail = true;
		}
	}
}
