using System;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using TestProject;

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
		
		FieldInfo prop = null;
		// Get the field in the first nested type.
		IEnumerable<Type> types = component.GetType().GetNestedTypes().Union(new Type[] { component.GetType() });
		IEnumerable<FieldInfo> fields = types.Select(t => t.GetField(varName));
		prop = fields.Where(f => f != null).FirstOrDefault();
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
		if (gameObject.HasComponent<ComponentType>())
		{
			string varName = gameObject.transform.name;
			Debug.Log(gameObject.name + "." + typeof(ComponentType).ToString() + "." + varName + ": must have a " + typeof(ComponentType).ToString() + " component");
			fail = true;
		}
	}
}
