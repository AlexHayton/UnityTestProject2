using System;

/// <summary>
/// Cache utility.
/// This can be used to cache variables
/// </summary>
public static class CacheUtility
{	
	public static CachedVariable<varType, objType> (this objType self, ref varType variable, Func<varType> lookupFunc)
	{
		if (variable == null)
		{
			variable = lookupFunc(self);
		}
		
		return variable;
	}
}
