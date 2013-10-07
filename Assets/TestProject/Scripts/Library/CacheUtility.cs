using System;
using UnityEngine;

/// <summary>
/// Cache utility.
/// This can be used to cache variables
/// </summary>
public static class CacheUtility
{	
	public static varType CacheVariable<objType, varType> ( objType self, 
															ref varType variable, 
															Func<objType, varType> lookupFunc) where objType : MonoBehaviour
	{
		if (variable != null)
		{
			return variable;
		}
		else
		{
			variable = lookupFunc(self);
			return variable;
		}
	}
}
