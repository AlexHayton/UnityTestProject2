using System;
using System.Reflection;
using System.Collections.Generic;

/// <summary>
/// Class utility.
/// Used to store all 
/// </summary>
public class ClassUtility
{
	private static IList<T> GetInstances<T>()
	{
        return (from t in Assembly.GetExecutingAssembly().GetTypes()
                       where t.BaseType == (typeof(T)) && t.GetConstructor(Type.EmptyTypes) != null
                       select (T)Activator.CreateInstance(t)).ToList();
	}
}
